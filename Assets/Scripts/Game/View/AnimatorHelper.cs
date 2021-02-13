using System;
using UnityEngine;

namespace Game.View
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorHelper : MonoBehaviour
    {
        public Vector3 Movement { get; private set; }
        public Quaternion Rotation { get; private set; }
        
        public Transform ChestTransform { get; private set; }

        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            ChestTransform = animator.GetBoneTransform(HumanBodyBones.Chest);
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

        public AnimatorStateInfo GetCurrentState(int layer)
        {
            return animator.GetCurrentAnimatorStateInfo(layer);
        }

        public void CrossFade(int hash, float transitionNormilizedTime, int layer)
        {
            animator.CrossFade(hash, transitionNormilizedTime, layer);
        }
    }
}