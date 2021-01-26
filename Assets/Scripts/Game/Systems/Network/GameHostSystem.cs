using DefaultNamespace.Messages;
using Game.Models;
using Libs.GameFramework;
using Mirror;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameHostSystem : GameNetworkSystem
    {
        [Inject] private SaveLoadSystem _saveLoadSystem;
        [Inject] private GameCharacterSystem characters;
        public override void Subscribe()
        {
            base.Subscribe();
            _networkManager.SessionStart += OnSessionStart;
            _networkManager.PlayerAddedEvent += OnNewPlayer;

        }

        private void OnSessionStart()
        {
            _saveLoadSystem.SpawnCharacters();
        }
        
        private void OnNewPlayer(NetworkConnection connection)
        {
            if (characters.TryFind(item => item.owner == null, out var character))
            {
                SetPlayerControl(character, connection);
                character.actor.SetAuthority(connection);
            }
        }

        private void SetPlayerControl(GameCharacter character, NetworkConnection connection)
        {
            character.owner = connection.identity;
            NetworkServer.SendToClientOfPlayer(connection.identity, new GiveCharacterMessage
            {
                actor = character.actor.id
            });
        }


        public override void Unsubscribe()
        {
            base.Unsubscribe();
            _networkManager.SessionStart -= OnSessionStart;
        }
    }
}