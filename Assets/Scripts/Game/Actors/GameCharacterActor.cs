using System;
using Game.Tools;
using Libs.GameFramework;
using Libs.GameFramework.DI;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace Game.Actors
{
    public class GameCharacterActor : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private NavMeshAgent agent;
        public uint id => identity.netId;

        private NetworkIdentity identity;
        public Transform cameraTarget { get; private set; }

        public CharacterMotor motor { get; private set; }

        private DependencyContainer _dependencyContainer;

        private void Awake()
        {
            motor = new CharacterMotor();

            _dependencyContainer = new DependencyContainer();
            _dependencyContainer.Register(this);
            _dependencyContainer.Register(identity = GetComponent<NetworkIdentity>());
            _dependencyContainer.Register(animator);
            _dependencyContainer.Register(agent);
            _dependencyContainer.Register(motor);
            _dependencyContainer.InjectDependencies();
            cameraTarget = animator.GetBoneTransform(HumanBodyBones.Chest);
        }

        public void SetAuthority(NetworkConnection connection)
        {
            identity.AssignClientAuthority(connection);
        }

        private void Update()
        {
            motor.Update();
        }
    }
}