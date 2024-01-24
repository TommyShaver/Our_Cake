using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement Var -------------------------------------------------------------------------
    [SerializeField] float moveSpeed = 10f;
    private Vector2 direction;
    private PlayerInput playerInput;
    private bool facingRight = true;

    //Compents ------------------------------------------------------------------------------
    private Rigidbody2D _rb;
    private Animator _animator;

    public BoxCollider2D ontheGround;

    //Physics ------------------------------------------------------------------------------
    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float linearDrag = 4f;


    //Basic Varibles
    public bool isOnTheGorund;

    //Start functions -----------------------------------------------------------------------
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    //Update function ------------------------------------------------------------------------
    private void Update()
    {
        direction = playerInput.Player.Move.ReadValue<Vector2>();
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
        _rb.AddForce(new Vector2(inputVector.x, 0) * moveSpeed, ForceMode2D.Impulse);

        //_rb.AddForce(Vector2.right * horizontal * moveSpeed);
        if((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }
        if(Mathf.Abs(_rb.velocity.x) > maxSpeed)
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * maxSpeed, _rb.velocity.y);
        }
        //_animator.SetFloat("something", Mathf.Abs(_rb.velocity.x));
    }
    private void modifyPhysics()
    {
        bool changeDirectopns = (direction.x > 0 && _rb.velocity.x < 0) || (direction.x < 0 && _rb.velocity.x > 0);
        if(Mathf.Abs(direction.x) < 0.4f || changeDirectopns) { _rb.drag = linearDrag; }
        else { _rb.drag = 0; }
    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    //Jumping ------------------------------------------------------------------------------------
    private void Jump(InputAction.CallbackContext context)
    {
        float _jumpSpeed = 12f;
        
        if(context.performed && isOnTheGorund == true) 
        {
            _rb.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            isOnTheGorund = false;
        }
    }

    //Check If Player Is Grounded ----------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Ground"))
        {
            isOnTheGorund = true;
        }
    }

}
