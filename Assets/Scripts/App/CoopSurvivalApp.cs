using DefaultNamespace;
using Libs.GameFramework;
using Mirror;

namespace App
{
    public class CoopSurvivalApp : AppManager
    {
        protected override void RegisterDependencies()
        {
            Register(NetworkManager.singleton);
        }
    }
}