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
        tabGroup.OnTabSelected(this);
        }

    public void OnPointerEnter(PointerEventData eventData)
        {
        tabGroup.OnTabEnter(this);
        }

    public void OnPointerExit(PointerEventData eventData)
        {
        tabGroup.OnTabExit(this);
        }

    void Start()
        {
        background = GetComponent<Image>();
        labelText = GetComponentInChildren<TMP_Text>();
        tabGroup.Subscribe(this);
        }
    }
