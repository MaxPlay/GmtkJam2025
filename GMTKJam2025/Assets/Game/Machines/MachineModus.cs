using System.Collections.Generic;
using UnityEngine;

public abstract class MachineModus : MonoBehaviour
{
    public virtual bool Tick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        newItem = null;
        return true;
    }

}
