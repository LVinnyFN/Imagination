using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mushroomson"))
        {
            other.GetComponent<MushroomSon>().EnterPortal();
        }
    }
}
