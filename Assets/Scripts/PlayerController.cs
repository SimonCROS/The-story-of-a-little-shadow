using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private bool grounded;
    public float speed = 4f;
    public float jumpAmount = 1.5f;
    public float scaleSpeed = 6f;
    public float minScale = 1f;
    public float maxScale = 4.2f;
    public Transform scaler;
    public SpriteRenderer shadowCaster;

    // MyPlayerControls is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    private DefaultPlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
            shadowCaster.flipX = true;
        else if (move > 0)
            shadowCaster.flipX = false;

        rb.MovePosition(transform.position + speed * Time.deltaTime * new Vector3(move, 0f, 0f));
        
        var scale = controls.Player.Scale.ReadValue<float>();
        var newScale = scaler.localScale.x + scale * scaleSpeed * Time.deltaTime;
        scaler.localScale = Mathf.Clamp(newScale, minScale, maxScale) * Vector3.one;

        if (controls.Player.Jump.IsPressed())
        {
            if (grounded)
            {
                var jumpCoeff = Mathf.Sqrt(newScale);
                rb.AddForce(Vector3.up * (jumpAmount * jumpCoeff), ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
