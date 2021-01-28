using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace App.Tools
{
    public class ObjectPropertiesOutput : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private Object target;
        private FieldInfo[] fields = new FieldInfo[0];

        public void SetTarget(Object target)
        {
            this.target = target;
            fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        private void Update()
        {
            var str = "";
            foreach (var field in fields)
            {
                str += $"{field.Name}: {field.GetValue(target)} \n";
            }

            _text.text = str;
        }
    }
}