using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Machine Modules/Goal Machine Module")]
public class GoalMachineModus : MachineModus
{
    [SerializeField] protected List<ConveyorItemData> breakingItems;
    [SerializeField] private Transform displayParent;

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;
        if (currentItem && breakingItems.Contains(currentItem.Data))
        {
            return false;
        }


        if (currentItem && !Machine.GameManager.CollectItemForWinCondition(currentItem.Data))
        {
            return false;
        }

        return true;
    }
}
