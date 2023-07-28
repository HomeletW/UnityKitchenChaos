using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryQueueUI : MonoBehaviour
{
    [SerializeField] private GameObject orderTemplate;

    private List<OrderUI> orders;

    private void Awake()
    {
        orders = new List<OrderUI>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderPlaced += DeliveryManager_OnOrderPlaced;
        DeliveryManager.Instance.OnOrderDelivered += DeliveryManager_OnOrderDelivered;
        orderTemplate.SetActive(false);
    }

    private void DeliveryManager_OnOrderPlaced(object sender, MenuRecipeSO recipe)
    {
        GameObject newOrder = Instantiate(orderTemplate, transform);
        OrderUI orderUI = newOrder.GetComponent<OrderUI>();
        orderUI.SetOrder(recipe);
        newOrder.SetActive(true);
        orders.Add(orderUI);
    }

    private void DeliveryManager_OnOrderDelivered(object sender, MenuRecipeSO recipe)
    {
        int index = orders.FindIndex(order => order.GetRecipe() == recipe);
        OrderUI orderUI = orders[index];
        orders.RemoveAt(index);
        Destroy(orderUI.gameObject);
    }
}
