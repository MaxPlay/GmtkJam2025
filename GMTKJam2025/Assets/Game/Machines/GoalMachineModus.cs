using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Machine Modules/Goal Machine Module")]
public class GoalMachineModus : MachineModus
{
    [SerializeField] protected List<ConveyorItemData> breakingItems;
    [SerializeField] private ConveyorItemData intendedItem;

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        if (currentItem && breakingItems.Contains(currentItem.Data))
        {
            newItem = null;
            return false;
        }

        newItem = null;

        if (currentItem && intendedItem == currentItem.Data)
        {
            //Score
        }
        else
        {
            //Take Damage
        }

        return true;
    }
}
