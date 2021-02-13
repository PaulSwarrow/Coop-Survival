using System.Collections.Generic;
using Game.Actors;
using Game.Data;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName ="WeaponConfig", menuName = "Game/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        public WeaponActor prefab;
    }
}