using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : MonoBehaviour
{
    void Awake()
    {
        GameController.controller.player = this.gameObject;
    }
}
