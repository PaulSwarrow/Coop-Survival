using Cinemachine;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerCameraSystem : GameSystem
    {
        [Inject] private CinemachineFreeLook vcamera;
        [Inject] private PlayerControllerSystem playerController;
        public override void Start()
        {
            vcamera.Follow = playerController.Target.actor.transform;
            vcamera.LookAt = playerController.Target.actor.cameraTarget;
        }


        public override void Stop()
        {
            
        }
    }
}