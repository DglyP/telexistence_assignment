using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
    public TabGroup tabGroup;
    public Image background;
    public TMP_Text labelText;

    public void OnPointerClick(PointerEventData eventData)
        {
        // Called when the tab button is clicked, informs the TabGroup that this button is selected
        tabGroup.OnTabSelected(this);
        }

    public void OnPointerEnter(PointerEventData eventData)
        {
        // Called when the pointer enters the tab button, informs the TabGroup that the button is being hovered over
        tabGroup.OnTabEnter(this);
        }

    public void OnPointerExit(PointerEventData eventData)
        {
        // Called when the pointer exits the tab button, informs the TabGroup that the button is no longer being hovered over
        tabGroup.OnTabExit(this);
        }

    void Start()
        {
        // Get the Image and TMP_Text components from the GameObject
        background = GetComponent<Image>();
        labelText = GetComponentInChildren<TMP_Text>();

        // Subscribe this TabButton to the TabGroup
        tabGroup.Subscribe(this);
        }
    }
