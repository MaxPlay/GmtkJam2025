using System.Collections.Generic;
using UnityEngine;

public abstract class MachineModus : MonoBehaviour
{
    public abstract bool Tick(ConveyorItem currentItem, out ConveyorItem newItem);

}
