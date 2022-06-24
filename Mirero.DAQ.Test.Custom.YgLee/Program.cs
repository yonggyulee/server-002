// See https://aka.ms/new-console-template for more information

using Mirero.DAQ.Test.Custom.YgLee;
using Mirero.DAQ.Test.Custom.YgLee.Dataset;
using Mirero.DAQ.Test.Custom.YgLee.Inference;
//using Mirero.DAQ.Test.Custom.YgLee.MachineLearning;

Console.WriteLine("Hello, World!");

#region Dataset

//DatasetClientTest.Test();

#endregion

#region Inference

InferenceClientTest.Test();

#endregion

//#region MachineLearning

////MachineLearningClientTest.Test();

//#endregion
