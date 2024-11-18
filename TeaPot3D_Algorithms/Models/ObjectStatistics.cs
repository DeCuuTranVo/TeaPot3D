using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaPot3D_Algorithms.Models
{
    public class ObjectStatistics
    {
        public decimal MaxX { get; set; } = decimal.MinValue;
        public decimal MaxY { get; set; } = decimal.MinValue;
        public decimal MaxZ { get; set; } = decimal.MinValue;

        public decimal MinX { get; set; } = decimal.MaxValue;
        public decimal MinY { get; set; } = decimal.MaxValue;
        public decimal MinZ { get; set; } = decimal.MaxValue;

        public decimal MidX { get; set; } = 0;
        public decimal MidY { get; set; } = 0;
        public decimal MidZ { get; set; } = 0;


    }
}
