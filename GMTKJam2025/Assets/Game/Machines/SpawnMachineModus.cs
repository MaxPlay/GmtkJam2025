using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[AddComponentMenu("Machine Modules/Spawn Machine Module")]
public class SpawnMachineModus : MachineModus
{
    [SerializeField] protected List<ConveyorItemData> breakingItems;
    [SerializeField, UnityEngine.Range(0, 10)] private int ticksBetweenSpawn;
    [SerializeField] private List<ConveyorItemData> spawnableItems;

    private int nextSpawningItem = 0;


    private int ticksBetweenSpawnCounter;

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        if(currentItem &&  breakingItems.Contains(currentItem.Data))
        {
            newItem = null;
            return false;
        }
        newItem = null;
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
            }
        }
        return true;
    }
}
