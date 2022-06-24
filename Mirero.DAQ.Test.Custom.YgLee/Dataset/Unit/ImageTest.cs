// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Dataset.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;
//
// public class ImageTest
// {
//     private static DatasetService.DatasetServiceClient _client;
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new DatasetService.DatasetServiceClient(channel);
//         
//         // TestCreateImage();
//         // TestListImages();
//         // TestGetImageById();
//         // TestUpdateImage();
//         // TestDeleteImage();
//     }
//
//     // private static void TestCreateImage()
//     // {
//     //     byte[] buffer;
//     //     const string imagePath = "D:/workspace/daq-server/TestImages";
//     //     const string filename = "image1_1.jpg";
//     //     var uri = Path.Combine(imagePath, filename);
//     //
//     //     using (var fs = new FileStream(uri, FileMode.Open, FileAccess.Read))
//     //     {
//     //         buffer = new byte[fs.Length];
//     //         fs.Read(buffer, 0, buffer.Length);
//     //     }
//     //
//     //     _client.CreateImage(new Image
//     //     {
//     //         ClassCodeId = 1,
//     //         DatasetId = 1,
//     //         Filename = "image1.jpg",
//     //         Extension = "jpg",
//     //         ImageCode = "right",
//     //         Buffer = ByteString.CopyFrom(buffer),
//     //         // Properties = "test_properties"
//     //     });
//     // }
//     //
//     // private static void TestListImages()
//     // {
//     //     var response = _client.ListImages(new ListImagesRequest
//     //     {
//     //         PageIndex = 1,
//     //         PageSize = 10
//     //     });
//     //     
//     //     Console.WriteLine($"PageIndex : {response.PageIndex}");
//     //     Console.WriteLine($"PageSize : {response.PageSize}");
//     //     Console.WriteLine($"Count : {response.Count}");
//     //
//     //     var images = response.Images;
//     //     var list = images.AsEnumerable();
//     //
//     //     foreach (var image in list)
//     //     {
//     //         Utils.ToString(image);
//     //     }
//     // }
//     //
//     // private static void TestGetImageById()
//     // {
//     //     var response = _client.GetImage(new GetImageRequest
//     //     {
//     //        ImageId = 25
//     //     });
//     //
//     //     Utils.ToString(response);
//     // }
//     //
//     // private static void TestUpdateImage()
//     // {
//     //     var response = _client.UpdateImage(new Image
//     //     {
//     //         // DatasetName = "dataset1",
//     //         ClassCodeId = 1,
//     //         Filename = "image1",
//     //         Extension = "image1...",
//     //         ImageCode = "right",
//     //         Properties = "test_properties"
//     //     });
//     //
//     //     Utils.ToString(response);
//     // }
//     //
//     // private static void TestDeleteImage()
//     // {
//     //     var response = _client.DeleteImage(new DeleteImageRequest
//     //     {
//     //         // Filename = "image3"
//     //     });
//     //     
//     //     Console.WriteLine(response.GetType());
//     // }
// }