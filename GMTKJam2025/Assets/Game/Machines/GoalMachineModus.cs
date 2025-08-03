using UnityEngine;

[AddComponentMenu("Machine Modules/Goal Machine Module")]
public class GoalMachineModus : MachineModus
{
    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;

        if (!Machine.GameManager.CollectItemForWinCondition(currentItem.Data))
        {
            return false;
        }

        return true;
    }
}
