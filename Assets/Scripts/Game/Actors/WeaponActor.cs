using System;
using Game.Data;
using Mirror;
using UnityEngine;

namespace Game.Actors
{
    //TODO rename to item
    public class WeaponActor : NetworkBehaviour
    {
        [SerializeField] private Transform leftHandIk;
        [SerializeField] private Transform directionDefiner;
        public Vector3 offset;
        public Vector3 rotation;
        public ItemHoldType holdType;

        private void Start()
        {
        }
        
        public Transform LeftHandIk => leftHandIk;

        public void FitToHand()
        {
            transform.localPosition = offset;
            transform.localRotation = Quaternion.Euler(rotation);
        }
    }
}