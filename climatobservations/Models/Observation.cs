using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatobservations.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Observer_Id { get; set; }
        public int Geolocation_Id { get; set; }
    }
}
