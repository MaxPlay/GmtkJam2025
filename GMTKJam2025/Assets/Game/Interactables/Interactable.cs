using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private bool blocked;

    public bool Blocked => blocked;


    [SerializeField] private UnityEvent<int> onInteract;
    public UnityEvent<int> OnInteract => onInteract;

    public void Interact(int interactionIndex)
    {
        OnInteract.Invoke(interactionIndex);
    }

    public void StartInteracting()
    {

    }

    public void StopInteracting()
    {

    }
}
