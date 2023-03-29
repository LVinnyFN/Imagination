using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSomething : MonoBehaviour
{
    [SerializeField] private GameObject something;

    private void Update()
    {
        transform.LookAt(something.transform);
    }
}
