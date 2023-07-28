using UnityEngine;
using System.Collections;

public class StaticDataReset : MonoBehaviour
{
    void Start()
    {
        // TODO : please don't use static data, this is extreamly
        // anti-pattern and bad practise
        ContainerCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();
        HolderCounter.ResetStaticData();
        PlateKitchenObject.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
