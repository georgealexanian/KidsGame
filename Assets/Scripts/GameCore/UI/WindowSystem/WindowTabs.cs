using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI.WindowSystem
{
    public class WindowTabs : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private List<TabItem> tabItems;
        [SerializeField] private TabItem defaultTab;
        [SerializeField] private Color _color;
        [SerializeField] private Color _selectedColor;
#pragma warning restore

        public void SetDefaultTab(int index)
        {
            for (var i = 0; i < tabItems.Count; i++)
            {
                if (i != index)
                {
                    continue;
                }

                defaultTab = tabItems[i];
            }
        }

        public TabItem GetTab(int idx)
        {
            return tabItems[idx];
        }

        public void SetTabActive(int idx)
        {
            for (int i = 0; i < tabItems.Count; i++)
            {
                var isSelected = i==idx;
                tabItems[i].SetSelected(isSelected);
                tabItems[i].content.SetActive(isSelected);
            }
        }
        
        private void Start()
        {
            foreach (var tabItem in tabItems)
            {
                tabItem.onTabClicked += () => OnTabClicked(tabItem);
                tabItem.SetTextColors(_color, _selectedColor);
            }
            
            OnTabClicked(defaultTab);
        }

        private void OnTabClicked(TabItem tab)
        {
            foreach (var tabItem in tabItems)
            {
                tabItem.SetSelected(tabItem == tab);
                tabItem.content.SetActive(tabItem == tab);
            }
        }
    }
}