using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ControllerButtonClick : MonoBehaviour
{
    [SerializeField]
    private InputActionReference input;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        input.action.started += OnAction;
    }

    private void OnAction(InputAction.CallbackContext obj)
    {
        button.onClick.Invoke();
    }

    private void OnDestroy()
    {
        input.action.started -= OnAction;
    }
}
