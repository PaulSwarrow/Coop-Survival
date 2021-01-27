using System;
using System.Collections.Generic;
using App.Ui;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameNetworkLobbyHud : MonoBehaviour
    {
        [SerializeField] private  Button hostBtn;
        [SerializeField] private  Button joinBtn;

        [SerializeField] private LobbyUiTabs tabs;

        private void Awake()
        {
            hostBtn.onClick.AddListener(StartAsHost);
            joinBtn.onClick.AddListener(StartAsClient);
            tabs.TabChangeEvent += OnTab;
            OnTab();
        }

        private void OnTab()
        {
            Transport.activeTransport = tabs.Current.Transport;
            hostBtn.interactable = joinBtn.interactable = tabs.Current.Available;
            
        }


        public void Disconnect()
        {
            if (Transport.activeTransport.ClientConnected())
            {
                NetworkManager.singleton.StopClient();
            }

            if (Transport.activeTransport.ServerActive())
            {
                NetworkManager.singleton.StopHost();
            }
        }

        public void StartAsHost()
        {
            NetworkManager.singleton.StartHost();
        }


        public void StartAsClient()
        {
            NetworkManager.singleton.networkAddress = tabs.Current.GetSelectedAddress();
            NetworkManager.singleton.StartClient();
        }

    }
}