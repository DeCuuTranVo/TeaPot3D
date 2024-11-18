using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TeaPot3D_Algorithms.StaticDetails;

namespace TeaPot3D_Algorithms.Models
{
    public class Plane
    {
        // A*X + B*Y + C*Z + D = 0
        public float A { get;} = 0;
        public float B { get;} = 0;
        public float C { get;} = 0;
        public float D { get;} = 0;

        public Plane(float pA, float pB, float pC, float pD) {
            A = pA;
            B = pB;
            C = pC;
            D = pD;
        }

        public Vector3 PlanePoint
        {
            get
            {
                if (this.A == 0)
                {
                    if (this.B  == 0)
                    {
                        if (this.C == 0)
                        {
                            throw new InvalidDataException("A plane can not have a normalization vector of <0,0,0>");
                        }
                        else
                        {
                            return new Vector3((float)0, (float)0, -this.D / this.C);
                        }                   
                    }
                    else
                    {
                        return new Vector3((float)0, -this.D / this.B, (float)0);
                    }                  
                }
                else
                {
                    return new Vector3(-this.D / this.A, (float)0, (float)0);
                }
            }      
        }

        public Vector3 PlaneNormVector
        {
            get
            {
                return new Vector3(this.A, this.B, this.C);
            }          
        }
    }
}
