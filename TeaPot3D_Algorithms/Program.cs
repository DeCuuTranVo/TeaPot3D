
using TeaPot3D_Algorithms.Models;
using TeaPot3D_Algorithms.Utils;

string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\teapot.obj";
//string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\cube.obj";

// Read a text file line by line.
string[] lines = File.ReadAllLines(objFilePath);

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
            X = decimal.Parse(words[1]),
            Y = decimal.Parse(words[2]),
            Z = decimal.Parse(words[3]),
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

Console.WriteLine("#Vertices: " + obj.Vertices.Count);
Console.WriteLine("#Faces: " + obj.Faces.Count);

Plane slicingPlane = new Plane(1, 0, 0, 0);
ObjectSplitter objectSplitter = new ObjectSplitter(obj, slicingPlane);

List<Object3D> object3Ds = objectSplitter.Split();
Object3D aboveObject = object3Ds[0];

// Folder, where a file is created.
// Make sure to change this folder to your own folder
string folder = @"D:\Document VT\Quiting Preparation\Intratech\InterviewTest Part 2\TeaPot3D_Algorithms\Samples\";
// Filename
string fileName = "AboveObject.obj";
// Fullpath. You can direct hardcode it if you like.
string fullPath = folder + fileName;

//
List<string> lineToWrites = new List<string>();
Dictionary<int, int> IdConvertDict_AboveObject = new Dictionary<int, int>();

int IdVertexCount = 1;
foreach (var item in aboveObject.Vertices.Values)
{
    string lineToWrite = $"v {item.X} {item.Y} {item.Z}";
    lineToWrites.Add(lineToWrite);

    IdConvertDict_AboveObject.Add(item.Id, IdVertexCount);
    IdVertexCount++;
}

lineToWrites.Add(string.Empty);

foreach (var item in aboveObject.Faces.Values)
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
File.WriteAllLines(fullPath, lineToWrites.ToArray());
