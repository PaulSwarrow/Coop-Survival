using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameNetworkLobbyHud : MonoBehaviour
    {
        public Text labelUser;
        public Dropdown dropdownSteamFriends;
        public Button hostBtn;
        public Button joinBtn;

        List<CSteamID> friendSteamIDs = new List<CSteamID>();


        // Use this for initialization
        void Start()
        {
            if (SteamManager.Initialized)
            {
                //get My steam name
                if (labelUser)
                {
                    labelUser.text = "Current Steam User: " + SteamFriends.GetPersonaName();
                }

                dropdownSteamFriends.ClearOptions();
                int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

                for (int i = 0; i < friendCount; ++i)
                {
                    CSteamID friendSteamId = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
                    string friendName = SteamFriends.GetFriendPersonaName(friendSteamId);
                    EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamId);

                    Dropdown.OptionData option = new Dropdown.OptionData();
                    option.text = friendName;
                    if (friendState != EPersonaState.k_EPersonaStateOffline)
                    {
                        dropdownSteamFriends.options.Add(option);
                        friendSteamIDs.Add(friendSteamId);
                    }
                }

                dropdownSteamFriends.value = 1;

                hostBtn.onClick.AddListener(StartAsHost);
                joinBtn.onClick.AddListener(StartAsClient);
            }
            else
            {
                Debug.LogError("Steam network initialization error");
            }
        }

        // Update is called once per frame
        void Update()
        {
        }


        public void Disconnect()
        {
            if (Transport.activeTransport.ClientConnected())
            {
                GameNetworkManager.singleton.StopClient();
            }

            if (Transport.activeTransport.ServerActive())
            {
                GameNetworkManager.singleton.StopHost();
            }
        }

        public void StartAsHost()
        {
            // Transport.activeTransport = SteamNetworkManager.steam;
            GameNetworkManager.singleton.StartHost();
        }

        /*public void StartAsHostTelepathy()
        {
            // Transport.activeTransport = SteamNetworkManager.telepathy;
            GameNetworkManager.singleton.StartHost();
        }*/

        public void StartAsClient()
        {
            // Transport.activeTransport = SteamNetworkManager.steam;
            Debug.Log("connect to friend index " + dropdownSteamFriends.value);
            Debug.Log("connect to friend steam ID " + friendSteamIDs[dropdownSteamFriends.value].ToString());
            GameNetworkManager.singleton.networkAddress = friendSteamIDs[dropdownSteamFriends.value].ToString();
            GameNetworkManager.singleton.StartClient();
        }

        /*public void StartAsClientTelepathy()
        {
            Transport.activeTransport = SteamNetworkManager.telepathy;
            GameNetworkManager.singleton.networkAddress = ipAddress.text;
            GameNetworkManager.singleton.StartClient();
        }*/
    }
}