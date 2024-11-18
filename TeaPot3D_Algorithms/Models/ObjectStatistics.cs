using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaPot3D_Algorithms.Models
{
    public class ObjectStatistics
    {
        public float MaxX { get; set; } = float.MinValue;
        public float MaxY { get; set; } = float.MinValue;
        public float MaxZ { get; set; } = float.MinValue;

        public float MinX { get; set; } = float.MaxValue;
        public float MinY { get; set; } = float.MaxValue;
        public float MinZ { get; set; } = float.MaxValue;

        public float MidX { get; set; } = 0;
        public float MidY { get; set; } = 0;
        public float MidZ { get; set; } = 0;


    }
}
