using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLifeGain : MonoBehaviour
{
    private TextMeshProUGUI lifeGainText;
    private float timer=1.5f;
    private Vector3 moveVector;

    void Awake()
    {
        lifeGainText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetHeal(float heal)
    {
        moveVector = new Vector3(Random.Range(1,3),Random.Range(1,3),0);
        int myheal = (int)heal;
        lifeGainText.text = myheal.ToString();
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
            lifeGainText.alpha -= Time.deltaTime;
            if (lifeGainText.alpha<=0)
            {
                Destroy(gameObject);
            }
        }
        
    }

}
