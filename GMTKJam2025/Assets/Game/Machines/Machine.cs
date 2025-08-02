using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[DisallowMultipleComponent]
public class Machine : MonoBehaviour
{
    private List<MachineModus> machineModi = new List<MachineModus>();
    [SerializeField] private int currentModus;
    private Carryable carryable;
    private Interactable interactable;
    private Conveyor currentConveyorPiece;


    public void Initialize(GameManager gameManager)
    {
        GameManager = gameManager;
        GetComponents(machineModi);
        carryable = GetComponent<Carryable>();
        if (carryable)
        {
            carryable.OnPickUp.AddListener(PickUp);
            carryable.OnDropOff.AddListener(DropOff);
        }
        interactable = GetComponent<Interactable>();
        if (interactable)
        {
            interactable.OnInteract.AddListener(Interacted);
        }

        currentConveyorPiece = GameManager.GetClosestConveyor(transform.position);


        if (currentConveyorPiece)
            DropOff(currentConveyorPiece);
    }

    private void Interacted(int interactionIndex)
    {

    }

    public void SetBlocked(bool blocked)
    {
        if (carryable)
            carryable.SetBlocked(blocked);
    }

    public GameManager GameManager { get; private set; }

    public bool ModuleTick(ConveyorItem currentItem, out ConveyorItem newItem)
    {
        if (machineModi.IsValidIndex(currentModus))
        {
            return machineModi[currentModus].Tick(currentItem, out newItem);
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