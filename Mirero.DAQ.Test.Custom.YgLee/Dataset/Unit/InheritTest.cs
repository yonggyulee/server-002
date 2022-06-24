// using Grpc.Net.Client;
// using Mirero.DAQ.Domain.Test.Service.Protos.V1;
//
// namespace Mirero.DAQ.Test.Custom.YgLee;
//
// public class InheritTest
// {
//     private static TestService.TestServiceClient _client;
//     
//     public static void Test()
//     {
//         var channel = GrpcChannel.ForAddress("http://localhost:5002");
//         _client = new TestService.TestServiceClient(channel);
//
//         _client.Test(new TestRequest
//         {
//             Msg = "Hello"
//         });
//     }
// }

namespace Mirero.DAQ.Test.Custom.YgLee.Dataset.Unit;

public class InheritTest
{
    public static void Test()
    {
        //IBase tmp = new DerivedClass();
        //tmp.Action();

        var tmp = "string";
        var list = tmp.Split("$");
        Console.WriteLine(list.Length);
        Console.WriteLine(list[0]);
    }

    interface IBase
    {
        public void Action();
    }

    class BaseClass : IBase
    {
        public void Action()
        {
            SomeAction();
        }

        protected virtual void SomeAction()
        {
            ActionCore();
        }

        protected virtual void ActionCore()
        {
            Console.WriteLine("BaseClass.ActionCore");
        }
    }

    class DerivedClass : BaseClass
    {
        //public void Action()
        //{
        //    SomeAction();
        //}
        
        protected override void ActionCore()
        {
            Console.WriteLine("DerivedClass.ActionCore");
        }
    }

    abstract class AbstractClass : BaseClass
    {
        public AbstractClass()
        {
            
        }
        
        protected abstract void AbstractAction();
    }
}