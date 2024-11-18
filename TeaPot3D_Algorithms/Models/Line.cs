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

        public decimal X0 { get; set; } = 0;
        public decimal Y0 { get; set;} = 0;
        public decimal Z0 { get; set;} = 0; 

        public decimal A { get; set;} = 0;
        public decimal B { get; set;} = 0;
        public decimal C { get; set; } = 0;

        public Vector3 LinePoint
        {
            get
            {
                return new Vector3((float)this.X0, (float)this.Y0, (float)this.Z0);
            }        
        }

        public Vector3 LineDirection
        {
            get
            {
                return new Vector3((float)this.A, (float)this.B, (float)this.C);
            }              
        }
    }
}
