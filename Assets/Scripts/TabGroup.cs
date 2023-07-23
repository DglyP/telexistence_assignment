using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TabGroup : MonoBehaviour
    {
    public List<TabButton> tabButtons;
    public TabButton selectedTab;
    public Sprite buttonSprite;
    public void Subscribe(TabButton button)
        {
        if (tabButtons == null)
            {
            tabButtons = new List<TabButton>();
            }
        tabButtons.Add(button);
        }

    public void OnTabEnter(TabButton button)
        {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
            {
            }
        }

    public void OnTabExit(TabButton button)
        {
        ResetTabs();
        }

    public void OnTabSelected(TabButton button)
        {
        selectedTab = button;
        ResetTabs();
        button.labelText.fontStyle |= TMPro.FontStyles.Underline;
        }

    public void ResetTabs()
        {
        foreach (TabButton button in tabButtons)
            {
            button.background.sprite = buttonSprite;
            if (selectedTab != null && button == selectedTab) { continue; }
            button.labelText.fontStyle &= ~TMPro.FontStyles.Underline;
            }
        }
    }
