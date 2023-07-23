using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
    {
    public List<TabButton> tabButtons; // List of tab buttons in this group
    public TabButton selectedTab; // Currently selected tab button
    public Sprite buttonSprite; // The default sprite for tab buttons

    // Subscribes a tab button to this tab group
    public void Subscribe(TabButton button)
        {
        if (tabButtons == null)
            {
            tabButtons = new List<TabButton>();
            }
        tabButtons.Add(button);
        }

    // Called when the pointer enters a tab button
    public void OnTabEnter(TabButton button)
        {
        ResetTabs();

        // If the button is not the currently selected tab, change its background color
        if (selectedTab == null || button != selectedTab)
            {
            button.background.color = Color.gray;
            }
        }

    // Called when the pointer exits a tab button
    public void OnTabExit(TabButton button)
        {
        ResetTabs();
        }

    // Called when a tab button is selected
    public void OnTabSelected(TabButton button)
        {
        selectedTab = button;
        ResetTabs();
        button.labelText.fontStyle |= TMPro.FontStyles.Underline; // Underline the label text of the selected tab
        }

    // Resets the appearance of all tab buttons in the group to their default state
    public void ResetTabs()
        {
        foreach (TabButton button in tabButtons)
            {
            button.background.sprite = buttonSprite; // Reset the button's background sprite
            button.background.color = Color.black; // Reset the button's background color

            // Remove underline from label text for all tab buttons except the selected one
            if (selectedTab != null && button == selectedTab)
                {
                continue;
                }
            button.labelText.fontStyle &= ~TMPro.FontStyles.Underline;
            }
        }
    }
