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
            NetworkServer.RegisterHandler<GiveCharacterMessage>(OnCharacterGiven);
            
        }

        private void OnCharacterGiven(GiveCharacterMessage message)
        {
            var character = ClientScene.spawnableObjects[message.actor];
            
            userController.SetCharacter(character.GetComponent<GameCharacterActor>());
        }

        public override void Unsubscribe()
        {
        }
    }
}