using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutbee : MonoBehaviour
{
    [SerializeField] private TutorialMush tutmush;

    void Update()
    {
        if (GetComponent<Enemy>().isDead)
        {
            tutmush.GetComponent<BoxCollider>().enabled = true;
            tutmush.beedone = true;
            tutmush.exclamation.SetActive(true);
        }
    }
}
