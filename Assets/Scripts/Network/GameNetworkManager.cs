using System;
using Libs.GameFramework;
using Libs.GameFramework.Interfaces;
using Mirror;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameNetworkManager : NetworkManager
    {
        public delegate void ConnectionEventDelegate(NetworkConnection connection);

        public event Action SessionStart;
        public event ConnectionEventDelegate PlayerAddedEvent;


        public override void OnServerConnect(NetworkConnection conn)
        {
            Debug.Log("Server: new client");
            base.OnServerConnect(conn);
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            Debug.Log("Server: client is ready");
            base.OnServerReady(conn);
        }

        public override void OnServerChangeScene(string newSceneName)
        {
            Debug.Log("Server: scene request");
            base.OnServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            Debug.Log("Server: scene loaded");
            base.OnServerSceneChanged(sceneName);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            Debug.Log("Client: connected");
            base.OnClientConnect(conn);
        }

        public override void OnStartHost()
        {
            Debug.Log("OnStartHost");
            base.OnStartHost();
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Debug.Log("Server: player added");
            base.OnServerAddPlayer(conn);
            if (numPlayers == 1) SessionStart?.Invoke();
            PlayerAddedEvent?.Invoke(conn);
        }
    }
}