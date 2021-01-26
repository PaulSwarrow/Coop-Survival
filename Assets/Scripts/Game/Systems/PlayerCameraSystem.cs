using Cinemachine;
using Libs.GameFramework;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCameraSystem : GameSystem
    {
        [Inject] private CinemachineFreeLook vcamera;
        [Inject] private PlayerControllerSystem playerController;

        public override void Subscribe()
        {
            playerController.CharacterGotEvent += SetTarget;
        }

        private void SetTarget()
        {
            vcamera.Follow = playerController.Target.transform;
            vcamera.LookAt = playerController.Target.cameraTarget;
        }


        public override void Unsubscribe()
        {
            playerController.CharacterGotEvent -= SetTarget;
        }
    }
}