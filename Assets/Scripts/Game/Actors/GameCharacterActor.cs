using System.Collections.Generic;
using Game.Actors.Components;
using Game.Data;
using Game.Tools;
using Libs.GameFramework;
using Libs.GameFramework.DI;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using NetworkAnimator = Mirror.NetworkAnimator;

namespace Game.Actors
{
    public class GameCharacterActor : NetworkBehaviour
    {
        public uint id => netIdentity.netId;

        public Transform CameraTarget => animator.GetCameraTarget();

        private DependencyContainer dependencyContainer;

        [Inject] public CharacterMotor Motor { get; private set; }
        [Inject] public ObstacleDetector ObstacleDetector { get; private set; }
        [Inject] public CharacterItemHolder Holder { get; private set; }

        [Inject] private CharacterAnimator animator;

        private void Awake()
        {
            // motor = new CharacterMotor();

            dependencyContainer = new DependencyContainer();
            dependencyContainer.Register(this);
            dependencyContainer.Register(netIdentity);
            // dependencyContainer.Register(GetComponent<NavMeshAgent>());
            dependencyContainer.Register(GetComponent<GameCharacterController>());
            dependencyContainer.Register(GetComponent<CharacterAnimator>());
            dependencyContainer.Register(GetComponent<CharacterMotor>());
            dependencyContainer.Register(GetComponent<ObstacleDetector>());
            dependencyContainer.Register(GetComponent<CharacterItemHolder>());
            dependencyContainer.InjectDependencies();
        }


        public void SetAuthority(NetworkConnection connection)
        {
            netIdentity.AssignClientAuthority(connection);
        }
    }
}