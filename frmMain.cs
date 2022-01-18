using BrightIdeasSoftware;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
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
using System.Text.RegularExpressions;
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
        UserCredential credential;
        DriveService driveService;

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

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            SysImageListHelper helper = new SysImageListHelper(olvFiles);
            olvColumn1.ImageGetter = delegate (object o)
            {
                var item = o as KFile;
                return helper.GetImageIndex(item.Name, item.IsFolder);
            };
            olvColumn3.AspectGetter = delegate (object o)
            {
                var item = (KFile)o;
                if (item.operation != Operation.NOCHANGE) return item.operation;
                return null;
            };
            olvColumn4.AspectGetter = delegate (object o)
            {
                var item = (KFile)o;
                if (item.IsFolder) return item.CountFile;
                return null;
            };
            olvFiles.CustomSorter = delegate (OLVColumn column, SortOrder order) {
                olvFiles.ListViewItemSorter = new KColumnComparer(column, order);
            };

            cbDrive.SelectedIndex = 0;
            await get_google_credential();
        }
        private async void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            lbDir.Text = dirCompare = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            var dir = await JobWalkDirectories.LoadFolderAsync(dirCompare);
            rootLocal = new KFile();
            rootLocal.Children.Add(dir);
            dir.Parent = rootLocal;
            show_folder_grid(rootLocal);

            load_local_checksum(dir);
        }
        private void frmMain_DragEnter(object sender, DragEventArgs e)
        {
            string folder = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            bool is_folder = Directory.Exists(folder);
            if (is_folder && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private async void btnLoadDrive_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string fileload = cbDrive.SelectedIndex == 0 ? DATA_GDRIVE : DATA_ONE_DRIVE;
            try
            {
                using (var stream = System.IO.File.Open(fileload, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();
                    var driveinfo = (DriveInfo)bformatter.Deserialize(stream);
                    lbUserinfo.Text = driveinfo.userinfo;
                    lbStorageamount.Text = driveinfo.storage_amount;
                    rootCloud = driveinfo.rootCloud;

                    show_folder_grid(rootCloud);

                    lbSource.Text = "Local";
                }
            }
            catch (FileNotFoundException)
            {
                if (cbDrive.SelectedIndex == 1)
                    await OneDrive();
                else if (cbDrive.SelectedIndex == 0)
                    await GDrive();
            }
            this.Cursor = Cursors.Default;
        }
        private async void btnLogout_ClickAsync(object sender, EventArgs e)
        {
            if (credential != null)
            {
                await credential.RevokeTokenAsync(CancellationToken.None);
                credential = null;
                driveService = null;
                btnClearLocal.PerformClick();
            }
        }
        private void btnClearLocal_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(DATA_GDRIVE))
                System.IO.File.Delete(DATA_GDRIVE);
            lbUserinfo.Text = lbStorageamount.Text = null;
        }


        private void btnSaveHash_Click(object sender, EventArgs e)
        {
            var selected = (DItem)olvFiles.SelectedObject;
            selected = selected ?? (DItem)rootCloud;

            StringBuilder fileContent = new StringBuilder();
            SaveMD5(selected, selected.Name ?? "", fileContent);

            System.IO.File.WriteAllText($"{selected.Name}.MD5", fileContent.ToString());
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
            c.compareFile = (itemA, itemB) =>
            {
                KFile fA = (KFile)itemA;
                KFile fB = (KFile)itemB;

                if (fA.Size != fB.Size)
                    return Operation.CHANGED;
                if (string.IsNullOrEmpty(fA.Checksum) || string.IsNullOrEmpty(fB.Checksum))
                    return Operation.UNKNOWN;
                if (fA.Checksum.Equals(fB.Checksum, StringComparison.OrdinalIgnoreCase))
                    return Operation.NOCHANGE;
                else
                    return Operation.CHANGED;
            };
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
                case Operation.UNKNOWN:
                    e.Item.ForeColor = Color.Orange;
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

        async Task get_google_credential()
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                IAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecretsStream = stream,
                    Scopes = new[] { DriveService.Scope.DriveReadonly },
                    DataStore = new FileDataStore("oauth/drive")
                });

                TokenResponse token = await flow.LoadTokenAsync(Environment.UserName, CancellationToken.None);
                if (token == null) return;

                credential = new UserCredential(flow, Environment.UserName, token);

                //bool res = await credential.RevokeTokenAsync(CancellationToken.None);
                
            }

            // Create Drive API service.
            driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString(),
            });

            var req = driveService.About.Get();
            req.Fields = "user(displayName,photoLink, me, emailAddress), storageQuota(limit,usage), maxImportSizes, maxUploadSize";
            try
            {
                About ab = await req.ExecuteAsync();
                lbUserinfo.Text = $"{ab.User.DisplayName} - {ab.User.EmailAddress}";
                lbStorageamount.Text = $"{ab.StorageQuota.Usage.ToReadableSize()}/{ab.StorageQuota.Limit.ToReadableSize()}";
            }
            catch(TokenResponseException ex)
            {
                credential = null;
                driveService = null;
                btnClearLocal.PerformClick();
            }
        }
        async Task GDrive()
        {
            if (credential == null)
            {
                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker
                    .AuthorizeAsync(
                        stream,
                        new[] { DriveService.Scope.DriveReadonly },
                        Environment.UserName,
                        CancellationToken.None,
                        new FileDataStore("oauth/drive"))
                    ;
                }

                driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = this.GetType().ToString(),
                });
            }

            var req = driveService.About.Get();
            req.Fields = "user(displayName,photoLink, me, emailAddress), storageQuota(limit,usage), maxImportSizes, maxUploadSize";
            About ab = await req.ExecuteAsync();
            lbUserinfo.Text = $"{ab.User.DisplayName} - {ab.User.EmailAddress}";
            lbStorageamount.Text = $"{ab.StorageQuota.Usage.ToReadableSize()}/{ab.StorageQuota.Limit.ToReadableSize()}";

            List<DItem> listfiles = new List<DItem>();
            string pageToken = null;
            do
            {
                // Define parameters of request.
                FilesResource.ListRequest request = driveService.Files.List();
                request.Fields = "nextPageToken, files(id,name,size,mimeType,parents,webViewLink,md5Checksum,createdTime,ownedByMe)";
                request.Spaces = "drive";
                request.Q = "(trashed = false) and ('me' in owners)";
                request.PageToken = pageToken;
                request.PageSize = 1000;

                // List files.
                var result = await request.ExecuteAsync();
                listfiles.AddRange(result.Files.Where(f => f.OwnedByMe == true).Select(f =>
                {
                    try
                    {
                        var x = new DItem
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Size = f.Size,
                            IsFolder = f.MimeType == "application/vnd.google-apps.folder",
                            Checksum = f.Md5Checksum,
                            CreatedDate = f.CreatedTime.Value,

                            ParentId = f.Parents[0],
                            WebUrl = f.WebViewLink
                        };
                        return x;
                    }
                    catch(Exception ex)
                    {
                        var a = ex;
                        return null;
                    }
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

            lbSource.Text = "Cloud";

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
                    SaveMD5(child, Path.Combine(path, child.Name), fileContent);
                else
                    fileContent.AppendLine($"{child.Checksum} *{Path.Combine(path, child.Name)}");
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
                bformatter.Serialize(stream, new DriveInfo { userinfo = lbUserinfo.Text, storage_amount = lbStorageamount.Text, rootCloud = rootCloud });
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

        private void olvFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var selection = olvFiles.SelectedObjects.Cast<KFile>().Where(f => !(f is DItem)).Select(f => f.Path).ToArray();
            if (selection.Length == 0) return;
            DataObject data = new DataObject(DataFormats.FileDrop, selection);
            this.AllowDrop = false;
            this.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move);
            this.AllowDrop = true;
        }

        void load_local_checksum(KFile item)
        {
            string root = Path.GetDirectoryName(item.Path);
            string file_checksum = Path.Combine(root, $"{item.Name}.MD5");
            if (!System.IO.File.Exists(file_checksum)) return;

            Dictionary<string, string> dic_checksum = new Dictionary<string, string>();
            const string record_pattern_md5 = @"^(\w{32}) \*?(.+)$";
            using (var stream = System.IO.File.OpenText(file_checksum))
            {
                while (!stream.EndOfStream)
                {
                    string line = stream.ReadLine();
                    Match m = Regex.Match(line, record_pattern_md5);
                    if (m.Success)
                        dic_checksum[Path.Combine(root, m.Groups[2].Value)] = m.Groups[1].Value;
                }
            }

            update_checksum(item);

            void update_checksum(KFile folder)
            {
                foreach (KFile child in folder.Children)
                {
                    if (child.IsFolder)
                        update_checksum(child);
                    else if (dic_checksum.TryGetValue(child.Path, out string cs))
                        child.Checksum = cs;
                }
            }
        }
    }
}
