using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaPot3D_Algorithms.Models;
using TeaPot3D_Algorithms.StaticDetails;

namespace TeaPot3D_Algorithms.Services
{
    public static class PlaneHandler
    {
        public static PointPlanePosition PlanePosition(Plane pPlane, Vertex pVertex)
        {
            float positionValue = pPlane.A * pVertex.X + pPlane.B * pVertex.Y + pPlane.C * pVertex.Z + pPlane.D;
            
            if (Math.Abs(positionValue) <= float.Epsilon)
            {
                return PointPlanePosition.OnPlane;
            }

            if (positionValue > 0)
            {
                return PointPlanePosition.Above;
            }
            
            if (positionValue < 0)
            {
                return PointPlanePosition.Below;
            }

            throw new InvalidOperationException("This state is invalid.");      
        }

        public static FacePlanePosition PlanePosition(Plane pPlane, Face face, Dictionary<int, Vertex> pVertexDict)
        {
            int countAbovePoint = 0;
            int countBelowPoint = 0;
            int countOnPlanePoint = 0;

            foreach (int vertexId in face.Vertices)
            {
                PointPlanePosition facePlanePosition = PlanePosition(pPlane, pVertexDict[vertexId]);
                
                if (facePlanePosition == PointPlanePosition.Above)
                {
                    countAbovePoint++;
                }
                else if (facePlanePosition == PointPlanePosition.Below)
                {
                    countBelowPoint++;
                }
                else
                {
                    countOnPlanePoint++;
                }
            }

            if (countAbovePoint > 0 && countBelowPoint == 0) {
                return FacePlanePosition.Above;
            }
            else if (countAbovePoint == 0 && countBelowPoint > 0)
            {
                return FacePlanePosition.Below;
            }
            else if (countAbovePoint > 0 && countBelowPoint > 0)
            {
                return FacePlanePosition.Cross;
            }
            else
            {
                return FacePlanePosition.Coincident;
            }

        }
    }
}
