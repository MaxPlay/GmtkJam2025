using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private bool blocked;

    public bool Blocked => blocked;


    [SerializeField] private UnityEvent<int> onInteract;
    public UnityEvent<int> OnInteract => onInteract;

    public void Interact()
    {

    }

    public void StartInteracting()
    {

    }

    public void StopInteracting()
    {

    }
}
