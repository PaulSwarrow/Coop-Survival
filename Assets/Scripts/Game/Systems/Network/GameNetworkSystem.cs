using App.Tools;
using DefaultNamespace.Messages;
using Game.Actors;
using Libs.GameFramework;
using Mirror;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class GameNetworkSystem : GameSystem
    {
        [Inject] protected GameNetworkManager _networkManager;
        [Inject] protected PlayerControllerSystem userController;

        public override void Subscribe()
        {
            _networkManager.ReadyEvent += OnReady; 
            NetworkClient.RegisterHandler<GiveCharacterMessage>(OnCharacterGiven);
        }

        private void OnReady()
        {
            
        }

        private void OnCharacterGiven(GiveCharacterMessage message)
        {
            var character = NetworkIdentity.spawned[message.actor];

            var actor = character.GetComponent<GameCharacterActor>();
            userController.SetCharacter(actor);
        }

        public override void Unsubscribe()
        {
            _networkManager.ReadyEvent -= OnReady;
        }

        public NetworkIdentity Client => NetworkClient.connection.identity;
    }
}