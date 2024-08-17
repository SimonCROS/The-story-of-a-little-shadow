using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    private bool m_Grounded;
    public Rigidbody m_CharacterRigidbody;
    public float m_Speed = 6f;
    public float m_JumpAmount = 10f;

    // MyPlayerControls is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    private DefaultPlayerControls controls;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new DefaultPlayerControls();
        }

        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        var input = controls.Player.Move.ReadValue<Vector2>();
        m_Rigidbody.MovePosition(transform.position + m_Speed * Time.deltaTime * (Vector3)input);

        if (controls.Player.Jump.IsPressed())
        {
            if (m_Grounded)
            {
                m_CharacterRigidbody.AddForce(Vector3.up * m_JumpAmount, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_Grounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            m_Grounded = false;
        }
    }

    private void CharacterGroundEnter()
    {
        m_Grounded = true;
    }

    private void CharacterGroundExit()
    {
        m_Grounded = false;
    }
}
