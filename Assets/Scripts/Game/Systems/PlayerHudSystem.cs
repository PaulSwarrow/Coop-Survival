using Libs.GameFramework;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerHudSystem : GameSystem
    {
        public override void Subscribe()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }

        public override void Unsubscribe()
        {
        }
    }
}