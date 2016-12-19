using BrightIdeasSoftware;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using KevinHelper;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManageCloudDrive
{
    public partial class frmMain : Form
    {
        const string MsaClientId = "148d9013-73ed-4668-befc-de991ca3563f";
        const string MsaReturnUrl = "https://login.live.com/oauth20_desktop.srf";

        static readonly string[] Scopes = { "onedrive.readonly", "wl.signin" };
        IOneDriveClient oneDriveClient;
        KFile rootLocal, rootCloud;
        KFile currentItem;
        const string DATA_ONE_DRIVE = "dataOneDrive.db";
        const string DATA_GDRIVE = "dataGDrive.db";

        List<DItem> dupList;
        string dirCompare;

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SysImageListHelper helper = new SysImageListHelper(olvFiles);
            olvColumn1.ImageGetter = delegate (object o)
            {
                var item = o as KFile;
                return helper.GetImageIndex(item.Name, item.IsFolder);
            };
            olvFiles.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                olvFiles.ListViewItemSorter = new KColumnComparer(column, order);
            };
        }
        private async void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            lbDir.Text = dirCompare = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            var dir = await JobWalkDirectories.LoadFolderAsync(dirCompare);
            rootLocal = new KFile();
            rootLocal.Children.Add(dir);
            dir.Parent = rootLocal;
            show_folder_grid(rootLocal);
        }
        private void frmMain_DragEnter(object sender, DragEventArgs e)
        {
            string folder = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            bool is_folder = Directory.Exists(folder);
            if (is_folder && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private async void rbOneDrive_CheckedChanged(object sender, EventArgs e)
        {
            string fileload = rbOneDrive.Checked ? DATA_ONE_DRIVE : DATA_GDRIVE;
            try
            {
                using (var stream = System.IO.File.Open(fileload, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();
                    rootCloud = (DItem)bformatter.Deserialize(stream);

                    show_folder_grid(rootCloud);
                }
            }
            catch (FileNotFoundException)
            {
                if (rbOneDrive.Checked)
                    await OneDrive();
                else if (rbGDrive.Checked)
                    await GDrive();
            }
        }

        private void btnSaveHash_Click(object sender, EventArgs e)
        {
            var selected = (DItem)olvFiles.SelectedObject;
            selected = selected ?? (DItem)rootCloud;

            StringBuilder fileContent = new StringBuilder();
            SaveMD5(selected, null, fileContent);

            System.IO.File.WriteAllText("res.txt", fileContent.ToString());
            MessageBox.Show("Save hash successfully!");
        }
        private void btnCheckDup_Click(object sender, EventArgs e)
        {
            dupList = new List<DItem>();
            CheckDuplicate((DItem)rootCloud);
            if (dupList.Count > 0)
                MessageBox.Show("There are some duplicated files.");
        }
        private void btnCompare_Click(object sender, EventArgs e)
        {
            ListComparer c = new ListComparer();
            var res = c.CompareFolder(rootLocal.Children[0], currentItem);
            show_folder_grid((KFile)res);
        }

        private void olvFiles_FormatRow(object sender, FormatRowEventArgs e)
        {
            KFile item = (KFile)e.Model;
            switch (item.operation)
            {
                case Operation.CHANGED:
                    e.Item.ForeColor = Color.DarkMagenta;
                    break;
                case Operation.NEW:
                    e.Item.ForeColor = Color.RoyalBlue;
                    break;
                case Operation.DELETE:
                    e.Item.ForeColor = Color.Crimson;
                    break;
            }
        }
        private void olvFiles_DoubleClick(object sender, EventArgs e)
        {
            var item = (KFile)olvFiles.SelectedObjects[0];
            if (item.Name == "..")
            {
                show_folder_grid((KFile)item.Parent);
                olvFiles.SelectedObject = currentItem;
                currentItem = (KFile)item.Parent;
            }
            else
            {
                show_folder_grid(item);
                currentItem = item;
            }
        }

        async Task GDrive()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker
                    .AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { DriveService.Scope.DriveReadonly },
                        "user",
                        CancellationToken.None,
                        new FileDataStore("oauth/drive"))
                    ;
                Console.WriteLine("Credential file saved");
            }

            // Create Drive API service.
            var driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString(),
            });

            List<DItem> listfiles = new List<DItem>();
            string pageToken = null;
            do
            {
                // Define parameters of request.
                FilesResource.ListRequest request = driveService.Files.List();
                request.Fields = "nextPageToken, files(id,name,size,mimeType,parents,webViewLink,md5Checksum,createdTime)";
                request.Spaces = "drive";
                request.Q = "trashed = false";
                request.PageToken = pageToken;
                request.PageSize = 1000;

                // List files.
                var result = await request.ExecuteAsync();
                listfiles.AddRange(result.Files.Select(f => new DItem
                {
                    Id = f.Id,
                    Name = f.Name,
                    Size = f.Size,
                    IsFolder = f.MimeType == "application/vnd.google-apps.folder",
                    Checksum = f.Md5Checksum,
                    CreatedDate = f.CreatedTime.Value,

                    ParentId = f.Parents[0],
                    WebUrl = f.WebViewLink
                }));

                pageToken = result.NextPageToken;
            }
            while (pageToken != null);

            Dictionary<string, DItem> dicFolder = listfiles.ToDictionary(i => i.Id);
            foreach (DItem item in listfiles)
            {
                if (!dicFolder.ContainsKey(item.ParentId))
                {
                    rootCloud = new DItem();
                    dicFolder[item.ParentId] = (DItem)rootCloud;
                }

                item.Parent = dicFolder[item.ParentId];
                item.Parent.Children.Add(item);
            }

            CalculateSize((DItem)rootCloud);
            show_folder_grid(rootCloud);
            SaveData(DATA_GDRIVE);
        }
        async Task OneDrive()
        {
            await get_authorize();

            rootCloud = new DItem();
            await get_children((DItem)rootCloud);
            show_folder_grid(rootCloud);

            SaveData(DATA_ONE_DRIVE);
        }
        async Task<bool> get_authorize()
        {
            var msaAuthProvider = new MsaAuthenticationProvider(
                MsaClientId,
                MsaReturnUrl,
                Scopes,
                new CredentialVault(MsaClientId)
            );
            try
            {
                await msaAuthProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                oneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", msaAuthProvider);
                return true;
            }
            catch (ServiceException ex)
            {
                MessageBox.Show(
                        ex.Error.Message,
                        "Authentication failed",
                        MessageBoxButtons.OK);
                return false;
            }
        }
        async Task get_children(DItem dItem)
        {
            IItemRequestBuilder irb = string.IsNullOrEmpty(dItem.Id) ?
                oneDriveClient.Drive.Root : oneDriveClient.Drive.Items[dItem.Id];

            try
            {
                List<Task> tasks = new List<Task>();
                var request = irb.Children.Request()
                    .Select("id,name,createdDateTime,size,file,folder,parentReference,webUrl");
                while (request != null)
                {
                    var children = await request
                        .GetAsync();

                    foreach (var item in children)
                    {
                        DItem child = new DItem
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Size = item.Size,
                            WebUrl = item.WebUrl,
                            Checksum = item.File != null && item.File.Hashes != null ? item.File.Hashes.Crc32Hash : null,
                            CreatedDate = item.CreatedDateTime.Value.DateTime,
                            Path = item.ParentReference.Path,

                            IsFolder = item.Folder != null,

                            Parent = dItem
                        };

                        dItem.Children.Add(child);

                        if (!child.IsFolder)
                            child.CountFile = 1;
                        else if (item.Folder.ChildCount > 0)
                            tasks.Add(get_children(child));
                    }

                    // next request if exceed 200 item
                    request = children.NextPageRequest;
                }

                await Task.WhenAll(tasks);

                dItem.Size = dItem.Children.Sum(i => ((KFile)i).Size);
                dItem.CountFile = dItem.Children.Sum(i => ((KFile)i).CountFile);
            }
            catch (ServiceException ex)
            {
                MessageBox.Show(ex.Error.Message);
            }
        }


        void SaveMD5(DItem item, string path, StringBuilder fileContent)
        {
            foreach (DItem child in item.Children)
            {
                if (child.IsFolder)
                    SaveMD5(child, path == null ? child.Name : path + "\\" + child.Name, fileContent);
                else
                    fileContent.AppendFormat("{0} *{1}\\{2}", child.Checksum, path, child.Name)
                        .AppendLine();
            }
        }
        void CalculateSize(DItem item)
        {
            foreach (DItem child in item.Children)
            {
                if (child.IsFolder)
                    CalculateSize(child);
                else
                    child.CountFile = 1;
            }

            item.Size = item.Children.Sum(i => ((KFile)i).Size);
            item.CountFile = item.Children.Sum(i => ((KFile)i).CountFile);
        }
        void show_folder_grid(KFile item)
        {
            if (item.Parent != null)
            {
                KFile parentFolder = new KFile { Name = "..", Parent = item.Parent };
                var list = new List<KFile> { parentFolder };
                list.AddRange(item.Children.Cast<KFile>());
                olvFiles.SetObjects(list);
            }
            else
                olvFiles.SetObjects(item.Children);
        }
        void SaveData(string name)
        {
            using (Stream stream = System.IO.File.Open(name, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, rootCloud);
            }
        }
        void CheckDuplicate(DItem item)
        {
            Dictionary<string, int> dicDup = new Dictionary<string, int>();
            foreach (DItem child in item.Children)
            {
                if (child.IsFolder)
                {
                    CheckDuplicate(child);
                    if (child.operation == Operation.CHANGED) item.operation = Operation.CHANGED;
                }
                else if (dicDup.ContainsKey(child.Name))
                {
                    dupList.Add(child);
                    item.operation = child.operation = Operation.CHANGED;
                }
                else
                    dicDup[child.Name] = 1;
            }
        }
    }

    public class KColumnComparer : IComparer
    {
        /// <summary>
        /// Gets or sets the method that will be used to compare two strings.
        /// The default is to compare on the current culture, case-insensitive
        /// </summary>
        public static StringCompareDelegate StringComparer
        {
            get { return stringComparer; }
            set { stringComparer = value; }
        }
        private static StringCompareDelegate stringComparer;

        /// <summary>
        /// Create a ColumnComparer that will order the rows in a list view according
        /// to the values in a given column
        /// </summary>
        /// <param name="col">The column whose values will be compared</param>
        /// <param name="order">The ordering for column values</param>
        public KColumnComparer(OLVColumn col, SortOrder order)
        {
            this.column = col;
            this.sortOrder = order;
        }

        /// <summary>
        /// Compare two rows
        /// </summary>
        /// <param name="x">row1</param>
        /// <param name="y">row2</param>
        /// <returns>An ordering indication: -1, 0, 1</returns>
        public int Compare(object x, object y)
        {
            return this.Compare((KFile)((OLVListItem)x).RowObject, (KFile)((OLVListItem)y).RowObject);
        }

        /// <summary>
        /// Compare two rows
        /// </summary>
        /// <param name="x">row1</param>
        /// <param name="y">row2</param>
        /// <returns>An ordering indication: -1, 0, 1</returns>
        public int Compare(KFile x, KFile y)
        {
            if (this.sortOrder == SortOrder.None)
                return 0;

            if (x.Name == "..")
                return -1;
            else if (y.Name == "..")
                return 1;

            int fol = x.IsFolder.CompareTo(y.IsFolder);
            if (fol != 0) return -fol;

            int result = 0;
            object x1 = this.column.GetValue(x);
            object y1 = this.column.GetValue(y);

            // Handle nulls. Null values come last
            bool xIsNull = (x1 == null || x1 == System.DBNull.Value);
            bool yIsNull = (y1 == null || y1 == System.DBNull.Value);
            
            if (xIsNull && yIsNull)
                result = 0;
            else if(xIsNull || yIsNull)
                result = (xIsNull ? -1 : 1);
            else
                result = this.CompareValues(x1, y1);

            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;

            return result;
        }

        /// <summary>
        /// Compare the actual values to be used for sorting
        /// </summary>
        /// <param name="x">The aspect extracted from the first row</param>
        /// <param name="y">The aspect extracted from the second row</param>
        /// <returns>An ordering indication: -1, 0, 1</returns>
        public int CompareValues(object x, object y)
        {
            // Force case insensitive compares on strings
            String xAsString = x as String;
            if (xAsString != null)
                return String.Compare(xAsString, y as String, StringComparison.CurrentCultureIgnoreCase);

            IComparable comparable = x as IComparable;
            return comparable != null ? comparable.CompareTo(y) : 0;
        }

        private OLVColumn column;
        private SortOrder sortOrder;
    }

}
