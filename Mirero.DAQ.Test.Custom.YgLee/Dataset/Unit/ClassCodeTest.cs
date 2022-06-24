// using Google.Protobuf;
// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Common.Protos;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class ClassCodeTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     private static readonly string DownloadFolder = DatasetClientTest.DownloadFolder;
//     private static readonly string ImagePath = Path.Combine(DatasetClientTest.TestDataPath, "TestImages");
//
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//
//         // TestCreateClassCode();
//         //TestListClassCodes();
//         //TestGetClassCodeById();
//         //TestUpdateClassCode();
//         TestDeleteClassCode();
//     }
//     
//     private static void TestCreateClassCode()
//     {
//         var imgList = new List<ClassCodeReferenceImage>();
//
//         for (int i = 7; i <= 9; i++)
//         {
//             var filename = "image1_" + i + ".jpg";
//             var uri = Path.Combine(ImagePath, filename);
//         
//             // switch (Path.DirectorySeparatorChar)
//             // {
//             //     case '\\':
//             //         uri = uri.Replace('/', Path.DirectorySeparatorChar);
//             //         break;
//             //     case '/':
//             //         uri = uri.Replace('\\', Path.DirectorySeparatorChar);
//             //         break;
//             // }
//             
//             using var fs = new FileStream(uri, FileMode.Open, FileAccess.Read);
//             var buffer = new byte[fs.Length];
//             fs.Read(buffer, 0, buffer.Length);
//             imgList.Add(new ClassCodeReferenceImage
//             {
//                 ClassCodeSetId = 3,
//                 Filename = "image" + i + ".jpg",
//                 Extension = "jpg",
//                 Buffer = ByteString.CopyFrom(buffer),
//             });
//         }
//         
//         _client.CreateClassCode(new CreateClassCodeRequest
//         {
//             Name = "code3",
//             Code = 3,
//             CreateUser = "이용규",
//             ClassCodeReferenceImages = { imgList },
//             ClassCodeSetId = 3,
//             LockTimeoutSec = 2,
//         });
//     }
//
//     private static void TestListClassCodes(string listFolderName = "list_class_code_test2")
//     {
//         var testPath = Path.Combine(DownloadFolder, listFolderName);
//         if (!Directory.Exists(testPath))
//         {
//             Directory.CreateDirectory(testPath);
//         }
//         
//         var response = _client.ListClassCodes(new ListClassCodesRequest
//         {
//             QueryParameter = new QueryParameter
//             {
//                 PageIndex = 0,
//                 PageSize = 10
//             },
//             WithBuffer = true,
//             LockTimeoutSec = 2,
//         });
//         
//         Console.WriteLine($"PageIndex : {response.PageResult.PageIndex}");
//         Console.WriteLine($"PageSize : {response.PageResult.PageSize}");
//         Console.WriteLine($"Count : {response.PageResult.Count}");
//     
//         var ClassCodes = response.ClassCodes;
//         var list = ClassCodes.AsEnumerable();
//     
//         foreach (var ClassCode in list)
//         {
//             Console.WriteLine("ClassCode:");
//             Console.WriteLine(ClassCode.Id);
//             Console.WriteLine(ClassCode.ClassCodeSetId);
//             Console.WriteLine(ClassCode.ClassCodeReferenceImages.Count);
//             Console.WriteLine("ClassCodeReferenceImages:");
//             foreach (var image in ClassCode.ClassCodeReferenceImages)
//             {
//                 Console.WriteLine(image.Id);
//                 Console.WriteLine(image.Filename);
//                 using var fs = new FileStream(Path.Combine(testPath, image.Filename), FileMode.Create);
//                 fs.Write(image.Buffer.ToByteArray());
//             }
//         }
//     }
//     
//     private static void TestGetClassCodeById(string resultFolderName = "get_class_code_test")
//     {
//         var testPath = Path.Combine(DownloadFolder, resultFolderName);
//
//         Directory.CreateDirectory(testPath);
//         
//         var response = _client.GetClassCode(new GetClassCodeRequest
//         {
//            ClassCodeId = 37,
//            LockTimeoutSec = 2,
//         });
//
//         Console.WriteLine(response.Id);
//         Console.WriteLine(response.ClassCodeSetId);
//         Console.WriteLine(response.ClassCodeReferenceImages.Count);
//
//         foreach (var image in response.ClassCodeReferenceImages)
//         {
//             using var fs = new FileStream(Path.Combine(testPath, image.Filename), FileMode.Create);
//             fs.Write(image.Buffer.ToByteArray());
//         }
//     }
//
//     private static void TestUpdateClassCode(int imgCount = 2, int imgNum = 14)
//     {
//         var files = Directory.EnumerateFiles(ImagePath).ToList();
//         var imgList = new List<ClassCodeReferenceImage>();
//
//         for (int k = imgNum - 2; k < imgNum; k++)
//         {
//             var fileUri = files[(k + 0) % files.Count];
//             using var fs = new FileStream(fileUri, FileMode.Open, FileAccess.Read);
//             var buffer = new byte[fs.Length];
//             fs.Read(buffer, 0, buffer.Length);
//             imgList.Add(new ClassCodeReferenceImage
//             {
//                 Filename = "image" + (k + 42) + "_updated" + ".jpg",
//                 Extension = "jpg",
//                 Buffer = ByteString.CopyFrom(buffer),
//                 ClassCodeSetId = 13
//             });
//         }
//         
//         var response = _client.UpdateClassCode(new UpdateClassCodeRequest
//         {
//             Id = 38,
//             Name = "code2_updated",
//             Code = 2,
//             UpdateUser = "홍길동",
//             ClassCodeSetId = 13,
//             Description = "updated",
//             ClassCodeReferenceImages = { imgList },
//             LockTimeoutSec = 2,
//         });
//
//         Utils.ToString(response);
//     }
//     
//     private static void TestDeleteClassCode()
//     {
//         var response = _client.DeleteClassCode(new DeleteClassCodeRequest
//         {
//             ClassCodeId = 38,
//             LockTimeoutSec = 2,
//         });
//         
//         Utils.ToString(response);
//     }
// }