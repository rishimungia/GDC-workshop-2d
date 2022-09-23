using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1.0f;
    [SerializeField]
    private float jumpForce = 10.0f;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float hoverForce = 15.0f;
    [SerializeField]
    private float maxHoverHeight = 10.0f;
    [SerializeField]
    private LayerMask hoverCheckLayer;
    [SerializeField]
    private ParticleSystem hoverParticles;

    private Rigidbody2D _rigidBody;
    
    private float horizontalInput;

    private bool inHoverRange;

    private bool isGrounded = false;
    private bool isHovering = false;
    
    // Start is called before the first frame update
    void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        
    }
    
    // FixedUpdate is called at fixed time intervals - ticks
    // All game logic should be here
    void FixedUpdate() {
        _rigidBody.velocity = new Vector2(horizontalInput * movementSpeed, _rigidBody.velocity.y);

        if (isHovering && inHoverRange) {
            _rigidBody.AddForce(new Vector2(0.0f, hoverForce), ForceMode2D.Impulse);
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, Mathf.Clamp(_rigidBody.velocity.y, 0.0f, 5.0f));
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        inHoverRange = Physics2D.Raycast(gameObject.transform.position, Vector2.down, maxHoverHeight, hoverCheckLayer);
    }

    public void OnMove (InputAction.CallbackContext contex) {
        horizontalInput = contex.ReadValue<float>();
    }

    public void OnJump (InputAction.CallbackContext context) {
        if(context.performed && isGrounded) {
            _rigidBody.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnHover (InputAction.CallbackContext context) {
        if (context.performed) {
            isHovering = true;
            hoverParticles.Play();
        }
        if (context.canceled) {
            isHovering = false;
            hoverParticles.Stop();
        }
    }
}
