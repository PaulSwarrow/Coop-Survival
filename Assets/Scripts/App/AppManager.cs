using DefaultNamespace;
using Libs.GameFramework;
using Mirror;

namespace App
{
    public class AppManager : BaseAppManager
    {
        protected override void RegisterDependencies()
        {
            Register(NetworkManager.singleton);
        }
    }
}