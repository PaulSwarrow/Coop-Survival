using System;
using Game.Data;
using UnityEngine;

namespace Game.Actors.Components
{
    public class GameCharacterController : BaseActorPositionHandler
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
            if (hasAuthority)
            {

                var origin = transform.position + Vector3.up;
                var distance = 1 + controller.skinWidth + controller.stepOffset;
                var normaleSumm = Vector3.zero;
                var forward = transform.forward;
                var safe = 0;
                for (var i = 0; i < count; i++)
                {
                    var q = Quaternion.Euler(0, i * 360f / count, 0);
                    var ray = new Ray(origin + q * (forward * r), Vector3.down);
                    if (Physics.Raycast(ray, distance))
                    {
                        var normale = (origin - ray.origin).normalized;
                        normaleSumm += normale;
                        safe++;
                    }
                }

                virtualVelocity += Physics.gravity * Time.deltaTime;
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
            }

            controller.Move(virtualVelocity * Time.deltaTime);
        }
        
        public bool Active
        {
            get => controller.enabled;
            set => controller.enabled = value;
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

        protected override void WriteData(ref PositionSyncData data)
        {
            data.velocity = virtualVelocity;
            data.forward = transform.forward;
            data.enabled = Active;
        }

        protected override void ApplyData(ref PositionSyncData data, Vector3 position)
        {
            transform.position = position;
            transform.forward = data.forward;
            virtualVelocity = data.velocity;
            controller.enabled = data.enabled;
        }

        protected override void SyncLateUpdate(ref PositionSyncData data)
        {
            base.SyncLateUpdate(ref data);
            virtualVelocity = Vector3.Lerp(virtualVelocity, data.velocity, 10 * Time.deltaTime);
        }
    }
}