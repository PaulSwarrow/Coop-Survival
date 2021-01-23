using Game.Models;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerControllerSystem : GameSystem
    {
        [Inject] private GameCharacterSystem characterSystem;


        private GameCharacter target;
        public GameCharacter Target => target; //hide by inteface?

        public override void Init()
        {
            target = characterSystem.CreateCharacter(Vector3.zero, Vector3.forward);
        }

        public override void Start()
        {
            
        }

        public override void Stop()
        {
        }
    }
}