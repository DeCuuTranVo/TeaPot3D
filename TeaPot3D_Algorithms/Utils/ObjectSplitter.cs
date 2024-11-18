using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TeaPot3D_Algorithms.Models;
using TeaPot3D_Algorithms.Services;
using TeaPot3D_Algorithms.StaticDetails;
using Plane = TeaPot3D_Algorithms.Models.Plane;

namespace TeaPot3D_Algorithms.Utils
{
    public class ObjectSplitter
    {
        // List of vertex above plane
        public Dictionary<int, Vertex> PointAbovePlaneDict = new Dictionary<int, Vertex>();

        // List of vertex below plane
        public Dictionary<int, Vertex> PointBelowPlaneDict = new Dictionary<int, Vertex>();

        // List of vertex on plane
        public Dictionary<int, Vertex> PointOnPlaneDict = new Dictionary<int, Vertex>();

        // List of faces above plane
        public Dictionary<int, Face> FaceAbovePlaneDict = new Dictionary<int, Face>();

        // List of faces below plane
        public Dictionary<int, Face> FaceBelowPlaneDict = new Dictionary<int, Face>();

        // List of faces cross plane
        public Dictionary<int, Face> FaceCrossPlaneDict = new Dictionary<int, Face>();

        // List of faces coincident plane
        public Dictionary<int, Face> FaceCoincidentPlaneDict = new Dictionary<int, Face>();

        public Plane _slicingPlane = null;
        public Object3D _object3D = null;

        public ObjectSplitter(Object3D pObject3D, Plane pSlicingPlane)
        {
            _slicingPlane = pSlicingPlane;
            _object3D = pObject3D;
        }

        // Split a face intercept plane to sub faces
        public List<Object3D> Split()
        {
            foreach (var vertexItem in _object3D.Vertices)
            {
                PointPlanePosition curPointPlanePosition = PlaneHandler.PlanePosition(_slicingPlane, vertexItem.Value);
                if (curPointPlanePosition == PointPlanePosition.Above)
                {
                    PointAbovePlaneDict.Add(vertexItem.Key, vertexItem.Value);
                }
                
                if (curPointPlanePosition == PointPlanePosition.Below)
                {
                    PointBelowPlaneDict.Add(vertexItem.Key, vertexItem.Value);
                }

                if (curPointPlanePosition == PointPlanePosition.OnPlane)
                {
                    PointOnPlaneDict.Add(vertexItem.Key, vertexItem.Value);
                }
            }

            foreach (var faceItem in _object3D.Faces)
            {
                FacePlanePosition curFacePlanePosition = PlaneHandler.PlanePosition(_slicingPlane, faceItem.Value, _object3D.Vertices);
                if (curFacePlanePosition == FacePlanePosition.Above)
                {
                    FaceAbovePlaneDict.Add(faceItem.Key, faceItem.Value);
                }

                if (curFacePlanePosition == FacePlanePosition.Below)
                {
                    FaceBelowPlaneDict.Add(faceItem.Key, faceItem.Value);
                }

                if (curFacePlanePosition == FacePlanePosition.Cross)
                {
                    FaceCrossPlaneDict.Add(faceItem.Key, faceItem.Value);
                }

                if (curFacePlanePosition == FacePlanePosition.Coincident)
                {
                    FaceCoincidentPlaneDict.Add(faceItem.Key, faceItem.Value);
                }
            }

            //foreach (var faceItem in FaceCrossPlaneDict.Values)
            //{
            //    SlicedFaceModel curSlicedFaceModel = TriangleSlicer.Process(_slicingPlane, faceItem, _object3D);
            //}

            Object3D objectAbovePlane = new Object3D()
            {
                Vertices = PointAbovePlaneDict.Concat(PointOnPlaneDict).ToDictionary(x => x.Key, x => x.Value ),
                Faces = FaceAbovePlaneDict.Concat(FaceCoincidentPlaneDict).ToDictionary(x => x.Key, x => x.Value)
            };

            Object3D objectBelowPlane = new Object3D()
            {
                Vertices = PointBelowPlaneDict.Concat(PointOnPlaneDict).ToDictionary(x => x.Key, x => x.Value),
                Faces = FaceBelowPlaneDict.Concat(FaceCoincidentPlaneDict).ToDictionary(x => x.Key, x => x.Value)
            };

            return new List<Object3D>() { objectAbovePlane, objectBelowPlane };
        }

        // Find intersection between a line and and plane

        // Add the intersection face to both splitted objects

        // Split a polygon into triangles
    }
}
