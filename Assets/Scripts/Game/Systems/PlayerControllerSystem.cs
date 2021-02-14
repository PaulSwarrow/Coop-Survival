using System;
using System.Linq;
using App;
using Game.Actors;
using Game.Configs;
using Game.Data;
using Game.Models;
using Lib.UnityQuickTools.Collections;
using Libs.GameFramework;
using Libs.GameFramework.Systems;
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
        [Inject] private ObjectSpawnSystem spawnSystem;


        private GameCharacterActor target;
        private ParkourConfig parkourConfig;
        private Vector3 aimTarget;
        public GameCharacterActor Target => target; //hide by interface?


        public override void Init()
        {
            parkourConfig = AppManager.current.resources.parkourConfig;
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
            manager.GizmosEvent += DrawGizmos;
            manager.UpdateEvent += OnUpdate;
        }

        private void DrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(aimTarget, 1);
        }

        private void OnUpdate()
        {
            if (target == null) return; //TODO opt

            if (target.Holder.IsEmpty)
            {

                //TEST:
                var weaponConfig = AppManager.current.resources.weapons.First();
                var weapon = spawnSystem.Spawn(weaponConfig.prefab);
                target.Holder.SetContent(weapon);
            }
            
            var aim = Input.GetButton("Fire2");
            aim = true;
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

                aimTarget = camera.transform.position + camera.transform.forward * 1000;
                target.Motor.AimTarget = aimTarget;
            }
            else
            {
                if (Input.GetButtonDown("Jump") && target.ObstacleDetector.CheckClimbAbility(out var climbInfo))
                {
                    //TODO better motion selection (distance and speed based)
                    //TODO motion ranking (for character upgrades)
                    if (parkourConfig.climbing.TryFind(item => item.Match(climbInfo), out var motion))
                    {
                        target.Motor.ClimbMotion(motion, climbInfo);
                    }
                }

                target.Motor.Look(moveVector);
                target.Motor.SetSpeed(Input.GetButton("Run") ? MovementSpeed.run : MovementSpeed.jog);
            }
        }

        public override void Unsubscribe()
        {
            manager.UpdateEvent -= OnUpdate;
        }
    }
}