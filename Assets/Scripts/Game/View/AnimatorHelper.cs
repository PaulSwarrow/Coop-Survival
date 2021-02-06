using System;
using UnityEngine;

namespace Game.View
{
    public class AnimatorHelper : MonoBehaviour
    {
        public Vector3 Movement { get; private set; }
        public Quaternion Rotation { get; private set; }

        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();

        }

        private void OnAnimatorMove()
        {
            Movement = animator.deltaPosition;
            Rotation = animator.deltaRotation;

        }

    }
}