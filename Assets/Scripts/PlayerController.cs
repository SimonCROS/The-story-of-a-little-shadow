using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour, DefaultPlayerControls.IPlayerActions
{
    Rigidbody m_Rigidbody;
    public float m_Speed = 6f;

    // MyPlayerControls is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    private DefaultPlayerControls controls;

    private Vector2 m_Input;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new DefaultPlayerControls();
            // Tell the "gameplay" action map that we want to get told about
            // when actions get triggered.
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        m_Rigidbody.MovePosition(transform.position + m_Speed * Time.deltaTime * (Vector3)m_Input);
    }

    void DefaultPlayerControls.IPlayerActions.OnMove(InputAction.CallbackContext context)
    {
        m_Input = controls.Player.Move.ReadValue<Vector2>();
    }

    void DefaultPlayerControls.IPlayerActions.OnLook(InputAction.CallbackContext context)
    {
    }

    void DefaultPlayerControls.IPlayerActions.OnAttack(InputAction.CallbackContext context)
    {
    }

    void DefaultPlayerControls.IPlayerActions.OnInteract(InputAction.CallbackContext context)
    {
    }

    void DefaultPlayerControls.IPlayerActions.OnCrouch(InputAction.CallbackContext context)
    {
    }

    void DefaultPlayerControls.IPlayerActions.OnJump(InputAction.CallbackContext context)
    {
    }

    void DefaultPlayerControls.IPlayerActions.OnPrevious(InputAction.CallbackContext context)
    {
    }

    void DefaultPlayerControls.IPlayerActions.OnNext(InputAction.CallbackContext context)
    {
    }

    void DefaultPlayerControls.IPlayerActions.OnSprint(InputAction.CallbackContext context)
    {
    }
}
