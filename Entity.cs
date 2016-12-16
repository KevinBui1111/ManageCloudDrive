using KevinHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageCloudDrive
{
    [Serializable()]
    public class DItem : KFile
    {
        public string Id { get; set; }
        public string WebUrl { get; set; }

        public string ParentId { get; set; }

    }
}
