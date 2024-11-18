
using TeaPot3D_Algorithms.Models;
using TeaPot3D_Algorithms.Utils;

string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\teapot.obj";
//string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\cube.obj";

Object3D obj = ObjectIO.ReadObjectFromFile(objFilePath);

ObjectStatistics objStats = StatisticsComputer.ComputeStatistics(obj);

Plane slicingPlaneOyz = new Plane(1, 0, 0, 0);
ObjectSplitter objectSplitterOyz = new ObjectSplitter(obj, slicingPlaneOyz);

List<Object3D> object3Ds_Oyz = objectSplitterOyz.Split();
List<Object3D> object3Ds_Oyz_Oxz = new List<Object3D>();

Plane slicingPlaneOxz = new Plane(0, 1, 0, (decimal)-objStats.MidY);
//Plane slicingPlaneOxz = new Plane(0, 1, 0, 0);
foreach (Object3D obj3DItem_Oyz in object3Ds_Oyz)
{
    ObjectSplitter objectSplitterOxz = new ObjectSplitter(obj3DItem_Oyz, slicingPlaneOxz);
    List<Object3D> object3Ds_Oxz = objectSplitterOxz.Split();
    object3Ds_Oyz_Oxz.AddRange(object3Ds_Oxz);
}

// Folder, where a file is created.
// Make sure to change this folder to your own folder
string folder = @"D:\Document VT\Quiting Preparation\Intratech\InterviewTest Part 2\TeaPot3D_Algorithms\Samples\";

int fileNameCounter = 0;
foreach (Object3D obj3DItem in object3Ds_Oyz_Oxz)
{
    ObjectIO.WriteObjectToFile(folder + $"{++fileNameCounter}.obj", obj3DItem);
}

//Vertex vertex0 = new Vertex() {
//    Id = 0,
//    X = -1,
//    Y = 0,
//    Z = 0
//};

//Vertex vertex1 = new Vertex()
//{
//    Id = 1,
//    X = 1,
//    Y = 0,
//    Z = 0
//};

//Line newLine = IntersectionComputer.ComputeLineFormular(vertex0, vertex1);

//Vertex vertex2 = IntersectionComputer.ComputeIntersectLineAndPlane(slicingPlaneOyz, newLine);

//Console.WriteLine("Intersection: ( " + vertex2.X + ", " + vertex2.Y + ", " + vertex2.Z + ")");
