using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private bool isTopDown = false;

    [Header("Configuración de Salto")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool facingRight = true;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // ✅ NUEVO: Cargar posición de spawn si existe
        if (PlayerPrefs.HasKey("SpawnX"))
        {
            float spawnX = PlayerPrefs.GetFloat("SpawnX");
            float spawnY = PlayerPrefs.GetFloat("SpawnY");
            transform.position = new Vector3(spawnX, spawnY, transform.position.z);
            
            // Limpiar los datos
            PlayerPrefs.DeleteKey("SpawnX");
            PlayerPrefs.DeleteKey("SpawnY");
        }
    }

    private void Update()
    {
        if (!canMove)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = isTopDown ? Input.GetAxisRaw("Vertical") : 0f;
        moveInput = new Vector2(horizontal, vertical).normalized;

        if (horizontal > 0 && !facingRight) Flip();
        else if (horizontal < 0 && facingRight) Flip();

        float currentSpeed = moveInput.sqrMagnitude;
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }

        if (!isTopDown && isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            if (animator != null)
            {
                animator.SetBool("IsJumping", true);
            }
        }

        if (isGrounded && animator != null)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isTopDown)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}