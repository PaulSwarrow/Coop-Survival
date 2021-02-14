using System;
using Game.Data;
using Game.Tools;
using Game.View;
using Libs.GameFramework;
using Mirror;
using UnityEngine;

namespace Game.Actors.Components
{
    
    public class CharacterItemHolder : NetworkBehaviour
    {
        [Inject] private CharacterAnimator animator;
        [Inject] private CharacterMotor motor;
        [SerializeField] private AimSolver aimSolver;
        
        private WeaponActor currentItem;
        private Transform RightHand => animator.GetBone(HumanBodyBones.RightHand);


        public WeaponActor CurrentItem => currentItem;
        public bool IsEmpty => currentItem == null;

        private void Awake()
        {

        }

        private void Update()
        {
            aimSolver.AimTarget = motor.AimTarget;
        }

        public void SetContent(WeaponActor item)
        {
            currentItem = item;
            item.transform.SetParent(RightHand);
            item.FitToHand();
            animator.ItemHoldType = item.holdType;
            aimSolver.holdType = item.holdType;
            animator.SetIKTarget(AvatarIKGoal.RightHand, aimSolver.RightHandIk);
            animator.SetIKTarget(AvatarIKGoal.LeftHand, item.LeftHandIk);
            

            //set aim solver ik offset
        }

        public void RemoveContent()
        {
            currentItem = null;
            
        }
    }
}