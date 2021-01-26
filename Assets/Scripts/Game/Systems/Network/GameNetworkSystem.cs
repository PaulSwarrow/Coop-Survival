using DefaultNamespace.Messages;
using Game.Actors;
using Libs.GameFramework;
using Mirror;

namespace DefaultNamespace
{
    public abstract class GameNetworkSystem : GameSystem
    {
        [Inject] protected GameNetworkManager _networkManager;
        [Inject] protected PlayerControllerSystem userController;

        public override void Subscribe()
        {
            NetworkClient.RegisterHandler<GiveCharacterMessage>(OnCharacterGiven);
        }

        private void OnCharacterGiven(GiveCharacterMessage message)
        {
            var character = NetworkIdentity.spawned[message.actor];

            userController.SetCharacter(character.GetComponent<GameCharacterActor>());
        }

        public override void Unsubscribe()
        {
        }

        public NetworkIdentity Client => NetworkClient.connection.identity;
    }
}