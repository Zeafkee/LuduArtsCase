using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput m_PlayerInput;

    private InputAction m_MoveAction;
    private InputAction m_InteractAction;
    private InputAction m_SprintAction;
    private InputAction m_ClearDataAction;

    private void Awake()
    {
        InitializeInputs();
    }
    void InitializeInputs()
    {
        m_MoveAction = m_PlayerInput.actions["Move"];
        m_InteractAction = m_PlayerInput.actions["Interact"];
        m_SprintAction = m_PlayerInput.actions["Sprint"];
        m_ClearDataAction = m_PlayerInput.actions["ClearData"];
    }
    #region Methods
    public Vector2 GetMoveInput()
    {
        return m_MoveAction.ReadValue<Vector2>();
    }
    public bool GetInteractInput()
    {
        return m_InteractAction.triggered;
    }
    public bool GetClearDataInput()
    {
        return m_ClearDataAction != null && m_ClearDataAction.triggered;
    }
    public bool GetSprintInput()
    {
        return m_SprintAction.IsPressed();
    }

    #endregion
}
