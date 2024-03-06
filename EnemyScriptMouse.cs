using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyScriptMouse : MonoBehaviour
{
    [SerializeField] private bool _aiWalkingleft;

    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveDistance;
    [SerializeField] private float _jumpSpeedEnemy = 5f;

    private Vector3 _startPostion;
    private Vector3 _completedCyclePos;
    private Vector3 _completedCyclesNeg;
    private Vector3 _rotation;
    private bool _facingRight = true;
    private bool _enemyIsDead = false;

    private BoxCollider2D _boxColliderEnemy;
    private Rigidbody2D   _rigidbodyEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _startPostion = gameObject.transform.position;
        _rotation = transform.rotation.eulerAngles;
        _boxColliderEnemy = GetComponent<BoxCollider2D>();
        _rigidbodyEnemy = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(!_enemyIsDead)
        {
            AIMoveMovement();
        }
    }
    //AI Movement ===================================================================================================
    private void AIMoveMovement()
    {
        if (!_aiWalkingleft)
        {
            //Moves in one direction ---------------------------------------------------------------------------------
            transform.Translate(Vector3.left * Time.deltaTime * _moveSpeed);
        }
        else
        { 
            //Moves in Circle ----------------------------------------------------------------------------------------
            transform.position = _startPostion + -_moveDirection * (_moveDistance * Mathf.Sin(Time.time * _moveSpeed));
            _completedCyclePos = _startPostion + _moveDirection * (_moveDistance);
            _completedCyclesNeg = _startPostion + -_moveDirection * (_moveDistance);
            if (Vector3.Distance(transform.position, _completedCyclePos) < 0.001f && !_facingRight)
            {
                _facingRight = true;
            }
            else if (Vector3.Distance(transform.position, _completedCyclesNeg) < 0.001f && _facingRight)
            {
                _facingRight = false;
            }

            // Set the rotation based on _facingRight
            transform.rotation = Quaternion.Euler(0, _facingRight ? 0 : 180, 0);
        }
    }

    //Collision ======================================================================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player hits enemy -------------------------------------------------------------------------------------------
        if (Mathf.Abs(collision.contacts[0].normal.y) > 0.9f && collision.gameObject.CompareTag("Player"))
        {
            _rigidbodyEnemy.AddForce(Vector2.up * _jumpSpeedEnemy, ForceMode2D.Impulse);
            _boxColliderEnemy.enabled = false;
            _enemyIsDead = true;
            _rotation.x += -180;
            transform.localRotation = Quaternion.Euler(_rotation);

        }
        //Enemy hits player -------------------------------------------------------------------------------------------
        if (Mathf.Abs(collision.contacts[0].normal.x) > 0.9f && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hurt: " + collision.collider.name);
        }
    }
}
