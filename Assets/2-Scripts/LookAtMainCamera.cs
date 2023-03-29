using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    private Camera target;

    private void Start()
    {
        target = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(target.transform);
    }
}
