using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MushroomGuard : MonoBehaviour
{

    [SerializeField] private GameObject dialogbox;
    public GameObject mywall;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 dir = other.transform.position-transform.position;
            Quaternion lookrotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookrotation, 5*Time.deltaTime).eulerAngles;
            transform.rotation = Quaternion.Euler(0, rotation.y, 0);
            //transform.LookAt(other.transform.GetChild(0).position);
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogbox.SetActive(true);
                GameController.controller.uiController.UseMouse(true);
                switch (name)
                {
                    case "MushroomGuard1":
                        dialogbox.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "You must deliver the mushroom rescue quest (Part I) before i allow your pass";
                        break;
                    case "MushroomGuard2":
                        dialogbox.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "You must deliver the mushroom rescue quest (Part II) before i allow your pass";
                        break;
                    case "MushroomGuard3":
                        dialogbox.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "You must deliver the mushroom rescue quest (Part III) before i allow your pass";
                        break;
                    case "MushroomGuard4":
                        dialogbox.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "You must deliver the mushroom rescue quest (Part IV) before i allow your pass";
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("talk", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("talk", false);
            dialogbox.SetActive(false);
            GameController.controller.uiController.UseMouse(false);
        }
    }
}
