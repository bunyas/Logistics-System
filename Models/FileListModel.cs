using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mascis.Models
{
    public class FileListModel
    {
        public List<FileList> FileListCollction { get; set; }
        public FileList FileListDetail { get; set; }
    }
    public class FileList
    {
        public string Id { get; set; }
        public string FileURL { get; set; }
        public string FileType { get; set; }
        public string Detail { get; set; }
        public int? e_reg_complaint_code { get; set; }
        //public FileStream fileStream { get; set; }
        // public FileContentResult fileStream { get; set; }
        //public int MyProperty { get; set; }FileContentResult
        private string Sax;

        //public string MyProperty
        //{
        //    get { return Sax; }
        //    set { Sax = value; }
        //}

    }
    //public class FileListModel
    //{
    //}
}