using System;
using Game.Tools;
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

        private void Awake()
        {
            identity = GetComponent<NetworkIdentity>();
            cameraTarget = animator.GetBoneTransform(HumanBodyBones.Chest);
            motor = new CharacterMotor(animator, agent);
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