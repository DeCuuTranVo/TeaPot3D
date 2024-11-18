using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaPot3D_Algorithms.Models;

namespace TeaPot3D_Algorithms.Utils
{
    public static class StatisticsComputer
    {
        public static ObjectStatistics ComputeStatistics(Object3D pObject3D)
        {
            ObjectStatistics result = new ObjectStatistics();
            foreach (Vertex vertexItem in pObject3D.Vertices.Values)
            {
                result.MaxX = Math.Max(result.MaxX, vertexItem.X);
                result.MinX = Math.Min(result.MinX, vertexItem.X);
                result.MaxY = Math.Max(result.MaxY, vertexItem.Y);
                result.MinY = Math.Min(result.MinY, vertexItem.Y);
                result.MaxZ = Math.Max(result.MaxZ, vertexItem.Z);
                result.MinZ = Math.Min(result.MinZ, vertexItem.Z);
            }

            result.MidX = (result.MaxX + result.MinX) / 2;
            result.MidY = (result.MaxY + result.MinY) / 2;
            result.MidZ = (result.MaxZ + result.MinZ) / 2;

            return result;
        } 
    }
}
