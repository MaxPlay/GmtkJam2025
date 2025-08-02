using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[AddComponentMenu("Machine Modules/Spawn Machine Module")]
public class SpawnMachineModus : MachineModus
{
    [SerializeField, UnityEngine.Range(0, 10)] private int ticksBetweenSpawn;
    [SerializeField] private List<ConveyorItemData> spawnableItems;


    private int ticksBetweenSpawnCounter;

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
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
                newItem = Instantiate(spawnableItems[Random.Range(0, spawnableItems.Count)].Prefab);
        }
        return true;
    }
}
