using System;
using UnityEngine;

public class Spider : MonoBehaviour
{
	private CharacterController controller;
	private SpriteRenderer spriteRenderer;
	private Vector3 velocity;
	private float lastJumpTime = float.MinValue;
	public float gravityValue = -9.81f;
	public float jumpHeight = 2f;
	public float jumpCooldown = 5f;
	public float speadIncrementer = 8f;
	public float moveSpeed = 5f;
    public int damage;
    public PlayerController playerController;
    public Sprite groundedSprite;
    public Sprite jumpSprite;

    private bool IsGrounded { get; set; }
    
    private void Awake()
    {
	    controller = GetComponent<CharacterController>();
	    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
	    lastJumpTime = Time.fixedTime;
    }

    private void Update()
    {
	    Sprite sprite = controller.isGrounded ? groundedSprite : jumpSprite;
	    if (sprite != spriteRenderer.sprite)
	    {
		    spriteRenderer.sprite = sprite;
	    }
    }
    
    private void FixedUpdate()
    {
	    IsGrounded = controller.isGrounded;
	    if (IsGrounded && velocity.y < 0)
	    {
		    velocity.y = 0f;
	    }

	    if (IsGrounded && playerController)
	    {
		    Vector3 directionToPlayer = (playerController.transform.position - transform.position).normalized;
		    velocity.x += directionToPlayer.x * speadIncrementer * Time.deltaTime;
		    velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
	    
		    if (Time.fixedTime - lastJumpTime > jumpCooldown)
		    {
			    lastJumpTime = Time.fixedTime;
			    velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
		    }
	    }
	    
	    velocity.y += gravityValue * Time.deltaTime;
	    controller.Move(velocity * Time.deltaTime);
    }
    
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			playerController.TakeDamage(damage);
		}
	}

	public void AddDamage()
	{
		Destroy(gameObject);
	}
}
