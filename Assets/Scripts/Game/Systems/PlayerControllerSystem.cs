using Game.Models;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerControllerSystem : GameSystem
    {
        [Inject] private GameCharacterSystem characterSystem;


        private GameCharacter target;
        public GameCharacter Target => target; //hide by interface?

        public override void Init()
        {
            target = characterSystem.CreateCharacter(Vector3.zero, Vector3.forward);
        }

        public override void Start()
        {
            GameManager.UpdateEvent += OnUpdate;
        }

        private void OnUpdate()
        {
            
        }

        public override void Stop()
        {
            GameManager.UpdateEvent += OnUpdate;
        }
    }
}