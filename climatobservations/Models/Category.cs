using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatobservations.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public int? Basecategory_Id { get; set; }
        public int Unit_Id { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
