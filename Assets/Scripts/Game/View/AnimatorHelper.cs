using System;
using UnityEngine;

namespace Game.View
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorHelper : MonoBehaviour
    {
        public Vector3 Movement { get; private set; }
        public Quaternion Rotation { get; private set; }


        //TODO better api
        public Vector3 LookAtTarget { get; set; }
        public float LookAtWeight { get; set; }
        public float TorsoWeight { get; set; }
        public float HeadWeight { get; set; }

        private Transform RightHandIkTarget;
        private Transform LeftHandIkTarget;
        private Vector3 rightHandPos;
        private Vector3 leftHandPos;
        private Quaternion rightHandRot;
        private Quaternion leftHandRot;

        public Transform ChestTransform { get; private set; }
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            ChestTransform = animator.GetBoneTransform(HumanBodyBones.Chest);
        }

        //TODO clean up IK
        private void Update()
        {
            if (RightHandIkTarget)
            {
                rightHandPos = RightHandIkTarget.position;
                rightHandRot = RightHandIkTarget.rotation;
            }
            if (LeftHandIkTarget)
            {
                leftHandPos = LeftHandIkTarget.position;
                leftHandRot = LeftHandIkTarget.rotation;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetLookAtPosition(LookAtTarget);
            animator.SetLookAtWeight(LookAtWeight, TorsoWeight, HeadWeight);

            if (RightHandIkTarget)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            }

            if (LeftHandIkTarget)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            }
        }

        public void SetLayerWeight(int layer, float value)
        {
            animator.SetLayerWeight(layer, value);
        }

        private void OnAnimatorMove()
        {
            Movement = animator.deltaPosition;
            Rotation = animator.deltaRotation;
        }

        public void SetFloat(int key, float value)
        {
            animator.SetFloat(key, value);
        }

        public void SetBool(int key, bool value)
        {
            animator.SetBool(key, value);
        }

        public void SetInt(int key, int value)
        {
            animator.SetInteger(key, value);
        }

        public AnimatorStateInfo GetCurrentState(int layer)
        {
            return animator.GetCurrentAnimatorStateInfo(layer);
        }

        public void CrossFade(int hash, float transitionNormilizedTime, int layer)
        {
            animator.CrossFade(hash, transitionNormilizedTime, layer);
        }

        public Transform GetBone(HumanBodyBones boneId) => animator.GetBoneTransform(boneId);

        public void SetIkTarget(AvatarIKGoal goal, Transform target)
        {
            //TODO better solution
            switch (goal)
            {
                case AvatarIKGoal.RightHand:
                    RightHandIkTarget = target;
                    break;
                case AvatarIKGoal.LeftHand:
                    LeftHandIkTarget = target;
                    break;
            }
        }
    }
}