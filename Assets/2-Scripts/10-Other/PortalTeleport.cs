using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{

    [SerializeField] private GameObject teleportquestion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            teleportquestion.SetActive(true);
            GameController.controller.uiController.UseMouse(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            teleportquestion.SetActive(false);
        }
    }

}
