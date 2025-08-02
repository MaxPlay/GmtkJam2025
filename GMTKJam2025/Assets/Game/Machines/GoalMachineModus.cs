using UnityEngine;

[AddComponentMenu("Machine Modules/Goal Machine Module")]
public class GoalMachineModus : MachineModus
{
    [SerializeField] private ConveyorItemData intendedItem;

    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;

        if (intendedItem == currentItem.Data)
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
