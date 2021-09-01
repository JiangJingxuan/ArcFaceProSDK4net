using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ArcFaceProSDK4net.FaceEngine.GetActiveDeviceInfo());
            Console.WriteLine("复制上面的激活码,按任意键退出");
            Console.ReadKey();
        }
    }
}
