using System.Collections.Generic;
using Game.Data;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Tools
{
    public class CharacterMotor
    {
        private Dictionary<MovementSpeed, float> speedValues = new Dictionary<MovementSpeed, float>
        {
            [MovementSpeed.idle] = 0,
            [MovementSpeed.walk] = 2,
            [MovementSpeed.jog] = 3,
            [MovementSpeed.run] = 6,
        };

        [Inject] private NetworkIdentity _identity;
        [Inject] private Animator _animator;
        [Inject] private NavMeshAgent _agent;
        private MovementSpeed speed = MovementSpeed.walk;

        public CharacterMotor()
        {
        }

        public bool Aiming
        {
            set => _animator.SetBool("armed", value);
        }

        public void Update()
        {
            if(!_identity.hasAuthority) return;
            
            var localVelocity = _animator.transform.InverseTransformVector(_agent.velocity);
            localVelocity *= ((int) speed / _agent.speed);

            _animator.SetFloat("forward", localVelocity.z);
            _animator.SetFloat("strafe", localVelocity.x);
        }

        public void Move(Vector3 inputVector)
        {
            _agent.speed = speedValues[speed];
            _agent.velocity = inputVector * _agent.speed;
        }


        public void Look(Vector3 forward)
        {
            // _agent.updateRotation = false;
            _agent.transform.forward = forward;
        }
    }
}