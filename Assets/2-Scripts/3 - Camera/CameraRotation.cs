using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 target;


    private void Start()
    {
        target.x = player.position.x;
        target.y = player.position.y;
        target.z = player.position.z;
    }

    private void Update()
    {
        transform.LookAt(player);
    }

    public void RotateLeft()
    {
        transform.RotateAround(target, Vector3.up, 30);
    }

    public void RotateRight()
    {
        transform.RotateAround(target, Vector3.up, -30);
    }

    
}
