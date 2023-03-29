using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholderCollider : MonoBehaviour
{
    [SerializeField] private GameObject beholderlight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            beholderlight.SetActive(true);
        }
    }
}
