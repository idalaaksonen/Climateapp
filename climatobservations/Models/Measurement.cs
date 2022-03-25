using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatobservations.Models
{
    internal class Measurement
    {
        public int Id { get; set; }
        public decimal Value { get; set; }  // Real kan vara decimal eller float? 
        public int Observation_Id { get; set; }
        public int Categegory_Id { get; set; }
    }
}
