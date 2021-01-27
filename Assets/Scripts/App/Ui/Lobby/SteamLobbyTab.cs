using System.Collections.Generic;
using Mirror;
using Mirror.FizzySteam;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Ui.Lobby
{
    public class SteamLobbyTab : BaseLobbyTab
    {
        [SerializeField] private Text labelUser;
        [SerializeField] private Dropdown dropdownSteamFriends;

        private List<CSteamID> friendSteamIDs = new List<CSteamID>();

        private void Start()
        {
            
            if (NetworkManager.singleton.TryGetComponent(out FizzySteamworks transport) && SteamManager.Initialized)
            {
                Transport = transport;
                labelUser.text = "Current Steam User: " + SteamFriends.GetPersonaName();


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

                Available = true;
                dropdownSteamFriends.value = 1;
            }
            else
            {
                Available = false;
            }
            
        }


        public override string GetSelectedAddress()
        {
            // Transport.activeTransport = SteamNetworkManager.steam;
            Debug.Log("connect to friend index " + dropdownSteamFriends.value);
            Debug.Log("connect to friend steam ID " + friendSteamIDs[dropdownSteamFriends.value].ToString());
            return friendSteamIDs[dropdownSteamFriends.value].ToString();
        }
    }
}