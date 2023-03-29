using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public Player player;
    public float range = 3f;

    void Start()
    {
        player = GameController.controller.player.GetComponent<Player>();
    }


    void Update()
    {
        
    }

    public void Hit()
    {
        Collider[] enemies = Physics.OverlapSphere(this.transform.localPosition, range);
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
    }

}
