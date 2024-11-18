using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaPot3D_Algorithms.Models;

namespace TeaPot3D_Algorithms.Utils
{
    public static class ObjectIO
    {
        public static Object3D ReadObjectFromFile(string pObjFileReadPath)
        {
            // Read a text file line by line.
            string[] lines = File.ReadAllLines(pObjFileReadPath);

            int vertexCount = 1;
            int faceCount = 1;

            Dictionary<int, Vertex> vertices = new Dictionary<int, Vertex>();
            Dictionary<int, Face> faces = new Dictionary<int, Face>();
            Object3D obj = new Object3D()
            {
                Vertices = vertices,
                Faces = faces
            };

            foreach (string line in lines)
            {
                string[] words = line.Trim().Split(' ');
                string entryType = words[0];
                if (entryType == "v")
                {
                    Vertex curVertex = new Vertex()
                    {
                        Id = vertexCount,
                        X = float.Parse(words[1]),
                        Y = float.Parse(words[2]),
                        Z = float.Parse(words[3]),
                    };

                    vertices.Add(vertexCount, curVertex);
                    vertexCount++;
                }

                if (entryType == "f")
                {
                    List<int> curVertices = new List<int>();
                    for (int i = 1; i < words.Length; i++)
                    {
                        curVertices.Add(int.Parse(words[i]));
                    }

                    Face curFace = new Face()
                    {
                        Id = faceCount,
                        Vertices = curVertices
                    };
                    faces.Add(faceCount, curFace);
                    faceCount++;
                }
            }

            //Console.WriteLine("#Vertices: " + obj.Vertices.Count);
            //Console.WriteLine("#Faces: " + obj.Faces.Count);
            return obj;

        }

        public static void WriteObjectToFile(string pObjFileWritePath, Object3D objectToWrite)
        {
            List<string> lineToWrites = new List<string>();
            Dictionary<int, int> IdConvertDict_AboveObject = new Dictionary<int, int>();

            int IdVertexCount = 1;
            foreach (var item in objectToWrite.Vertices.Values)
            {
                string lineToWrite = $"v {item.X} {item.Y} {item.Z}";
                lineToWrites.Add(lineToWrite);

                IdConvertDict_AboveObject.Add(item.Id, IdVertexCount);
                IdVertexCount++;
            }

            lineToWrites.Add(string.Empty);

            foreach (var item in objectToWrite.Faces.Values)
            {
                string lineToWrite = $"f";
                foreach (var vertexId in item.Vertices)
                {
                    lineToWrite += $" {IdConvertDict_AboveObject[vertexId]}";
                }

                lineToWrites.Add(lineToWrite);
            }

            // An array of strings
            // Write array of strings to a file using WriteAllLines.
            // If the file does not exists, it will create a new file.
            // This method automatically opens the file, writes to it, and closes file
            File.WriteAllLines(pObjFileWritePath, lineToWrites.ToArray());
        }
    }
}
