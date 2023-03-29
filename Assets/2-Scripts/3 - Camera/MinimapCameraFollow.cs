using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playertransform;

    private void Start()
    {
        playertransform = GameController.controller.player.transform;
    }

    void Update()
    {
        transform.position= new Vector3(playertransform.position.x,transform.position.y,playertransform.position.z);
    }
}
