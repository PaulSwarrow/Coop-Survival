using System;
using UnityEngine;

namespace Game.Actors
{
    public class GameCharacterActor : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        public Transform cameraTarget { get;  private set; }

        private void Awake()
        {
            cameraTarget = animator.GetBoneTransform(HumanBodyBones.Chest);
        }
    }
}