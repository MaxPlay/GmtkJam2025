using UnityEngine;

[AddComponentMenu("Machine Modules/Inactive Machine Module")]
public class InactiveMachineModus : MachineModus
{
    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = currentItem;
        return true;
    }
}
