using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private UnityEvent onBreak;
    [SerializeField] private UnityEvent onRepair;
    [SerializeField] private List<GameObject> blinkingLights;

    private bool initiallyPositioned;

    public void Initialize(GameManager gameManager)
    {
        GameManager = gameManager;
        currentModus = mainMachineModus;
        mainMachineModus.SetMachine(this);
        if (!secondaryMachineModus)
            secondaryMachineModus = mainMachineModus;
        else
            secondaryMachineModus.SetMachine(this);

        inactiveMachineModus = gameObject.AddComponent<InactiveMachineModus>();
        inactiveMachineModus.SetMachine(this);

        inactiveMachineModus.ModusEntered.AddListener((() => EnableLights(true)));
        inactiveMachineModus.ModusExited.AddListener((() => EnableLights(false)));

        inactiveMachineModus.ModusExited.Invoke();
        if (secondaryMachineModus != mainMachineModus)
            secondaryMachineModus.ModusExited.Invoke();
        mainMachineModus.ModusEntered.Invoke();

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

    private void EnableLights(bool enable)
    {
        foreach (GameObject blinkingLight in blinkingLights)
        {
            blinkingLight.SetActive(enable);
        }
    }

    public void Interact(int interactionIndex)
    {
        if (GameManager.TryFixMachine(this))
        {
            onRepair.Invoke();
            return;
        }

        MachineModus newModus = interactionIndex switch
        {
            0 => mainMachineModus,
            1 => secondaryMachineModus,
            _ => inactiveMachineModus
        };

        if (newModus == currentModus)
            return;

        currentModus.ModusExited.Invoke();
        currentModus = newModus;
        currentModus.ModusEntered.Invoke();
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
            bool working = currentModus.Tick(currentItem, out newItem);
            if (!working)
            {
                Interact(2);
                GameManager.MachineBroken(this);
                onBreak.Invoke();
            }
            return working;
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

        transform.SetParent(conveyor.transform);
        transform.DOKill();
        if (!initiallyPositioned)
        {
            initiallyPositioned = true;
            transform.DOLocalMove(Vector3.zero, 0).SetEase(Ease.OutQuad);
            transform.DORotateQuaternion(Quaternion.LookRotation(conveyor.OutPosition - conveyor.CenterPosition, Vector3.up), 0).SetEase(Ease.OutQuad);
        }
        else
        {
            transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutQuad);
            transform.DORotateQuaternion(Quaternion.LookRotation(conveyor.OutPosition - conveyor.CenterPosition, Vector3.up), 0.2f).SetEase(Ease.OutQuad);
        }
        
        currentConveyorPiece.InstallMachine(this);
    }
}