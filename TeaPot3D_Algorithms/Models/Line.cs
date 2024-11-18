using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TeaPot3D_Algorithms.Models
{
    public class Line
    {
        // X(t) = X(0) + A*t
        // Y(t) = Y(0) + B*t
        // Z(t) = Z(0) + C*t

        public float X0 { get; set; } = 0;
        public float Y0 { get; set;} = 0;
        public float Z0 { get; set;} = 0; 

        public float A { get; set;} = 0;
        public float B { get; set;} = 0;
        public float C { get; set; } = 0;

        public Vector3 LinePoint
        {
            get
            {
                return new Vector3(this.X0, this.Y0, this.Z0);
            }        
        }

        public Vector3 LineDirection
        {
            get
            {
                return new Vector3(this.A, this.B, this.C);
            }              
        }
    }
}
