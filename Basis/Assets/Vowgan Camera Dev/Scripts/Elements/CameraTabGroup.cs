using System.Collections.Generic;
using UnityEngine;

namespace Vowgan.HandheldCameraSystem
{
    public class CameraTabGroup : MonoBehaviour
    {

        [field: SerializeField] public FocusableButton currentTab { get; private set; }
        [field: SerializeField] public GameObject currentPage { get; private set; }

        [SerializeField] private Dictionary<FocusableButton, GameObject> _tabPages = new();



        private void Awake()
        {
            if (_tabPages.ContainsKey(currentTab))
            {
                OnTabSelected(currentTab);
            }
        }

        public void AddTab(FocusableButton tab, GameObject page, bool switchImmediate = false)
        {
            _tabPages[tab] = page;
            tab.onClick.AddListener(() => OnTabSelected(tab));

            if (switchImmediate) OnTabSelected(tab);
        }

        public void RemoveTab(FocusableButton tab)
        {
            GameObject page = _tabPages[tab];

            _tabPages.Remove(tab);
            tab.onClick.RemoveListener(() => OnTabSelected(tab));

            if (currentTab == tab)
                OnTabSelected(null);
        }

        private void OnTabSelected(FocusableButton tab)
        {
            if (tab == currentTab) return;

            currentTab?.SetFocused(false);
            currentTab = tab;
            currentTab?.SetFocused(true);

            currentPage?.SetActive(false);
            currentPage = _tabPages[tab];
            currentPage?.SetActive(true);
        }
    }
}
