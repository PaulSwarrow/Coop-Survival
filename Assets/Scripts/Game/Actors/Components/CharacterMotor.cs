using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Tools;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Game.Actors.Components
{
    public class CharacterMotor : NetworkBehaviour
    {
        private Dictionary<MovementSpeed, float> speedValues = new Dictionary<MovementSpeed, float>
        {
            [MovementSpeed.idle] = 0,
            [MovementSpeed.walk] = 2,
            [MovementSpeed.jog] = 3,
            [MovementSpeed.run] = 6,
        };

        private MovementSpeed speed = MovementSpeed.walk;


        [Inject] private CharacterAnimator animator;

        // [Inject] private NavMeshAgent agent;
        [Inject] private GameCharacterController agent;
        [Inject] private ObstacleDetector obstacleDetector;
        private bool aim;

        private void Start()
        {
            // agent.speed = speedValues[speed];
        }

        private void Update()
        {
            animator.Velocity = agent.Velocity;
        }

        public void SetAim(bool value)
        {
            aim = value;
            animator.Aim = value;
        }

        public void SetSpeed(MovementSpeed value)
        {
            speed = value;
            // agent.speed = speedValues[speed];
        }

        public void Move(Vector3 vector)
        {
            agent.Move(vector * (speedValues[speed] * Time.deltaTime));
        }

        public void Look(Vector3 forward)
        {
            animator.Forward = forward;
            if (aim)
            {
                agent.transform.forward = forward;
            }
            else if(!animator.InAction)
            {
                var currentForward = agent.transform.forward;
                var delta = Vector3.SignedAngle(currentForward, forward, Vector3.up);
                var q = Quaternion.Euler(0, 6 * Time.deltaTime * delta, 0);
                agent.transform.forward = q * currentForward;
            }
        }

        public void ClimbMotion(ParkourMotion motion, ClimbPointInfo climbInfo)
        {
            if (animator.InAction) return;

            var q = Quaternion.LookRotation(climbInfo.normale, Vector3.up);
            var startPoint = climbInfo.startPoint + q * motion.StartOffset;
            agent.Active = false;
            animator.PlayMotion(new AnimationCallData
                {
                    layer = 0,
                    hash = motion.AnimationHash,
                    position = startPoint,
                    rotation = q
                },
                OnActionComplete);
        }

        private void OnActionComplete()
        {
            agent.Active = true;
        }
    }
}