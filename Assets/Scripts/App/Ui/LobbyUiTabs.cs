using System;
using System.Collections.Generic;
using System.Linq;
using App.Ui.Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace App.Ui
{
    public class LobbyUiTabs : MonoBehaviour
    {
        [Serializable]
        public class Tab
        {
            public event Action<Tab> ClickEvent;
            public Button btn;
            public BaseLobbyTab view;

            public void Init()
            {
                btn.onClick.AddListener(OnClick);
                Active = false;
            }

            private void OnClick()
            {
                ClickEvent?.Invoke(this);
            }

            public bool Active
            {
                set
                {
                    view.gameObject.SetActive(value);
                    btn.interactable = !value;
                }
            }
        }
        
        
        

        [SerializeField] private List<Tab> tabs;
        private Tab current;

        public event Action TabChangeEvent;
        public BaseLobbyTab Current => current.view;
        private void Awake()
        {
            foreach (var tab in tabs)
            {
                tab.Init();
                tab.ClickEvent += SetCurrent;
            }
            SetCurrent(tabs.First());
        }
        
        private void SetCurrent(Tab tab)
        {
            if (current != null) current.Active = false; 
            tab.Active = true;
            current = tab;
            TabChangeEvent?.Invoke();
        }
    }
}