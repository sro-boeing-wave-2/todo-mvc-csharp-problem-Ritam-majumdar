using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectX.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public List<CheckList> CheckList { get; set; }
        public List<Label> Label { get; set; }
        public bool Pinned { get; set; }

        public bool IsEquals(Note n)
        {
            if(this.Title == n.Title && this.Message == n.Message && this.Pinned == n.Pinned)
            return true;
            else
                return false;
        }
    }
    public class CheckList
    {
        public int Id { get; set; }
        public string Checklist { get; set; }
    }
    public class Label
    {
        public int Id { get; set; }
        public string label { get; set; }
    }
}
