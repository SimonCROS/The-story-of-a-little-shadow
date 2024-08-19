using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private float lastHitTime = float.MinValue;
    public float playerSpeed = 4f;
    public float jumpHeight = 0.6f;
    public float playerScale = 1f;
    public float scaleSpeed = 6f;
    public float minScale = 1f;
    public float maxScale = 4.2f;
    public float gravityValue = -9.81f;
    public bool haveSword = false;
    public int maxHealth = 3;
    public int health;
    public int immunityDuration = 1;
    public Transform scaler;
    public SpriteRenderer shadowCaster;
    public Sprite spriteWithoutSword;
    public Sprite spriteWithSword;
    
    private bool IsGrounded { get; set; }

    // MyPlayerControls is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    private DefaultPlayerControls controls;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        health = maxHealth;
    }

    private void OnEnable()
    {
        controls ??= new DefaultPlayerControls();

        ReloadScale();
        ReloadSprite();
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
    
    private void FixedUpdate()
    {
        IsGrounded = controller.isGrounded;
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        // Move left right
        var move = new Vector3(controls.Player.Move.ReadValue<float>(), 0, 0);
        if (move.x < 0)
            shadowCaster.flipX = true;
        else if (move.x > 0)
            shadowCaster.flipX = false;
        controller.Move(move * (Time.deltaTime * playerSpeed));

        if (IsGrounded)
        {
            // Check if something above
            var securityMargin = 0.01f;
            var top = controller.transform.position + controller.center + new Vector3(0, (controller.height / 2) + securityMargin, 0);
            bool touchingCeiling = Physics.Raycast(top, Vector3.up, out RaycastHit hit, 0.1f + Mathf.Max(velocity.y, 0));
        
            // Visual scale
            var scale = controls.Player.Scale.ReadValue<float>();
            if (scale < 0 || !touchingCeiling)
            {
                playerScale += scale * scaleSpeed * Time.deltaTime;
                playerScale = Mathf.Clamp(playerScale, minScale, maxScale);
            }
        }

        ReloadScale();
        
        // Jump
        if (controls.Player.Jump.IsPressed() && IsGrounded)
        {
            float jumpVariation = Mathf.Sqrt(playerScale);
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * jumpVariation * gravityValue);
        }

        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - lastHitTime < immunityDuration)
        {
            return;
        }
        
        health -= damage;
        lastHitTime = Time.time;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void UnlockSword()
    {
        haveSword = true;
        ReloadSprite();
    }

    private void ReloadScale()
    {
        // Collider scale
        float center = 0.58f * playerScale;
        controller.center = new Vector3(controller.center.x, center, controller.center.z);
        controller.height = center * 2f - 0.16f;
        controller.radius = playerScale * 0.16f;
        scaler.localScale = new Vector3(playerScale, playerScale, 1f);
    }

    private void ReloadSprite()
    {
        shadowCaster.sprite = haveSword ? spriteWithSword : spriteWithoutSword;
    }
}
