using UnityEngine;
using UnityEngine.AI;

namespace Game.Tools
{
    public class CharacterMotor
    {
        private readonly Animator _animator;
        private readonly NavMeshAgent _agent;

        public CharacterMotor(Animator animator, NavMeshAgent agent)
        {
            _animator = animator;
            _agent = agent;
        }

        public void Update()
        {
            var localVelocity = _animator.transform.InverseTransformVector(_agent.velocity);
            _animator.SetFloat("forward", localVelocity.magnitude);
            _animator.SetFloat("strafe", localVelocity.x);
            
        }

        public void Move(Vector3 inputVector)
        {
            _agent.velocity = inputVector * _agent.speed;
        }
    }
}