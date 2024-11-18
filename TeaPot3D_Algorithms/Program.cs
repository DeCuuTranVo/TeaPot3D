
using TeaPot3D_Algorithms.Models;
using TeaPot3D_Algorithms.Utils;

string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\Input\\teapot.obj";
//string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\Input\\cube.obj";

// Folder, where a file is created.
// Make sure to change this folder to your own folder
string folderProcessing = @"D:\Document VT\Quiting Preparation\Intratech\InterviewTest Part 2\TeaPot3D_Algorithms\Samples\Processing\";
string folderOutput = @"D:\Document VT\Quiting Preparation\Intratech\InterviewTest Part 2\TeaPot3D_Algorithms\Samples\Output\";

Object3D obj = ObjectIO.ReadObjectFromFile(objFilePath);

ObjectStatistics objStats = StatisticsComputer.ComputeStatistics(obj);

Plane slicingPlaneOyz = new Plane(1, 0, 0, 0);
//Plane slicingPlaneOyz = new Plane(0, 1, 0, -1);
//Plane slicingPlaneOyz = new Plane(0, 1, 0, -objStats.MidY);
ObjectSplitter objectSplitterOyz = new ObjectSplitter(obj, slicingPlaneOyz);

List<Object3D> object3Ds_Oyz = objectSplitterOyz.Split();

int fileNameCounterProcessing = 1;
foreach (Object3D obj3DItem in object3Ds_Oyz)
{
    ObjectIO.WriteObjectToFile(folderProcessing + $"processing_{++fileNameCounterProcessing}.obj", obj3DItem);
}

List<Object3D> object3Ds_Oyz_Oxz = new List<Object3D>();

Plane slicingPlaneOxz = new Plane(0, 1, 0, (float)-objStats.MidY);
//Plane slicingPlaneOxz = new Plane(0, 1, 0, 0);
foreach (Object3D obj3DItem_Oyz in object3Ds_Oyz)
{
    ObjectSplitter objectSplitterOxz = new ObjectSplitter(obj3DItem_Oyz, slicingPlaneOxz);
    List<Object3D> object3Ds_Oxz = objectSplitterOxz.Split();
    object3Ds_Oyz_Oxz.AddRange(object3Ds_Oxz);
}

int fileNameCounterOutput = 0;
foreach (Object3D obj3DItem in object3Ds_Oyz_Oxz)
{
    ObjectIO.WriteObjectToFile(folderOutput + $"output_{++fileNameCounterOutput}.obj", obj3DItem);
}

