using Xam.Plugin.CrossVersionControl;

namespace Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            var storeVersion = CheckVersion.VerifyAndroid("com.assistofficial.app");
            var b = storeVersion;

            var c = CheckVersion.VerifyIos("com.assistofficial.app");
            var t = c;
        }
    }
}
