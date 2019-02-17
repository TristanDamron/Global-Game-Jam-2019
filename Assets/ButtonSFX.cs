using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData p)
    {
        AudioController.PlaySFX("sfx_ui_highlight");
    }

    public void OnPointerDown(PointerEventData p)
    {
        AudioController.PlaySFX("sfx_ui_click");
    }
}
