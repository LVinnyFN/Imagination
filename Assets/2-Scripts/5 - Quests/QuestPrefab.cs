using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    public Quest myQuest;
    public GameObject mirror;
    public GameObject mylittlestar;

    public void SelectQuest()
    {
        if (GetComponentInParent<QuestMenu>() == null)
        {
            GetComponentInParent<QuestLog>().SelectQuest(this.gameObject);
        }
        else
        {
            GetComponentInParent<QuestMenu>().SelectQuest(this.gameObject);
        }

    }
}
