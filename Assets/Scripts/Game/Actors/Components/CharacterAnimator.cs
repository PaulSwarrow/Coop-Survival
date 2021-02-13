using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
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

        [SerializeField] private AnimatorHelper animator;
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

            var localVelocity = transform.InverseTransformVector(Velocity);

            var turn = Vector3.SignedAngle(transform.forward, Forward, Vector3.up) / 10;

            animator.SetFloat(ForwardKey, localVelocity.z);
            animator.SetFloat(StrafeKey, localVelocity.x);
            animator.SetFloat(TurnKey, turn);
            animator.SetBool(AimKey, cachedData.aim);

            if (rootMotion) //TODO blend
            {
                transform.position += animator.Movement;
                transform.rotation *= animator.Rotation;
            }
        }

        //API:
        public bool InAction { get; private set; }
        public Transform GetCameraTarget() => animator.ChestTransform;

        public bool Aim
        {
            set => data.aim = value;
        }

        public Vector3 Velocity { get; set; } //TODO remove hardcode
        public Vector3 Forward { get; set; }

        public void PlayMotion(int layer, int hash, Action callback)
        {
            var data = new AnimationCallData
            {
                layer = layer,
                hash = hash,
                position = transform.position,
                rotation = transform.rotation
            };
            PlayMotion(data, callback);
        }

        public void PlayMotion(AnimationCallData data, Action callback)
        {
            PlayMotionCommand(data);
            StartCoroutine(CoroutineWrapper(ActionCoroutine(data), callback));
        }

        [Command]
        private void PlayMotionCommand(AnimationCallData data)
        {
            //TODO handle standalone server
            PlayMotionRrc(data);
        }

        [ClientRpc]
        private void PlayMotionRrc(AnimationCallData data)
        {
            if (hasAuthority) return;
            StartCoroutine(ActionCoroutine(data));
        }

        private IEnumerator CoroutineWrapper(IEnumerator coroutine, Action callback)
        {
            yield return coroutine;
            callback.Invoke();
        }

        private IEnumerator ActionCoroutine(AnimationCallData data)
        {
            //TODO: add blending curves
            //TODO ik points
            //TODO smooth fade out
            InAction = true;
            float fadeInNTime = .051f;
            float fadeOutNTime = .25f;
            var cachedState = animator.GetCurrentState(data.layer);
            if (cachedState.shortNameHash == data.hash)
            {
                Debug.LogError("Actions overlap");
            }

            var duration = 1.03f; // animator.GetNextAnimatorClipInfo(layer).First().clip.length;

            //TODO cache transforms
            var actualPosition = animator.transform.position;
            var actualRotation = animator.transform.rotation;

            transform.position = data.position;
            transform.rotation = data.rotation;

            animator.transform.position = actualPosition;
            animator.transform.rotation = actualRotation;

            animator.transform.DOLocalMove(Vector3.zero, duration * .06f);
            animator.transform.DOLocalRotateQuaternion(Quaternion.identity, duration * .06f);

            animator.CrossFade(data.hash, fadeInNTime, data.layer);


            rootMotion = true;

            yield return new WaitForSeconds(duration * (1 - fadeOutNTime));

            animator.CrossFade(cachedState.fullPathHash, fadeOutNTime, data.layer);

            yield return new WaitUntil(() =>
            {
                return animator.GetCurrentState(data.layer).shortNameHash == cachedState.shortNameHash;
            });
            //TODO it seems coroutine ends too early (can cause freeze if called one after another)
            // yield return new WaitForSeconds(fadeOutNTime*duration);
            rootMotion = false;
            InAction = false;
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