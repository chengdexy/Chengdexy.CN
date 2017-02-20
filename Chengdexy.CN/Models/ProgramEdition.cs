using System;

namespace Chengdexy.CN.Models
{
    public class ProgramEdition
    {
        public int ID { get; set; }
        public int ProgramID { get; set; }
        public DateTime PublishDate { get; set; }
        public string EditionString { get; set; }
        public string DownloadUrl { get; set; }
        public virtual Program Program { get; set; }
    }
}