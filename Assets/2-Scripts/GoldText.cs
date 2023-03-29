using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoldText : MonoBehaviour
{
    private TextMeshProUGUI goldEarn;
    private Image goldimage;
    private float timer = 1.5f;
    private Vector3 moveVector;

    void Awake()
    {
        goldEarn = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        goldimage = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void SetGold(int gold)
    {
        moveVector = new Vector3(Random.Range(1, 5), Random.Range(1, 5), 0);
        goldEarn.text = "+  " + gold.ToString() + " GOLD! ";

    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 0.2f * Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer > 0.5f)
        {
            transform.localScale += Vector3.one * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
        }
        if (timer <= 0)
        {
            goldEarn.alpha -= Time.deltaTime;
            goldimage.color -= new Color(0,0,0,0.2f)*Time.deltaTime;
            if (goldEarn.alpha <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
