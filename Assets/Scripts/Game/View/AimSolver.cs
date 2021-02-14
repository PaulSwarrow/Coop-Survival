using System;
using Game.Configs;
using Game.Data;
using Lib.UnityQuickTools.Collections;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.View
{
    public class AimSolver : MonoBehaviour
    {
        [SerializeField] private ItemHoldTypeAnimatorConfig holdItemsConfig;
        [SerializeField] private Animator animator; //TODO solve dependency
        [SerializeField] private Transform itemHolder;

        private Transform shoulder;
        private Transform _transform;
        private ItemHoldTypeAnimatorConfig.ItemOffset holdItemIkOffsset;

        private void Awake()
        {
            _transform = transform;
            shoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
        }

        private void Update()
        {
            var position = shoulder.position;
            _transform.position = position;
            _transform.rotation = Quaternion.LookRotation(AimTarget - position, Vector3.up);
        }


        public ItemHoldType holdType
        {
            set
            {
                if (holdItemIkOffsset == null || holdItemIkOffsset.holdType != value)
                {
                    if (holdItemsConfig.offsets.TryFind(item => item.holdType == value, out holdItemIkOffsset))
                    {
                        RightHandIk.localPosition = holdItemIkOffsset.position;
                        RightHandIk.localEulerAngles = holdItemIkOffsset.rotation;
                    }
                }
            }
        }

        public Vector3 AimTarget { get; set; }

        public Transform RightHandIk => itemHolder;

#if UNITY_EDITOR
        public void SaveOffset()
        {
            if (holdItemIkOffsset != null)
            {
                holdItemIkOffsset.position = RightHandIk.localPosition;
                holdItemIkOffsset.rotation = RightHandIk.localEulerAngles;
            }
            
            EditorUtility.SetDirty(holdItemsConfig);
            AssetDatabase.SaveAssets();
        }
        
#endif
    }
}