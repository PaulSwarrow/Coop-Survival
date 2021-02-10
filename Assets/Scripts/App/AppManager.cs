using App.Modules;
using DefaultNamespace;
using Libs.GameFramework;
using Mirror;

namespace App
{
    public class AppManager : BaseAppManager<AppManager>
    {
        [Inject] public ResourcesProvider resources { get; private set; }
        protected override void RegisterDependencies()
        {
            Register(GetComponent<ResourcesProvider>());
            Register(FindObjectOfType<GameNetworkManager>());
            Register(this);
        }
    }
}