using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.Model;

namespace ApplicationDb.Cor.EntityModels
{
    public class FileCabinets
    {
        public Guid Id { get; set; }
        public string FilName { get; set; }
        public FileFormatType FormatType { get; set; }
        public byte[] Data { get; set; }
        public long Size { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
