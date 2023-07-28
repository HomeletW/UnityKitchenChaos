using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private GameObject success;
    [SerializeField] private GameObject failed;
    [SerializeField] private float showTimer = 2f;

    private void Start()
    {
        DeliveryManager.Instance.OnOrderDelivered += DeliveryManager_OnOrderDelivered;
        DeliveryManager.Instance.OnFailedOrderDeliver += DeliveryManager_OnFailedOrderDeliver;
    }

    private void DeliveryManager_OnFailedOrderDeliver(object sender, System.EventArgs e)
    {
        StartCoroutine(ShowResult(failed));
    }

    private void DeliveryManager_OnOrderDelivered(object sender, MenuRecipeSO e)
    {
        StartCoroutine(ShowResult(success));
    }

    private IEnumerator ShowResult(GameObject objToShow)
    {
        GameObject obj = Instantiate(objToShow, transform);
        yield return new WaitForSeconds(showTimer);
        Destroy(obj);
    }
}
