using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Transform ingredientList;
    [SerializeField] private GameObject ingredientItemTemplate;

    private MenuRecipeSO recipe;

    private void Awake()
    {
        ingredientItemTemplate.SetActive(false);
    }

    public void SetOrder(MenuRecipeSO recipe)
    {
        titleText.text = recipe.recipeName;
        foreach (KitchenObjectSO ingr in recipe.ingredients)
        {
            GameObject ingrObj = Instantiate(ingredientItemTemplate, ingredientList);
            Image ingrIcon = ingrObj.GetComponent<Image>();
            ingrIcon.sprite = ingr.icon;
            ingrObj.SetActive(true);
        }
        this.recipe = recipe;
    }

    public MenuRecipeSO GetRecipe()
    {
        return recipe;
    }
}
