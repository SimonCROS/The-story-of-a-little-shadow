using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    private bool m_Grounded;
    public float m_Speed = 4f;
    public float m_JumpAmount = 1.5f;
    public float m_ScaleSpeed = 6f;
    public float m_MinScale = 1f;
    public float m_MaxScale = 4.2f;
    public Transform m_Scaler;
    public SpriteRenderer m_ShadowCaster;

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
        var move = controls.Player.Move.ReadValue<float>();
        if (move < 0)
            m_ShadowCaster.flipX = true;
        else if (move > 0)
            m_ShadowCaster.flipX = false;

        m_Rigidbody.MovePosition(transform.position + m_Speed * Time.deltaTime * new Vector3(move, 0f, 0f));
        
        var scale = controls.Player.Scale.ReadValue<float>();
        var newScale = m_Scaler.localScale.x + scale * m_ScaleSpeed * Time.deltaTime;
        m_Scaler.localScale = Mathf.Clamp(newScale, m_MinScale, m_MaxScale) * Vector3.one;

        if (controls.Player.Jump.IsPressed())
        {
            if (m_Grounded)
            {
                var jumpCoeff = Mathf.Sqrt(newScale);
                m_Rigidbody.AddForce(Vector3.up * (m_JumpAmount * jumpCoeff), ForceMode.Impulse);
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
}
