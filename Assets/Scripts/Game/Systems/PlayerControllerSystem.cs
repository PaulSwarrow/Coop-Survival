using Game.Models;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerControllerSystem : GameSystem
    {
        [Inject] private GameCharacterSystem characterSystem;
        [Inject] private Camera camera;


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
            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            var q = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
            var inputVector = q * Vector3.ClampMagnitude(input, 1);

            target.Move(inputVector);
        }

        public override void Stop()
        {
            GameManager.UpdateEvent += OnUpdate;
        }
    }
}