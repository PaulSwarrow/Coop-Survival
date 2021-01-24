using System;
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


        private GameCharacter target;
        public GameCharacter Target => target; //hide by interface?

        public override void Init()
        {
        }
        

        public void SetCharacter(GameCharacter character)
        {
            Assert.IsNotNull(character);
            if(target != null) ClearCharacter();
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
            // GameManager.UpdateEvent += OnUpdate;
        }

        private void OnUpdate()
        {
            var aim = Input.GetButton("Fire2");
            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            var q = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
            var inputVector = q * Vector3.ClampMagnitude(input, 1);
            var lookVector = q * Vector3.forward;

            target.actor.motor.Move(inputVector);
            target.actor.motor.Aiming = aim;
            if (aim) target.actor.motor.Look(lookVector);
        }

        public override void Unsubscribe()
        {
            // GameManager.UpdateEvent += OnUpdate;
        }
    }
}