using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MachineModus : MonoBehaviour
{
    public Machine Machine { get; private set; }

    public abstract bool Tick(ConveyorItem currentItem, out ConveyorItem newItem);

    public void SetMachine(Machine machine)
    {
        Machine = machine;
    }
}
