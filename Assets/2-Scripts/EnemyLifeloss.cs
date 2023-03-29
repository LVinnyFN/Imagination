using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyLifeloss : MonoBehaviour
{
    private TextMeshProUGUI lifeLossText;
    private float timer=1.5f;
    private Vector3 moveVector;

    void Awake()
    {
        lifeLossText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetDmg(int damage, int iscritical)
    {
        moveVector = new Vector3(Random.Range(1,3),Random.Range(1,3), Random.Range(1, 3));
        lifeLossText.text = damage.ToString();
        if (iscritical >= 1)
        {
            lifeLossText.fontSize *= 1.5f;
            lifeLossText.color = new Color(1,0.2f,0);
        }
    }

    public void SetPoison(int damage)
    {
        moveVector = new Vector3(Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3));
        lifeLossText.text = damage.ToString();
        lifeLossText.color = Color.green;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector*0.2f * Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer>0.5f)
        {
            transform.localScale += Vector3.one*Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
        }
        if (timer<=0)
        {
            lifeLossText.alpha -= Time.deltaTime;
            if (lifeLossText.alpha<=0)
            {
                Destroy(gameObject);
            }
        }
        
    }

}
