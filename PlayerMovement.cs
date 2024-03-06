using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMovement : MonoBehaviour
{
    //Movement Var -------------------------------------------------------------------------
    [SerializeField] float moveSpeed = 10f;
    private Vector2 direction;
    private PlayerInput playerInput;
    private bool facingRight = true;

    //Compents ------------------------------------------------------------------------------
    private Rigidbody2D rb;
    private Animator animator;
    public LayerMask groundLayer;

    //Physics ------------------------------------------------------------------------------
    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float linearDrag = 4f;
    [SerializeField] float groundLenght = 1.0f;
    [SerializeField] float graivty = 1f;
    [SerializeField] float fallMultiplier = 7f;
    [SerializeField] float _jumpSpeed = 8f;

    //Basic Varibles
    public bool isOnTheGorund;
    public bool isHoldingJumpButton;


    //Start functions -----------------------------------------------------------------------
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Jump.canceled += Jump;
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    //Update function ------------------------------------------------------------------------
    private void Update()
    {
        direction = playerInput.Player.Move.ReadValue<Vector2>();
        isOnTheGorund = Physics2D.Raycast(transform.position, Vector2.down, groundLenght, groundLayer);
    }
    private void LateUpdate()
    {
        moveCharater(direction.x);
        modifyPhysics();
    }


    //Physics ----------------------------------------------------------------------------------
    private void moveCharater(float horizontal)
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        rb.AddForce(new Vector2(inputVector.x, 0) * moveSpeed, ForceMode2D.Force);

        //_rb.AddForce(Vector2.right * horizontal * moveSpeed);
        if((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }
        if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        //_animator.SetFloat("something", Mathf.Abs(_rb.velocity.x));
    }
    private void modifyPhysics()
    {
        bool changeDirectopns = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if(isOnTheGorund)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changeDirectopns) { rb.drag = linearDrag; }
            else { rb.drag = 0; }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = graivty;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0)
            {
                rb.gravityScale = graivty * fallMultiplier;
            }
            else if(rb.velocity.y > 0 && !isHoldingJumpButton)
            {
                rb.gravityScale = graivty * (fallMultiplier / 2);
            }
        }
        
    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    //Jumping ------------------------------------------------------------------------------------
    private void Jump(InputAction.CallbackContext context)
    {
        
        if(context.performed && isOnTheGorund == true) 
        {
            rb.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            isHoldingJumpButton = true;
        }
        if (context.canceled)
        {
            isHoldingJumpButton = false;
        }
    }
   
    //Check If Player Is Grounded ----------------------------------------------------------------
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLenght);   
    }

    //If you jump on a enemy ---------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Mathf.Abs(collision.contacts[0].normal.y) > 0.9f && collision.gameObject.CompareTag("Enemy"))
        {
            rb.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
