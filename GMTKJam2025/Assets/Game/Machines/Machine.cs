using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] private List<MachineModus> machineModi;
    [SerializeField] private int currentModus;
    [SerializeField] private Carryable carryable;
    [SerializeField] private Conveyor currentConveyorPiece;

    [Button("Assign Machine Modi")]
    private void AssignMachineModi()
    {
        machineModi = GetComponents<MachineModus>().ToList();
    }


    public void Initialize(GameManager gameManager)
    {
        GameManager = gameManager;
        if (carryable)
        {
            carryable.OnPickUp.AddListener(PickUp);
            carryable.OnDropOff.AddListener(DropOff);
        }
        if(currentConveyorPiece)
            currentConveyorPiece.InstallMachine(this);
    }

    public void SetBlocked(bool blocked)
    {
        if (carryable)
            carryable.SetBlocked(blocked);
    }

    public GameManager GameManager { get; private set; }

    public virtual ConveyorItem ApplyToItem(ConveyorItem item)
    {
        if (machineModi.IsValidIndex(currentModus))
        {
            return machineModi[currentModus].ApplyItem(item);
        }

        return null;
    }

    public void Tick()
    {

    }

    private void PickUp()
    {
        currentConveyorPiece.InstallMachine(null);
    }

    private void DropOff(Conveyor conveyor)
    {
        currentConveyorPiece = conveyor;
        currentConveyorPiece.InstallMachine(this);
    }
}