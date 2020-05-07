using Xam.Plugin.CrossVersionControl;

namespace Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            var androidVersion = CheckVersion.VerifyAndroid("com.appbandleid.app");
            var iOSVersion = CheckVersion.VerifyIos("com.appbandleid.app");
        }
    }
}
