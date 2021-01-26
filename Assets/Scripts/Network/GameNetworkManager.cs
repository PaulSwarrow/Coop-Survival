using System;
using Libs.GameFramework;
using Libs.GameFramework.Interfaces;
using Mirror;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameNetworkManager : NetworkManager
    {
        public event Action SessionStartEvent;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Client ready" + numPlayers);
            if (numPlayers == 1)
            {
                SessionStartEvent?.Invoke();
            }

        }
        
        
        

    }
}