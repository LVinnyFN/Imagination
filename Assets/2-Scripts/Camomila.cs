using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Camomila : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rect;
    public GameObject sub;

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
        sub.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale /= 1.1f;
        sub.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameController.controller.generalaudiosorce.clip = GameController.controller.myaudios[2];
        GameController.controller.generalaudiosorce.volume = 1 * GameController.controller.fxvalue;
        GameController.controller.generalaudiosorce.Play();
    }

}
