using System;
using Game.Data;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Tools
{
    public class CharacterAnimator : NetworkBehaviour
    {
        private static readonly int AimKey = Animator.StringToHash("armed");
        private static readonly int ForwardKey = Animator.StringToHash("forward");
        private static readonly int StrafeKey = Animator.StringToHash("strafe");
        private static readonly int TurnKey = Animator.StringToHash("turn");

        private struct InputData
        {
            public bool aim;
        }

        private InputData data;
        private InputData cachedData;

        [SerializeField] private Animator animator;

        [Inject] private NavMeshAgent agent;

        private void Update()
        {
            if (hasAuthority)
            {
                if (!data.Equals(cachedData))
                {
                    cachedData = data;
                    SyncUp(data);
                }
            }

            var localVelocity = transform.InverseTransformVector(agent.velocity);
            localVelocity *= ((int) Speed / agent.speed);

            var turn = Vector3.SignedAngle(transform.forward, Forward,Vector3.up) / 10;

            animator.SetFloat(ForwardKey, localVelocity.z);
            animator.SetFloat(StrafeKey, localVelocity.x);
            animator.SetFloat(TurnKey, turn);
            animator.SetBool(AimKey, cachedData.aim);
            
        }
        //API:
        public Transform GetCameraTarget() => animator.GetBoneTransform(HumanBodyBones.Chest);

        public bool Aim
        {
            set => data.aim = value;
        }

        public MovementSpeed Speed { get; set; }//TODO remove hardcode
        public Vector3 Forward { get; set; }

        //SYNC:
        [Command]
        private void SyncUp(InputData input)
        {
            cachedData = input;
            SyncDown(input);
        }

        [ClientRpc]
        private void SyncDown(InputData input)
        {
            cachedData = input;
        }


    }
}