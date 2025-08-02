using UnityEngine;

/// <summary>
/// Component for Objects that can Interact with Interactables
/// </summary>
public class Interacting : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    private void Update()
    {
        //Go Through Interactables and Check to Show Tooltips
    }

    public bool TryInteract()
    {
        return false;
    }
}
