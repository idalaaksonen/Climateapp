using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatobservations.Models
{
    public class Climate
    {
        public Observer observer { get; set; }
        public Observation observation { get; set; }
        public Category category{ get; set; }
        public decimal value { get; set; }

        public override string ToString()
        {
            return ($"{observer.Firstname} {observer.Lastname} {observation.Date.ToString("yyyy-MM-dd")} {category.Name} {value}");
        }

    }
}
