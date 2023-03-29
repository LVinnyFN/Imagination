using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamparina : MonoBehaviour
{
    [SerializeField] private GameObject vagalume;
    [SerializeField] private GameObject minhaluz;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vagalume.SetActive(true);
            minhaluz.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            vagalume.SetActive(false);
            minhaluz.SetActive(false);
        }

    }

}
