using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropMat : MonoBehaviour
{
    public Item myItem;
    public ParticleSystem pickparticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("pickup", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.controller.uiController.ShowAction("pickup",false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bool picked = other.GetComponent<Player>().myinventory.AddItem(myItem, 1);
                if (picked)
                {
                    StartCoroutine(Destroy());
                }
            }
        }
    }


    public IEnumerator Destroy()
    {
        GameController.controller.uiController.ShowAction("pickup",false);
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        pickparticles.Play();
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }
}
