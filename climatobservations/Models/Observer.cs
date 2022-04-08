using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace climatobservations.Models
{
    public class Observer
    {
        public int Id { get; set; }
        public string Firstname { get; set; } // Frågetecken säger att antingen så har den ett värde eller inte, för att slippa felmeddelande
        public string Lastname { get; set; }

        public override string ToString()
        {
            return $"{Firstname} {Lastname}";
        }
    }
}
