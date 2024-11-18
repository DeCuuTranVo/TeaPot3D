using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TeaPot3D_Algorithms.Models;
using TeaPot3D_Algorithms.Services;
using TeaPot3D_Algorithms.StaticDetails;
using Plane = TeaPot3D_Algorithms.Models.Plane;

namespace TeaPot3D_Algorithms.Utils
{
    public static class TriangleSlicer
    {
        // Input a triangle to be slice

        // Process: 2 cases
        // Case 1: (2 above && 1 below) || (1 above && 2 below)
        // Case 2: 1 above, 1 on plane, 1 below

        // Output: Return 3 new triangles (case 1) or 2 new triangles (case 2)
        public static SlicedFaceVertexModel Process(Plane pPlane, Face pFace, Object3D pObj)
        {
            Dictionary<PointPlanePosition, List<Vertex>> faceVerticesDict = new Dictionary<PointPlanePosition, List<Vertex>>();

            faceVerticesDict.Add(PointPlanePosition.Above, new List<Vertex>());
            faceVerticesDict.Add(PointPlanePosition.OnPlane, new List<Vertex>());
            faceVerticesDict.Add(PointPlanePosition.Below, new List<Vertex>());

            foreach (int vertexIdx in pFace.Vertices)
            {
                Vertex vertexItem = pObj.Vertices[vertexIdx];
                PointPlanePosition curPointPlanePosition = PlaneHandler.PlanePosition(pPlane, vertexItem);
                faceVerticesDict[curPointPlanePosition].Add(vertexItem);
            }

            int countAbove = faceVerticesDict[PointPlanePosition.Above].Count();
            int countOnPlane = faceVerticesDict[PointPlanePosition.OnPlane].Count();
            int countBelow = faceVerticesDict[PointPlanePosition.Below].Count();

            if (countAbove > 0 && countBelow > 0 && countOnPlane == 0)
            {
                return SliceCaseOne(pPlane, faceVerticesDict, pObj);
            }
            else if (countBelow > 0 && countBelow > 0 && countOnPlane > 0)
            {
                return SliceCaseTwo(pPlane, faceVerticesDict, pObj);
            }
            else
            {
                throw new InvalidOperationException("There must be at least a vertex above plane and a vertex below plane");
            }

        }

        //Create new faces between those vertices
        //Separate those faces between above and below plane then add new faces to SlicedFaceModel
        //Return
        // Case 1: (2 above && 1 below) || (1 above && 2 below)
        public static SlicedFaceVertexModel SliceCaseOne(Plane pPlane, Dictionary<PointPlanePosition, List<Vertex>> pFaceVerticesDict, Object3D pObj)
        {
            SlicedFaceVertexModel slicedFaceModel = new SlicedFaceVertexModel();
            List<PointsOnLineSet> pointsOnLineSetList = new List<PointsOnLineSet>();

            //For each pair of vertices above and below plane, compute 1 intersection vertex
            foreach (Vertex vertexAbove in pFaceVerticesDict[PointPlanePosition.Above])
            {
                foreach (Vertex vertexBelow in pFaceVerticesDict[PointPlanePosition.Below])
                {
                    Line lineCross = IntersectionComputer.ComputeLineFormular(vertexBelow, vertexAbove);
                    Vertex vertexIntersect = IntersectionComputer.ComputeIntersectLineAndPlane(pPlane, lineCross);

                    // Check if vertex really on plane

                    //
                    //vertexIntersect.Id = pObj.Vertices.Count + 1;
                    vertexIntersect.Id = pObj.Vertices.MaxBy(x => x.Key).Key + 1;

                    pFaceVerticesDict[PointPlanePosition.OnPlane].Add(vertexIntersect);

                    PointsOnLineSet set = new PointsOnLineSet();

                    set.PointAbovePlane = vertexAbove;
                    set.PointOnPlane = vertexIntersect;
                    set.PointBelowPlane = vertexBelow;

                    pointsOnLineSetList.Add(set);

                    pObj.Vertices.Add(vertexIntersect.Id, vertexIntersect);
                    slicedFaceModel.VerticesOnPlane.Add(set.PointOnPlane);
                }
            }

            if (pointsOnLineSetList[0].PointBelowPlane == pointsOnLineSetList[1].PointBelowPlane)
            {

                Face faceAbovePlane1 = new Face()
                {
                    //Id = pObj.Faces.Count + 1
                    Id = pObj.Faces.MaxBy(x => x.Key).Key + 1
                };
                faceAbovePlane1.Vertices = new List<int>
                {
                    pointsOnLineSetList[0].PointAbovePlane.Id,
                    pointsOnLineSetList[0].PointOnPlane.Id,
                    pointsOnLineSetList[1].PointOnPlane.Id
                };

                Face faceAbovePlane2 = new Face()
                {
                    //Id = pObj.Faces.Count + 2
                    Id = pObj.Faces.MaxBy(x => x.Key).Key + 2
                };
                faceAbovePlane2.Vertices = new List<int>
                {
                    pointsOnLineSetList[0].PointAbovePlane.Id,
                    pointsOnLineSetList[1].PointAbovePlane.Id,
                    pointsOnLineSetList[1].PointOnPlane.Id
                };

                Face faceBelowPlane = new Face()
                { 
                    //Id = pObj.Faces.Count + 3
                    Id = pObj.Faces.MaxBy(x => x.Key).Key + 3
                };
                faceBelowPlane.Vertices = new List<int>
                {
                    pointsOnLineSetList[0].PointBelowPlane.Id,
                    pointsOnLineSetList[0].PointOnPlane.Id,
                    pointsOnLineSetList[1].PointOnPlane.Id
                };

                slicedFaceModel.FacesAbovePLane.Add(faceAbovePlane1);
                slicedFaceModel.FacesAbovePLane.Add(faceAbovePlane2);
                slicedFaceModel.FacesBelowPlane.Add(faceBelowPlane);

                pObj.Faces.Add(faceAbovePlane1.Id, faceAbovePlane1);
                pObj.Faces.Add(faceAbovePlane2.Id, faceAbovePlane2);
                pObj.Faces.Add(faceBelowPlane.Id, faceBelowPlane);
            }

            if (pointsOnLineSetList[0].PointAbovePlane == pointsOnLineSetList[1].PointAbovePlane)
            {
                Face faceBelowPlane1 = new Face()
                {
                    //Id = pObj.Faces.Count + 1
                    Id = pObj.Faces.MaxBy(x => x.Key).Key + 1
                };
                faceBelowPlane1.Vertices = new List<int>
                {
                    pointsOnLineSetList[0].PointBelowPlane.Id,
                    pointsOnLineSetList[0].PointOnPlane.Id,
                    pointsOnLineSetList[1].PointOnPlane.Id
                };

                Face faceBelowPlane2 = new Face()
                {
                    //Id = pObj.Faces.Count + 2
                    Id = pObj.Faces.MaxBy(x => x.Key).Key + 2
                };
                faceBelowPlane2.Vertices = new List<int>
                {
                    pointsOnLineSetList[0].PointBelowPlane.Id,
                    pointsOnLineSetList[1].PointBelowPlane.Id,
                    pointsOnLineSetList[1].PointOnPlane.Id
                };

                Face faceAbovePlane = new Face()
                {
                    //Id = pObj.Faces.Count + 3
                    Id = pObj.Faces.MaxBy(x => x.Key).Key + 3
                };
                faceAbovePlane.Vertices = new List<int>
                {
                    pointsOnLineSetList[0].PointAbovePlane.Id,
                    pointsOnLineSetList[0].PointOnPlane.Id,
                    pointsOnLineSetList[1].PointOnPlane.Id
                };

                slicedFaceModel.FacesAbovePLane.Add(faceAbovePlane);
                slicedFaceModel.FacesBelowPlane.Add(faceBelowPlane1);
                slicedFaceModel.FacesBelowPlane.Add(faceBelowPlane2);

                pObj.Faces.Add(faceAbovePlane.Id, faceAbovePlane);
                pObj.Faces.Add(faceBelowPlane1.Id, faceBelowPlane1);
                pObj.Faces.Add(faceBelowPlane2.Id, faceBelowPlane2);
            }

            return slicedFaceModel;
        }


