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

        private struct InputData
        {
            public bool aim;
        }

        private InputData data;
        private InputData cachedData;

        [SerializeField] private Animator animator;

        [Inject] private NavMeshAgent agent;

        private MovementSpeed speed = MovementSpeed.walk; //TODO remove hardcode

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
            localVelocity *= ((int) speed / agent.speed);


            animator.SetFloat(ForwardKey, localVelocity.z);
            animator.SetFloat(StrafeKey, localVelocity.x);
            animator.SetBool(AimKey, cachedData.aim);
        }
        //API:
        public Transform GetCameraTarget() => animator.GetBoneTransform(HumanBodyBones.Chest);

        public bool Aim
        {
            set => data.aim = value;
        }

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