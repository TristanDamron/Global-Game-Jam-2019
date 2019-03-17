using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData p)
    {
        DynamicAudioController.Play("sfx_ui_highlight");
    }
    public void OnPointerDown(PointerEventData p)
    {
        DynamicAudioController.Play("sfx_ui_click");
    }
}
