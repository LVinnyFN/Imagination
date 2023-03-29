using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public CraftGrid craftScript;
    public Recipes recipe;

    void Start()
    {
        GetComponent<Image>().sprite=recipe.icon; // atualizo meu sprite de acordo com o icone da minha receita
    }

    public void SelectSlot()
    {
        craftScript.SelectRecipeSlot(this);
    }
}
