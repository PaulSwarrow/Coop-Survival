using System;
using Game.Data;
using UnityEngine;

namespace Game.Actors
{
    public class WeaponActor : MonoBehaviour
    {
        public Vector3 offset;
        public Vector3 rotation;
        public WeaponHoldingType holdType;

        private void Start()
        {
            FitToHand();
        }

        public void FitToHand()
        {
            transform.localPosition = offset;
            transform.localRotation = Quaternion.Euler(rotation);
        }
    }
}