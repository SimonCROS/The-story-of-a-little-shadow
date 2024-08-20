using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private float lastHitTime = float.MinValue;
    private float lastAttackTime = float.MinValue;
    private bool isVisuallyAttacking = false;
    private bool haveSwordDirty = false;
    public float playerSpeed = 4f;
    public float jumpHeight = 0.6f;
    public float playerScale = 1f;
    public float scaleSpeed = 6f;
    public float minScale = 1f;
    public float maxScale = 4.2f;
    public float gravityValue = -9.81f;
    public float attackDuration = 0.2f;
    public float attackDelay = 0.3f;
    public float attackRadius = 0.3f;
    public bool haveSword = false;
    public int maxHealth = 3;
    public int health;
    public int immunityDuration = 1;
    public LayerMask monsterOverlapMask;
    public Transform scaler;
    public SpriteRenderer shadowCaster;
    public SpriteRenderer attackRenderer;
    public Sprite spriteWithoutSword;
    public Sprite spriteWithSword;

    [SerializeField]
    public AudioSource playerHurt;

	[SerializeField]
	public AudioSource spiderHurt;

	[SerializeField]
	public AudioSource swordAttack;

	private bool IsGrounded { get; set; }
    private bool IsAttacking => Time.time - lastAttackTime < attackDuration;
    private bool CanAttack => haveSword && Time.time - lastAttackTime > attackDelay;

    // MyPlayerControls is the C# class that Unity generated.
    // It encapsulates the data from the .inputactions asset we created
    // and automatically looks up all the maps and actions for us.
    private DefaultPlayerControls controls;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        health = maxHealth;
    }

    private void OnValidate()
    {
        haveSwordDirty = true;
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

    private void OnDrawGizmos()
    {
        if (IsAttacking)
        {
            Gizmos.DrawSphere(attackRenderer.transform.position, attackRadius * playerScale);
        }
    }

    private void Update()
    {
        if (haveSwordDirty)
        {
            ReloadSprite();
            haveSwordDirty = false;
        }
        
        // Attack
        if (CanAttack && controls.Player.Attack.IsPressed())
        {
            lastAttackTime = Time.time;
			swordAttack.Play();
		}

        if (IsAttacking)
        {
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            var numColliders = Physics.OverlapSphereNonAlloc(attackRenderer.transform.position, attackRadius * playerScale, hitColliders, monsterOverlapMask);
            for (int i = 0; i < numColliders; i++)
            {
                hitColliders[i].SendMessage("AddDamage");
                spiderHurt.Play();
            }
        }

        if (IsAttacking != isVisuallyAttacking)
        {
            ReloadSprite();
            isVisuallyAttacking = IsAttacking;
        }
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
        UpdateVisualDirection(move);
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
        playerHurt.Play();

		health -= damage;
        lastHitTime = Time.time;
        if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void UnlockSword()
    {
        haveSword = true;
        ReloadSprite();
    }

    private void UpdateVisualDirection(Vector3 direction)
    {
        Vector3 rendererPosition = attackRenderer.transform.localPosition;
        
        if (direction.x < 0)
        {
            rendererPosition.x = -Mathf.Abs(rendererPosition.x);
            attackRenderer.flipX = true;
            shadowCaster.flipX = true;
        }
        else if (direction.x > 0)
        {
            rendererPosition.x = Mathf.Abs(rendererPosition.x);
            attackRenderer.flipX = false;
            shadowCaster.flipX = false;
        }

        attackRenderer.transform.localPosition = rendererPosition;
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
        attackRenderer.gameObject.SetActive(IsAttacking);
    }
}
