using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Camomila2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.controller.generalaudiosorce.clip = GameController.controller.myaudios[1];
        GameController.controller.generalaudiosorce.volume = 1 * GameController.controller.fxvalue;
        GameController.controller.generalaudiosorce.Play();
        rect.localScale *= 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale /= 1.1f;
    }


    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameController.controller.generalaudiosorce.clip = GameController.controller.myaudios[2];
        GameController.controller.generalaudiosorce.volume = 1*GameController.controller.fxvalue;
        GameController.controller.generalaudiosorce.Play();
    }

}
