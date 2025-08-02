using UnityEngine;

public class InactiveMachineModus : MachineModus
{
    public override bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = currentItem;
        return true;
    }
}
