using UnityEngine;

public class Spider : MonoBehaviour
{
	private CharacterController controller;
	private SpriteRenderer spriteRenderer;
	private Vector3 velocity;
	public float gravityValue = -9.81f;
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
}
