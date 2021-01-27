using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace App.Ui.Lobby
{
    public class TelepathLobbyTab : BaseLobbyTab
    {
        [SerializeField] private InputField hostTextField;

        private void Awake()
        {
            hostTextField.text = "localhost";
        }

        private void Start()
        {
            Available = NetworkManager.singleton.TryGetComponent(out TelepathyTransport transport);
            Transport = transport;

        }

        public override string GetSelectedAddress()
        {
            return hostTextField.text;
        }
    }
}