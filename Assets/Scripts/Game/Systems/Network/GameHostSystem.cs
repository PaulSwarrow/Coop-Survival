using Libs.GameFramework;
using Mirror;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameHostSystem : GameNetworkSystem
    {

        public override void Subscribe()
        {
            Debug.Log(_networkManager);
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