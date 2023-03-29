using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luminaria : MonoBehaviour
{
    [SerializeField] private GameObject minhaluz;

    private void Start()
    {
        minhaluz.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minhaluz.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minhaluz.SetActive(false);
        }
    }
}
