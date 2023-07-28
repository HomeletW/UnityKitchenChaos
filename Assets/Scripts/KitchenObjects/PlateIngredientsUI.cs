using UnityEngine;
using System.Collections;

public class PlateIngredientsUI : MonoBehaviour
{
    [SerializeField] GameObject iconTemplate;

    private PlateKitchenObject plate;

    private void Awake()
    {
        iconTemplate.SetActive(false);
        plate = GetComponentInParent<PlateKitchenObject>();

        // place in Awake because Start is not called on Instantiate
        plate.OnPlateUpdated += Plate_OnPlateUpdated;
    }

    private void Start()
    {
    }

    private void Plate_OnPlateUpdated(object sender, PlateKitchenObject.PlateUpdatedArg e)
    {
        // spawn a new icon
        GameObject icon = Instantiate(iconTemplate, transform);
        PlateIngredientsIconUI iconUI = icon.GetComponent<PlateIngredientsIconUI>();
        iconUI.SetKitchenObjectSO(e.NewIngredient);
        icon.SetActive(true);
    }
}
