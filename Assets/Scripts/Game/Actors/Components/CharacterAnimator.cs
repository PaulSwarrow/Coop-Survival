using System;
using System.Collections;
using System.Linq;
using Game.Data;
using Game.View;
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
        [SerializeField] private AnimatorHelper animatorHelper;
        private bool rootMotion;

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

            var localVelocity = transform.InverseTransformVector(NormalizedVelocity);

            var turn = Vector3.SignedAngle(transform.forward, Forward, Vector3.up) / 10;

            animator.SetFloat(ForwardKey, localVelocity.z);
            animator.SetFloat(StrafeKey, localVelocity.x);
            animator.SetFloat(TurnKey, turn);
            animator.SetBool(AimKey, cachedData.aim);

            if (rootMotion) //TODO blend
            {
                transform.position += animatorHelper.Movement;
                transform.rotation *= animatorHelper.Rotation;
            }
        }

        //API:
        public Transform GetCameraTarget() => animator.GetBoneTransform(HumanBodyBones.Chest);

        public bool Aim
        {
            set => data.aim = value;
        }

        public Vector3 NormalizedVelocity { get; set; } //TODO remove hardcode
        public Vector3 Forward { get; set; }


        public IEnumerator ActionCoroutine(int layer, int stateNameHash, bool rootMotion, Action callback)
        {
            float fadeInNTime = .1f;
            float fadeOutNTime = .15f;
            var cachedState = animator.GetCurrentAnimatorStateInfo(layer);
            animator.CrossFade(stateNameHash, fadeInNTime, layer);

            var duration = 1.03f; // animator.GetNextAnimatorClipInfo(layer).First().clip.length;

            this.rootMotion = rootMotion;

            yield return new WaitForSeconds(duration * (1 - fadeOutNTime));

            animator.CrossFade(cachedState.fullPathHash, fadeOutNTime, layer);

            yield return new WaitForSeconds(fadeOutNTime);
            this.rootMotion = false;

            callback();
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