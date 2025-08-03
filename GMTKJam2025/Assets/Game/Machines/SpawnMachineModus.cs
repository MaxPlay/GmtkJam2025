using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[AddComponentMenu("Machine Modules/Spawn Machine Module")]
public class SpawnMachineModus : MachineModus
{
    [SerializeField] protected List<ConveyorItemData> breakingItems;
    [SerializeField, UnityEngine.Range(0, 10)] private int ticksBetweenSpawn;
    [SerializeField] private List<ConveyorItemData> spawnableItems;

    [SerializeField] private Transform displayParent;
    [SerializeField] private float displayScale;

    private ConveyorItem currentDisplay;

    private int nextSpawningItem = 0;


    private int ticksBetweenSpawnCounter;

    void Start()
    {
        SetDisplay(spawnableItems[0]);
    }

    private void SetDisplay(ConveyorItemData data)
    {
        if(currentDisplay)
            Destroy(currentDisplay.gameObject);
        currentDisplay = Instantiate(data.Prefab);
        currentDisplay.transform.localScale = Vector3.one * displayScale;
        currentDisplay.transform.position = displayParent.position;
        currentDisplay.transform.SetParent(displayParent);
    }

    private void LateUpdate()
    {
        if (currentDisplay)
        {
            currentDisplay.transform.rotation = Quaternion.identity;
        }
    }

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;
        if (currentItem &&  breakingItems.Contains(currentItem.Data))
        {
            return false;
        }
        ticksBetweenSpawnCounter++;
        if (ticksBetweenSpawnCounter > ticksBetweenSpawn)
        {
            ticksBetweenSpawnCounter = 0;
            if (currentItem != null)
            {
                return false;
            }

            if (spawnableItems.Count > 0)
            {
                newItem = Instantiate(spawnableItems[nextSpawningItem].Prefab);
                nextSpawningItem = (nextSpawningItem + 1) % spawnableItems.Count;
                SetDisplay(spawnableItems[nextSpawningItem]);
            }
        }
        return true;
    }
}
