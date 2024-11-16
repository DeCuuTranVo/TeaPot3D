using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaPot3D_Algorithms.StaticDetails;

namespace TeaPot3D_Algorithms.Models
{
    public class Plane
    {
        // A*X + B*Y + C*Z + D = 0
        public decimal A { get;} = 0;
        public decimal B { get;} = 0;
        public decimal C { get;} = 0;
        public decimal D { get;} = 0;

        public Plane(decimal pA, decimal pB, decimal pC, decimal pD) {
            A = pA;
            B = pB;
            C = pC;
            D = pD;
        }
       
    }
}
