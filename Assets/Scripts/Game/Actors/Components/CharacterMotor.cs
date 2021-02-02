using System;
using System.Collections.Generic;
using Game.Data;
using Game.Tools;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

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
        [Inject] private NavMeshAgent agent;

        private void Start()
        {
            agent.speed = speedValues[speed];
        }

        public void SetAim(bool value)
        {
            animator.Aim = value;
        }

        public void SetSpeed(MovementSpeed value)
        {
            speed = value;
            animator.Speed = value;
            agent.speed = speedValues[speed];
        }
        public void Move(Vector3 vector)
        {
            agent.velocity = vector * agent.speed;
        }

        public void Look(Vector3 forward)
        {
            animator.Forward = forward;
            var currentForward = agent.transform.forward;
            var delta = Vector3.SignedAngle(currentForward, forward, Vector3.up);
            var q = Quaternion.Euler(0, 10 * delta * Time.deltaTime, 0);
            agent.transform.forward = q * currentForward;
        }
    }
}