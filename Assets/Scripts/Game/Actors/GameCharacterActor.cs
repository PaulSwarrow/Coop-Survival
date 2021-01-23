using System;
using Game.Tools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace Game.Actors
{
    public class GameCharacterActor : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private NavMeshAgent agent;
        public Transform cameraTarget { get; private set; }

        public CharacterMotor motor { get; private set; }

        private void Awake()
        {
            cameraTarget = animator.GetBoneTransform(HumanBodyBones.Chest);
            motor = new CharacterMotor(animator, agent);
        }

        private void Update()
        {
            motor.Update();
        }
    }
}