using System;
using Game.Actors;
using Game.Data;
using Game.Models;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;

namespace DefaultNamespace
{
    public class PlayerControllerSystem : GameSystem
    {
        public event Action CharacterGotEvent;
        public event Action CharacterLooseEvent;

        [Inject] private GameCharacterSystem characterSystem;
        [Inject] private Camera camera;
        [Inject] private BaseGameManager manager;


        private GameCharacterActor target;
        public GameCharacterActor Target => target; //hide by interface?

        public override void Init()
        {
        }


        public void SetCharacter(GameCharacterActor character)
        {
            Assert.IsNotNull(character);
            if (target != null) ClearCharacter();
            target = character;
            CharacterGotEvent?.Invoke();
        }

        public void ClearCharacter()
        {
            target = null;
            CharacterLooseEvent?.Invoke();
        }

        public override void Subscribe()
        {
            manager.UpdateEvent += OnUpdate;
        }

        private void OnUpdate()
        {
            if (target == null) return; //TODO opt

            var aim = Input.GetButton("Fire2");
            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            var q = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
            var moveVector = q * Vector3.ClampMagnitude(input, 1);
            var lookVector = q * Vector3.forward;

            target.Motor.Move(moveVector);
            target.Motor.SetAim(aim);
            if (aim)
            {
                target.Motor.SetSpeed(MovementSpeed.walk);
                target.Motor.Look(lookVector);
            }
            else
            {
                target.Motor.Look(moveVector);
                target.Motor.SetSpeed(Input.GetButton("Run")? MovementSpeed.run : MovementSpeed.jog);
            }
        }

        public override void Unsubscribe()
        {
            manager.UpdateEvent -= OnUpdate;
        }
    }
}