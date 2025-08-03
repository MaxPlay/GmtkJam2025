using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MachineModus : MonoBehaviour
{
    [SerializeField] protected UnityEvent modusEntered = new();

    [SerializeField] protected UnityEvent modusExited = new();

    public UnityEvent ModusExited => modusExited;

    public UnityEvent ModusEntered => modusEntered;

    public Machine Machine { get; private set; }

    public abstract bool Tick(ConveyorItem currentItem, out ConveyorItem newItem);

    public void SetMachine(Machine machine)
    {
        Machine = machine;
    }
}
