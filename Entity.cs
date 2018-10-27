using BrightIdeasSoftware;
using KevinHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManageCloudDrive
{
    [Serializable()]
    public class DItem : KFile
    {
        public string Id { get; set; }
        public string WebUrl { get; set; }

        public string ParentId { get; set; }

    }
    [Serializable()]
    public class DriveInfo
    {
        public string userinfo { get; set; }
        public string storage_amount { get; set; }
        public KFile rootCloud { get; set; }

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
            else if (xIsNull || yIsNull)
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
