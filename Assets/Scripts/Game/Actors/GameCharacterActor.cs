using System.Collections.Generic;
using Game.Data;
using Libs.GameFramework.DI;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Actors
{
    public class GameCharacterActor : NetworkBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private NavMeshAgent agent;
        public uint id => netIdentity.netId;

        public Transform cameraTarget { get; private set; }

        public bool Aiming { get; set; }
        // public CharacterMotor motor { get; private set; }

        private DependencyContainer dependencyContainer;


        private Dictionary<MovementSpeed, float> speedValues = new Dictionary<MovementSpeed, float>
        {
            [MovementSpeed.idle] = 0,
            [MovementSpeed.walk] = 2,
            [MovementSpeed.jog] = 3,
            [MovementSpeed.run] = 6,
        };

        private MovementSpeed speed = MovementSpeed.walk;

        //SERVER VARS:
        [SyncVar] private Vector3 _movement;
        [SyncVar] private bool _aiming;

        private void Awake()
        {
            // motor = new CharacterMotor();

            dependencyContainer = new DependencyContainer();
            dependencyContainer.Register(this);
            dependencyContainer.Register(netIdentity);
            dependencyContainer.Register(animator);
            dependencyContainer.Register(agent);
            // _dependencyContainer.Register(motor);
            dependencyContainer.InjectDependencies();
            cameraTarget = animator.GetBoneTransform(HumanBodyBones.Chest);

            agent.speed = speedValues[speed];
        }

        public void SetAuthority(NetworkConnection connection)
        {
            netIdentity.AssignClientAuthority(connection);
        }

        private void Update()
        {
            if (hasAuthority)
            {
                SyncUp(agent.velocity, Aiming);
            }
            else
            {
                SyncDown();
            }

            var localVelocity = transform.InverseTransformVector(agent.velocity);
            localVelocity *= ((int) speed / agent.speed);
            animator.SetFloat("forward", localVelocity.z);
            animator.SetFloat("strafe", localVelocity.x);
            animator.SetBool("armed", Aiming);
        }


        public void Move(Vector3 vector)
        {
            agent.velocity = vector * agent.speed;
        }

        public void Look(Vector3 forward)
        {
            agent.transform.forward = forward;
        }

        private void SyncDown()
        {
            agent.velocity = _movement;
            Aiming = _aiming;
        }

        [Command]
        private void SyncUp(Vector3 movement, bool aim)
        {
            _movement = movement;
            _aiming = aim;
        }
    }
}