using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TeaPot3D_Algorithms.Models;
using Plane = TeaPot3D_Algorithms.Models.Plane;

namespace TeaPot3D_Algorithms.Utils
{
    public static class IntersectionComputer
    {
        public static Line ComputeLineFormular(Vertex vertex0, Vertex vertex1)
        {
            Line line = new Line();

            line.X0 = vertex0.X;
            line.Y0 = vertex0.Y;
            line.Z0 = vertex0.Z;

            line.A = vertex1.X - vertex0.X;
            line.B = vertex1.Y - vertex0.Y;
            line.C = vertex1.Z - vertex0.Z;

            return line;
        }

        public static Vertex ComputeIntersectLineAndPlane(Plane pPlane, Line pLine)
        {
            Vector3? intersectionPoint = IntersectLinePlane(pLine.LinePoint, pLine.LineDirection, pPlane.PlanePoint, pPlane.PlaneNormVector);

            if (intersectionPoint.HasValue)
            {
                //Console.WriteLine("Intersection point: " + intersectionPoint.Value);
                return new Vertex()
                {
                    Id = 0,
                    X = (float)intersectionPoint.Value.X,
                    Y = (float)intersectionPoint.Value.Y,
                    Z = (float)intersectionPoint.Value.Z
                };
            }
            else
            {
                throw new InvalidOperationException("The Line must intersect the plane");
                //Console.WriteLine("Line and plane do not intersect.");
            }
        }

        private static Vector3? IntersectLinePlane(Vector3 linePoint, Vector3 lineDirection, Vector3 planePoint, Vector3 planeNormal)
        {
            float denominator = Vector3.Dot(planeNormal, lineDirection);

            // Check if the line and plane are parallel
            if (MathF.Abs(denominator) < float.Epsilon)
                return null;

            float t = Vector3.Dot(planeNormal, planePoint - linePoint) / denominator;
            return linePoint + t * lineDirection;
        }
    }
}

//Vector3 linePoint = new Vector3(1, 2, 3);
//Vector3 lineDirection = new Vector3(2, 1, -1);
//Vector3 planePoint = new Vector3(0, 0, 1);
//Vector3 planeNormal = new Vector3(1, 1, 1);

//Vector3? intersectionPoint = Geometry.IntersectLinePlane(linePoint, lineDirection, planePoint, planeNormal);

//if (intersectionPoint.HasValue)
//{
//    Console.WriteLine("Intersection point: " + intersectionPoint.Value);
//}
//else
//{
//    Console.WriteLine("Line and plane do not intersect.");
//}
