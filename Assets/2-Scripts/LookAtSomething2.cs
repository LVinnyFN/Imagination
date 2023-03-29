using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSomething2 : MonoBehaviour
{
    [SerializeField] private GameObject something;

    private void Update()
    {
        transform.LookAt(new Vector3(something.transform.position.x, something.transform.position.y,0));
    }
}
