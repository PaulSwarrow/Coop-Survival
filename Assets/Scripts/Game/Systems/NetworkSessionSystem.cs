using Libs.GameFramework;
using Mirror;
using UnityEngine;

namespace DefaultNamespace
{
    public class NetworkSessionSystem : GameSystem
    {
        [Inject] private NetworkManager _networkManager;

        public override void Subscribe()
        {
            Debug.Log(_networkManager);
            Debug.Log("Game: subscribe");
        }

        public override void Start()
        {
            base.Start();
            Debug.Log("Game: start");
        }

        public override void Unsubscribe()
        {
        }
    }
}