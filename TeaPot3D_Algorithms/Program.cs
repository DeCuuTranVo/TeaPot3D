
using TeaPot3D_Algorithms.Models;
using TeaPot3D_Algorithms.Services;
using TeaPot3D_Algorithms.Utils;

string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\Input\\teapot.obj";
//string objFilePath = "D:\\Document VT\\Quiting Preparation\\Intratech\\InterviewTest Part 2\\TeaPot3D_Algorithms\\Samples\\Input\\cube.obj";

// Folder, where a file is created.
// Make sure to change this folder to your own folder
//string folderProcessing = @"D:\Document VT\Quiting Preparation\Intratech\InterviewTest Part 2\TeaPot3D_Algorithms\Samples\Processing\";
string folderOutput = @"D:\Document VT\Quiting Preparation\Intratech\InterviewTest Part 2\TeaPot3D_Algorithms\Samples\Output\";

AlgorithmRunner.Run(objFilePath, folderOutput);
Console.WriteLine("Algorithm ran successfully!");

