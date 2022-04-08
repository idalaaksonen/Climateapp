using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatobservations.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public float Value { get; set; }  // Real kan vara decimal eller float? 
        public int Observation_Id { get; set; }
        public int Category_Id { get; set; }
    }
}
