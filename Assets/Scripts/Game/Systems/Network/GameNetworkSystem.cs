using Libs.GameFramework;
using Mirror;

namespace DefaultNamespace
{
    public abstract class GameNetworkSystem : GameSystem
    {
        [Inject] protected NetworkManager _networkManager;
        public override void Subscribe()
        {
            
        }

        public override void Unsubscribe()
        {
        }
    }
}