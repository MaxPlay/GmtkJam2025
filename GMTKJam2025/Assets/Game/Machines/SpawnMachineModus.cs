using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SpawnMachineModus : MachineModus
{
    [SerializeField, UnityEngine.Range(0,10)] private int ticksBetweenSpawn;
    [SerializeField] private List<ConveyorItemData> spawnableItems;


    private int ticksBetweenSpawnCounter;

    public override ConveyorItem ApplyItem(ConveyorItem item)
    {
        return null;
    }

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        ticksBetweenSpawnCounter++;
        if (ticksBetweenSpawnCounter > ticksBetweenSpawn)
        {
            ticksBetweenSpawnCounter = 0;
            if (currentItem != null)
            {
                newItem = null;
                return false;
            }

            newItem = Instantiate(spawnableItems[Random.Range(0, spawnableItems.Count)].Prefab.gameObject)
                .GetComponent<ConveyorItem>();
            return true;
        }

        newItem = null;
        return true;
    }
}
