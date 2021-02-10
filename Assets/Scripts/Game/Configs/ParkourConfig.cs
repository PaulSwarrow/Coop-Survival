using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName ="ParkourMotionList", menuName = "Game/ParkourMotionList")]
    public class ParkourConfig : ScriptableObject
    {
        public List<ParkourMotion> climbing;


    }
}