        // Case: One point above plane, one point below plane, one point on plane
        public static SlicedFaceVertexModel SliceCaseTwo(Plane pPlane, Dictionary<PointPlanePosition, List<Vertex>> pFaceVerticesDict, Object3D pObj)
        {
            SlicedFaceVertexModel slicedFaceModel = new SlicedFaceVertexModel();
            List<PointsOnLineSet> pointsOnLineSetList = new List<PointsOnLineSet>();

            Vertex pointOnPlaneInitial = pFaceVerticesDict[PointPlanePosition.OnPlane][0];

            //For each pair of vertices above and below plane, compute 2 intersection vertex
            foreach (Vertex vertexAbove in pFaceVerticesDict[PointPlanePosition.Above])
            {
                foreach (Vertex vertexBelow in pFaceVerticesDict[PointPlanePosition.Below])
                {
                    Line lineCross = IntersectionComputer.ComputeLineFormular(vertexBelow, vertexAbove);
                    Vertex vertexIntersect = IntersectionComputer.ComputeIntersectLineAndPlane(pPlane, lineCross);

                    // Check if vertex really on plane

                    //
                    //vertexIntersect.Id = pObj.Vertices.Count + 1;
                    vertexIntersect.Id = pObj.Vertices.MaxBy(x => x.Key).Key + 1;

                    pFaceVerticesDict[PointPlanePosition.OnPlane].Add(vertexIntersect);

                    PointsOnLineSet set = new PointsOnLineSet();

                    set.PointAbovePlane = vertexAbove;
                    set.PointOnPlane = vertexIntersect;
                    set.PointBelowPlane = vertexBelow;

                    pointsOnLineSetList.Add(set);

                    pObj.Vertices.Add(vertexIntersect.Id, vertexIntersect);
                    slicedFaceModel.VerticesOnPlane.Add(set.PointOnPlane);
                }
            }

            Face faceAbovePlane = new Face()
            {
                //Id = pObj.Faces.Count + 1
                Id = pObj.Faces.MaxBy(x => x.Key).Key + 1
            };
            faceAbovePlane.Vertices = new List<int>
            {
                pointsOnLineSetList[0].PointAbovePlane.Id,
                pointsOnLineSetList[0].PointOnPlane.Id,
                pointOnPlaneInitial.Id
            };

            Face faceBelowPlane = new Face()
            {
                //Id = pObj.Faces.Count + 2
                Id = pObj.Faces.MaxBy(x => x.Key).Key + 2
            };
            faceBelowPlane.Vertices = new List<int>
            {
                pointsOnLineSetList[0].PointBelowPlane.Id,
                pointsOnLineSetList[0].PointOnPlane.Id,
                pointOnPlaneInitial.Id
            };

            slicedFaceModel.FacesAbovePLane.Add(faceAbovePlane);
            slicedFaceModel.FacesBelowPlane.Add(faceBelowPlane);

            pObj.Faces.Add(faceAbovePlane.Id, faceAbovePlane);
            pObj.Faces.Add(faceBelowPlane.Id, faceAbovePlane);
            

            return slicedFaceModel;
        }
    }
}
