using Mirror;
using UnityEngine;

namespace App.Ui.Lobby
{
    public abstract class BaseLobbyTab : MonoBehaviour
    {
        public bool Available { get; protected set; }
        public Transport Transport { get; protected set; }

        public abstract string GetSelectedAddress();
    }
}