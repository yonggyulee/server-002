namespace Mirero.DAQ.Test.Custom.JhJang;

public class Utils
{
    public static void ToString(object obj)
    {
        var props = obj.GetType().GetProperties();

        for (int i = 0; i < props.Length; i++)
        {
            Console.Write(props[i].GetValue(obj));
            if (i != props.Length - 1)
            {
                Console.WriteLine(", ");
            }
        }
        Console.WriteLine();
    }
}