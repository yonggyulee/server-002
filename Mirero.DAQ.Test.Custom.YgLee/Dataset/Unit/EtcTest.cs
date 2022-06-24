using System.Web;

namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;

public class EtcTest
{

    public static void test_1111()
    {
        //var str = "12345";
        //str.Contains(Convert.ToString(1));

        //long id = 1234;

        //Convert.ToString(id).Contains(Convert.ToString(1));

        Console.WriteLine(Path.Combine("dataset", "", ""));
    }

    public static void TestParseQureyString()
    {
        var qurey = "ds_id=4&type=image";

        var parsed = HttpUtility.ParseQueryString(qurey);

        var keys = parsed.AllKeys;
        foreach (var key in keys)
        {
            Console.WriteLine(key);
        }

    }

    public static void TestParseOdataString()
    {
        var qurey = "ds_id=4&type=image";

        // var parsed = ODataUriParser.ParseFilter(qurey,);
    }

    public static void TestPathCombine()
    {
        Console.WriteLine(Environment.CurrentDirectory);
        var di = new DirectoryInfo(Environment.CurrentDirectory);

        var path = Path.Combine(di.Parent.ToString(), "dataset_file_storage");
        Console.WriteLine(path);

        var uri = "\\volume1";
        
        Console.WriteLine(Path.Combine(uri, "img"));

        var dir = Path.GetDirectoryName(Path.Combine(uri, "img"));
        
        Console.WriteLine(dir);

        var uri2 = "/volume1/data";

        dir = Path.GetDirectoryName(uri2);
        
        Console.WriteLine(dir);


        // if(uri.StartsWith("\\")) Console.WriteLine("uri starts \\");
        //
        // Console.WriteLine(Path.Combine(path, "/volume1"));
        //
        // Console.WriteLine(Path.Combine(path, "\\volume1"));
        //
        // Console.WriteLine(Path.Combine(path, "volume1"));
        //
        // Console.WriteLine(Path.Combine(path, "volume1\\dataset1"));
        //
        // Console.WriteLine(Path.Combine(path, "volume1/dataset1"));
        //
        // Console.WriteLine(Path.Combine(path, "./volume1/dataset1"));
        //
        // Console.WriteLine(Path.Combine(path, ".\\volume1\\dataset1"));
        //
        // Console.WriteLine(uri.Replace("\\",""));
        // uri = "/volume1";
        // Console.WriteLine(uri.Replace("/", ""));
    }

    public static void TestEnumerateFiles()
    {
        Console.WriteLine("TestEnumerateFiles...");
        var path = "D:\\workspace\\daq-server\\TestImages";
        var searchPattern = "image1_1.jpg + image1_2.jpg";
        var files = Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories);
        foreach (var file in files)
        {
            Console.WriteLine(file);
        }
    }

    public static void TestDirectory()
    {
        Console.WriteLine("TestDirectory...");
        var path = "D:\\test_directory\\test_directory1\\test_directory2\\test_directory3";

        Directory.CreateDirectory(path);
    }

    public static void TestMoveDir()
    {
        Console.WriteLine("TestMoveDir...");
        var path = "D:\\test_directory";
        var newPath = "D:\\workspace\\test_directory";

        Directory.Move(path, newPath);
    }

    public static void TestCompareDateTime()
    {
        var date = DateTime.ParseExact("20220123153247", "yyyyMMddHHmmss",
            System.Globalization.CultureInfo.InvariantCulture);

        var now = DateTime.Now;
        
        Console.WriteLine(date >= DateTime.Parse("20220123153247"));
    }
    
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }

    public static void TestBook()
    {
        var book1 = new Book
        {
            Title = "MainTitle",
            Author = "Lee"
        };
        
        var book2 = TestChangeBook(book1);
        
        Console.WriteLine(book1.Title);

        book2.Title = "book2";
        
        Console.WriteLine(book1.Title);
        Console.WriteLine(book2.Title);

    }
    
    private static Book TestChangeBook(Book book)
    {
        book.Title = "change_title";
        return book;
    }
}