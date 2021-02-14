using System;
using UnityEngine;

namespace App.Tools
{
    public class DirectionGizmo : MonoBehaviour
    {
        [SerializeField] private float length =1;
        [SerializeField] private Color color = Color.white;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawRay(transform.position, transform.forward * length);
        }
    }
}