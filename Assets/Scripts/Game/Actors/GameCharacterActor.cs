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

        private InputData inputData;
        [SyncVar] private InputData _inputData;

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
            InputData data;
            if (hasAuthority)
            {
                data = inputData;
                if (!inputData.Equals(_inputData))
                    SyncUp(inputData);

                inputData.movement = Vector3.zero; //safety
            }
            else
            {
                inputData = SyncDown();
                data = inputData;
            }


            //animate
            var localVelocity = transform.InverseTransformVector(data.movement);
            localVelocity *= ((int) speed / agent.speed);


            animator.SetFloat("forward", localVelocity.z);
            animator.SetFloat("strafe", localVelocity.x);
            animator.SetBool("armed", data.aim);
        }

        public bool Aiming
        {
            get => inputData.aim;
            set => inputData.aim = value;
        }

        public void Move(Vector3 vector)
        {
            inputData.movement = vector * agent.speed;
            //apply input
            agent.velocity = inputData.movement;
        }

        public void Look(Vector3 forward)
        {
            agent.transform.forward = forward;
        }

        private InputData SyncDown()
        {
            return new InputData
            {
                movement = Vector3.Lerp(_inputData.movement, inputData.movement, 10 * Time.deltaTime),
                aim = _inputData.aim
            };
        }

        [Command]
        private void SyncUp(InputData data)
        {
            _inputData = data;
        }

        public struct InputData
        {
            public Vector3 movement;
            public bool aim;
        }
    }
}