using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject platesPrefab;
    [SerializeField] private float platesHeight = 0.1f;

    private PlatesCounter platesCounter;

    private readonly object platesStackLock = new object();
    private Stack<GameObject> platesStack;

    private void Awake()
    {
        platesCounter = GetComponentInParent<PlatesCounter>();
        platesStack = new Stack<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlatesCountUpdated += PlatesCounter_OnPlatesCountUpdated;
    }

    private void PlatesCounter_OnPlatesCountUpdated(object sender, int newCount)
    {
        lock (platesStackLock)
        {
            int offset = newCount - platesStack.Count;
            if (offset > 0)
            {
                SpawnPlates(offset);
            }
            else
            {
                RemovePlates(-offset);
            }
        }
    }

    private void SpawnPlates(int numToSpawn)
    {
        while (numToSpawn != 0)
        {
            GameObject obj = Instantiate(platesPrefab, platesCounter.GetSpawnPoint());
            obj.transform.localPosition = new Vector3(0, platesHeight * platesStack.Count, 0);
            platesStack.Push(obj);
            numToSpawn--;
        }
    }

    private void RemovePlates(int numToRemove)
    {
        Debug.Assert(numToRemove <= platesStack.Count);
        while (numToRemove != 0)
        {
            GameObject obj = platesStack.Pop();
            Destroy(obj);
            numToRemove--;
        }
    }
}
