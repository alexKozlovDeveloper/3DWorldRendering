using Assets.Scripts.Storing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Storing.Models.Layers
{
    public class Altitude : ILayer
    {
        public double Value { get; set; }

        public Altitude() { }

        public Altitude(double value)
        {
            Value = value;
        }
    }
}
