using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatobservations.Models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } // varchar
        public int? Basecategory_Id { get; set; }
        public int Unit_Id { get; set; }
    }
}
