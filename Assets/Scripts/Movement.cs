using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // movement vars
    [SerializeField]
    private float moveSpeed = 10.0f;

    // jump vars
    [SerializeField]
    private float jumpForce = 15.0f;
    [SerializeField]
    private GameObject groundCheck;
    [SerializeField]
    private LayerMask groundLayers;

    // hover vars
    [SerializeField]
    private float maxHoverHeight;
    [SerializeField]
    private float hoverForce;
    [SerializeField]
    private LayerMask hoverLayers;
    [SerializeField]
    private ParticleSystem hoverParticles;

    private Rigidbody2D _rigidBody;
    private Animator animator;

    private float horizontalInput;
    private bool isGrounded;
    private bool inHoverRange;
    private bool hoverInput;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // Game is running - 30 FPS - Called 30 times / sec
    void Update()
    {
        
    }

    // This is called in fixed time intervals - ticks
    void FixedUpdate() {
        // does not follow physics - basically teleports the game object
        // transform.Translate(new Vector3(moveSpeed * horizontalInput * Time.deltaTime, 0.0f, 0.0f));

        // _rigidBody.AddForce(new Vector2(moveSpeed, 0.0f), ForceMode2D.Impulse);

        _rigidBody.velocity = new Vector2(moveSpeed * horizontalInput, _rigidBody.velocity.y);

        if(hoverInput && inHoverRange) {
            _rigidBody.AddForce(new Vector2(0.0f, hoverForce), ForceMode2D.Impulse);
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Clamp(_rigidBody.velocity.y, 0, 5.0f));

            if(_rigidBody.velocity.x > 0) {
                animator.SetBool("tiltForward", true);
                animator.SetBool("tiltBackwards", false);
            }
            else if(_rigidBody.velocity.x < 0) {
                animator.SetBool("tiltForward", false);
                animator.SetBool("tiltBackwards", true);
            }
            else {
                animator.SetBool("tiltForward", false);
                animator.SetBool("tiltBackwards", false);
            }
        }

        // check if player is grounded - colliding with groundLayer
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.01f, groundLayers);

        // check if player is in hover range
        inHoverRange = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, maxHoverHeight, hoverLayers);
    }

    public void OnMove (InputAction.CallbackContext context) {
        // context - started -> performed -> cancelled
        horizontalInput = context.ReadValue<float>();
    }

    public void OnJump (InputAction.CallbackContext context) {
        if (context.performed && isGrounded) {
            _rigidBody.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnHover (InputAction.CallbackContext context) {
        if(context.performed) {
            hoverInput = true;
            hoverParticles.Play();
        }
        if(context.canceled) {
            hoverInput = false;
            hoverParticles.Stop();
            animator.SetBool("tiltForward", false);
            animator.SetBool("tiltBackwards", false);
        }
    }
}
