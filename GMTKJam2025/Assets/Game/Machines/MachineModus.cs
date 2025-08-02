using System.Collections.Generic;
using UnityEngine;

public abstract class MachineModus : MonoBehaviour
{
    public abstract ConveyorItem ApplyItem(ConveyorItem item);

    public virtual bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;
        return true;
    }

}
