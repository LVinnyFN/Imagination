using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{

    public GameObject[] enemies;
    private IEnumerator respawn;

    private void Start()
    {
        GameController.controller.respawnController = this;
    }

    public void Respawn(int id, Vector3 pos, Quaternion rot, int minLvl, int maxLvl)
    {
        respawn = RespawnEnemy(id, pos, rot, minLvl, maxLvl);
        StartCoroutine(respawn);
    }

    private IEnumerator RespawnEnemy(int id, Vector3 pos, Quaternion rot, int minLvl, int maxLvl)
    {
        yield return new WaitForSeconds(30);
        GameObject instance = Instantiate(enemies[id], pos, rot);
        instance.GetComponent<Enemy>().minLvl = minLvl + 1;
        instance.GetComponent<Enemy>().maxLvl = maxLvl + 1;
    }
}
