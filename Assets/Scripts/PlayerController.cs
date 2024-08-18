using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private bool groundedPlayer;
    private Vector3 playerVelocity;
    public float playerSpeed = 4f;
    public float jumpHeight = 0.6f;
    public float playerScale = 1f;
    public float scaleSpeed = 6f;
    public float minScale = 1f;
    public float maxScale = 4.2f;
    public float gravityValue = -9.81f;
    public Transform scaler;
    public SpriteRenderer shadowCaster;

    // MyPlayerControls is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    private DefaultPlayerControls controls;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Move left right
        var move = new Vector3(controls.Player.Move.ReadValue<float>(), 0, 0);
        if (move.x < 0)
            shadowCaster.flipX = true;
        else if (move.x > 0)
            shadowCaster.flipX = false;
        controller.Move(move * (Time.deltaTime * playerSpeed));

        // Visual scale
        var scale = controls.Player.Scale.ReadValue<float>();
        playerScale += scale * scaleSpeed * Time.deltaTime;
        playerScale = Mathf.Clamp(playerScale, minScale, maxScale);
        scaler.localScale = new Vector3(playerScale, playerScale, 1f);

        // Collider scale
        float center = 0.58f * playerScale;
        controller.center = new Vector3(controller.center.x, center, controller.center.z);
        controller.height = center * 2f - 0.16f;
        controller.radius = playerScale * 0.16f;
        
        // Jump
        if (controls.Player.Jump.IsPressed() && groundedPlayer)
        {
            float jumpVariation = Mathf.Sqrt(playerScale);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * jumpVariation * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
