using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoverBees : MonoBehaviour
{
    private Vector3 _startingPostion;

    [SerializeField] private float _moveDistance;

    [SerializeField] private bool _xMoving;
    [SerializeField] private bool _yMoving;
    [SerializeField] private bool _zMoving;

    [SerializeField] private AnimationCurve _speedCurve;
    private float _curveValue;
    private void Start()
    {
        _startingPostion = transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _curveValue = _speedCurve.Evaluate(Time.time);
        if (_xMoving == true) { XAxisMovement(); }
        if (_yMoving == true) { YAxisMovement(); }
        if (_zMoving == true) { ZAxisMovement(); }
    }

    private void XAxisMovement()
    {
        //This function will effect the X Axis
        float _loopXAxis = Mathf.PingPong(_curveValue, 1) * _moveDistance + _startingPostion.x;
        transform.position = new Vector3(_loopXAxis, transform.position.y, transform.position.z);
    }
    private void YAxisMovement()
    {
        //This function will effect the Y Axis
        float _loopYAxis = Mathf.PingPong(_curveValue, 1) * _moveDistance + _startingPostion.y;
        transform.position = new Vector3(transform.position.x, _loopYAxis, transform.position.z);
    }
    private void ZAxisMovement()
    {
        //This function will effect the Z Axis
        float _loopZAxis = Mathf.PingPong(_curveValue, 1) * _moveDistance + _startingPostion.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, _loopZAxis);
    }
}
