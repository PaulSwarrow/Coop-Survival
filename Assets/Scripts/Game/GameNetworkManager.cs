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

        public delegate void ConnectionEventHandler(NetworkConnection connection);

        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
        }

        private bool loaded;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("Client ready" + numPlayers);
            if (numPlayers == 1)
            {
                SessionStartEvent?.Invoke();
                loaded = true;
            }

        }

    }
}