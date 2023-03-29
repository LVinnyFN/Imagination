using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneEnter : MonoBehaviour
{
    private int timer;

    [SerializeField] private GameObject feedbackbox;
    [SerializeField] private MushroomSon[] mush1 = new MushroomSon[3];
    [SerializeField] private MushroomSon[] mush2 = new MushroomSon[3];
    [SerializeField] private MushroomSon[] mush3 = new MushroomSon[2];
    [SerializeField] private MushroomSon[] mush4 = new MushroomSon[1];
    [SerializeField] private MushroomSon[] allmush = new MushroomSon[9];

    private void Update()
    {
        timer -= (int)Time.deltaTime;
        if (timer<0)
        {
            timer = 0;
        }
    }

    public void DeliverAll()
    {
        for (int i = 0; i < allmush.Length; i++)
        {
            allmush[i].DeliverMushroom();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && timer<=0)
        {
            timer = 5;
            feedbackbox.SetActive(true);
            feedbackbox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Entering "+this.name;
        }
        switch (this.name)
        {
            case "Mushroom Village":
                for (int i = 0; i < allmush.Length; i++)
                {
                    allmush[i].DeliverMushroom();
                }
                break;
            case "Glowing Garden":
                for (int i = 0; i < mush1.Length; i++)
                {
                    mush1[i].ActivateMushroom();
                }
                break;
            case "Haunted Ship":
                for (int i = 0; i < mush2.Length; i++)
                {
                    mush2[i].ActivateMushroom();
                }
                break;
            case "Lost Forest":
                for (int i = 0; i < mush3.Length; i++)
                {
                    mush3[i].ActivateMushroom();
                }
                break;
            case "Fallen Ruins":
                for (int i = 0; i < mush4.Length; i++)
                {
                    mush4[i].ActivateMushroom();
                }
                break;
            default:
                break;
        }
    }
}
