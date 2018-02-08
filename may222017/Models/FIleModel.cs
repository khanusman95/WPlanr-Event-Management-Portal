using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace may222017.Models
{
    [Table("Files")]
    public class FileModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] File { get; set; }
    }
}