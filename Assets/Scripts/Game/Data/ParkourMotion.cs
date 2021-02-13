using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class ParkourMotion
    {
        public string Animation;
        public float MinHeight;
        public float MaxHeight;
        public Vector3 StartOffset;
        public ClimbType ClimbType;

        private int animationHash;

        public int AnimationHash =>
            animationHash == 0 ? animationHash = Animator.StringToHash(Animation) : animationHash;

        public bool Match(ClimbPointInfo info)
        {
            return info.climbHeight < MaxHeight &&
                   info.climbHeight > MinHeight &&
                   info.climbType == ClimbType;
        }
    }
}