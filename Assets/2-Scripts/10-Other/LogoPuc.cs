using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoPuc : MonoBehaviour
{
    public void StartMusic()
    {
        GameController.controller.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
}
