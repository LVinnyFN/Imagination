using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string skill;

    public void OnPointerEnter(PointerEventData eventdata)
    {
        GameController.controller.uiController.ShowSkillTooltip(true, skill);
    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        GameController.controller.uiController.ShowSkillTooltip(false, skill);
    }
}