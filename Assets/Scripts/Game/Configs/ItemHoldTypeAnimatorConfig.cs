using System;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName ="ItemHoldTypeAnimatorConfig", menuName = "Game/ItemHoldTypeAnimatorConfig")]
    public class ItemHoldTypeAnimatorConfig : ScriptableObject
    {
        [Serializable]
        public class ItemOffset
        {
            public string id => holdType.ToString();
            public ItemHoldType holdType;
            public Vector3 position;
            public Vector3 rotation;
        }

        public List<ItemOffset> offsets;
    }
}