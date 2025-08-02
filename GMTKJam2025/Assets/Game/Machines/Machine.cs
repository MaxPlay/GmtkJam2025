using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[DisallowMultipleComponent]
public class Machine : MonoBehaviour
{
    [SerializeField]
    private MachineModus mainMachineModus;
    [SerializeField]
    private MachineModus secondaryMachineModus;
    private MachineModus inactiveMachineModus;
    [ReadOnly, SerializeField] private MachineModus currentModus;
    private Carryable carryable;
    private Interactable interactable;
    private Conveyor currentConveyorPiece;


    public void Initialize(GameManager gameManager)
    {
        GameManager = gameManager;
        currentModus = mainMachineModus;
        if (!secondaryMachineModus)
            secondaryMachineModus = mainMachineModus;

        inactiveMachineModus = gameObject.AddComponent<InactiveMachineModus>();
        carryable = GetComponent<Carryable>();
        if (carryable)
        {
            carryable.OnPickUp.AddListener(PickUp);
            carryable.OnDropOff.AddListener(DropOff);
        }
        interactable = GetComponent<Interactable>();
        if (interactable)
        {
            interactable.OnInteract.AddListener(Interact);
        }

        currentConveyorPiece = GameManager.GetClosestConveyor(transform.position);


        if (currentConveyorPiece)
            DropOff(currentConveyorPiece);
    }

    public void Interact(int interactionIndex)
    {
        currentModus = interactionIndex switch
        {
            0 => mainMachineModus,
            1 => secondaryMachineModus,
            2 => inactiveMachineModus
        };
    }

    public void SetBlocked(bool blocked)
    {
        if (carryable)
            carryable.SetBlocked(blocked);
    }

    public GameManager GameManager { get; private set; }

    public bool ModuleTick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        if (currentModus)
        {
            return currentModus.Tick(currentItem, out newItem);
        }

        newItem = null;
        return true;
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
        transform.position = conveyor.CenterPosition;
        transform.LookAt(conveyor.OutPosition);
        currentConveyorPiece.InstallMachine(this);
    }
}