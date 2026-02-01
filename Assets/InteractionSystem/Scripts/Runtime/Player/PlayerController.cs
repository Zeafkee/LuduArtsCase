using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] private float m_Gravity = -9.81f;

    [Header("Ground Check")]
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private float m_GroundDistance;
    [SerializeField] private LayerMask m_GroundMask;

    [Header("References")]
    [SerializeField] private CharacterController m_Controller;
    [SerializeField] private InputManager m_InputManager;
    [SerializeField] private CinemachinePanTilt m_PanTilt;

    private Vector3 m_Velocity;
    [SerializeField] private bool m_IsGrounded;
    [SerializeField] private bool m_IsSprinting;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Move();
        
        if (m_InputManager.GetClearDataInput())
        {
            if (LuduInteraction.Runtime.Core.SaveManager.Instance != null)
            {
                LuduInteraction.Runtime.Core.SaveManager.Instance.ClearSave();
            }
        }
    }
    private void LateUpdate()
    {
        Move_Mouse();
    }

    #region Functions
    void Move()
    {
        m_IsGrounded = Physics.CheckSphere(m_GroundCheck.position, m_GroundDistance, m_GroundMask);

        Vector2 _input = m_InputManager.GetMoveInput();
        m_IsSprinting = m_InputManager.GetSprintInput();

        Vector3 _move = transform.right * _input.x + transform.forward * _input.y;

        if (m_IsGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = -2f;
        }
        else
        {
            m_Velocity.y += m_Gravity * Time.deltaTime;
        }

        float _moveSpeed = m_IsSprinting ? m_RunSpeed : m_WalkSpeed;

        Vector3 finalVelocity = _move * _moveSpeed;
        finalVelocity.y = m_Velocity.y;

        m_Controller.Move(finalVelocity * Time.deltaTime);
    }

    void Move_Mouse()
    {
        transform.eulerAngles = Vector3.up * m_PanTilt.PanAxis.Value;
    }
    #endregion
}