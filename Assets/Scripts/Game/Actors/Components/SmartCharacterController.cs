using System;
using UnityEngine;

namespace Game.Actors.Components
{
    public class SmartCharacterController : MonoBehaviour
    {
        [SerializeField] private float r = .5f;
        [SerializeField] private int count = 6;
        [SerializeField] private float edgeForce = 1.5f;
        [SerializeField] private CharacterController controller;
        public Vector3 Velocity => controller.velocity;

        private bool safeGround;

        private Vector3 virtualVelocity;

        private void Update()
        {
            // controller.Move(Vector3.down * (9.8f * Time.deltaTime));


            var origin = transform.position + Vector3.up;
            var distance = 1 + controller.skinWidth + controller.stepOffset;
            var normaleSumm = Vector3.zero;
            var forward = transform.forward;
            var safe = 0;
            for (int i = 0; i < count; i++)
            {
                var q = Quaternion.Euler(0, i * 360f / count, 0);
                var ray = new Ray(origin + q * (forward * r), Vector3.down);
                if (Physics.Raycast(ray, distance))
                {
                    var normale = (origin - ray.origin).normalized;
                    normaleSumm += normale;
                    safe++;
                    // Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);
                }
                else
                {
                    // Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);
                }
            }

            virtualVelocity += Physics.gravity * Time.deltaTime;

            Debug.DrawRay(origin, normaleSumm, Color.blue);
            if (safe < count / 2f)
            {
                safeGround = false;
                virtualVelocity += normaleSumm * Time.deltaTime;
                virtualVelocity -= virtualVelocity * (0.4f * Time.deltaTime);
            }
            else
            {
                safeGround = true;
            }

            if (controller.isGrounded) virtualVelocity.y = -1;
            controller.Move(virtualVelocity * Time.deltaTime);
        }


        public void Move(Vector3 speedValue)
        {
            if (safeGround)
            {
                var y = virtualVelocity.y;
                virtualVelocity = speedValue / Time.deltaTime;
                virtualVelocity.y = y;
            }
        }
    }
}