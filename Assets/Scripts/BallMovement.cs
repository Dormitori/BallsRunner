using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public List<Transform> lanes;

    public float jumpingHeight = 2f;
    public float jumpingTime = 1;
    public float fallingGravity = 9.8f;
    public float fallingSpeed = 5;

    private int _curLane = 1;
    private InputAction _move;
    private bool _isMoving;
    

    private bool _isJumping;
    private bool _isFalling;
    private float _fallingHeight;
    private float _t;

    public void Start()
    {
        _move = InputSystem.actions.FindAction("move");
    }

    public void Update()
    {
        MoveLane();
        Jump();
    }

    private void Jump()
    {
        var upMove = _move.ReadValue<Vector2>()[1];
        if (Mathf.Approximately(upMove, 1f) && !_isJumping)
        {
            _isJumping = true;
            _t = 0;
        }

        if (_isJumping)
        {
            if (!_isFalling)
            {
                var newPos = transform.position;
                newPos.y = (2 * jumpingHeight / jumpingTime) * _t -
                           (jumpingHeight / (jumpingTime * jumpingTime)) * _t * _t;
                transform.position = newPos;
            }

            if (Mathf.Approximately(upMove, -1f) && !_isFalling)
            {
                _isFalling = true;
                _fallingHeight = transform.position.y;
                _t = 0;
            }

            if (_isFalling)
            {
                var newPos = transform.position;
                newPos.y = _fallingHeight - fallingSpeed * _t - fallingGravity * _t * _t / 2;
                transform.position = newPos;
            }
            _t += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _isJumping = false;
        _isFalling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isJumping = false;
        _isFalling = false;
    }
    

    private void MoveLane()
    {
        var laneMove = _move.ReadValue<Vector2>()[0];

        if (_move.WasPressedThisFrame())
        {
            if (Mathf.Approximately(laneMove, -1f) && _curLane > 0)
            {
                _curLane--;
            }

            if (Mathf.Approximately(laneMove, 1f) && _curLane < 2)
            {
                _curLane++;
            }
        }

        var moveTowardsPost = transform.position;
        moveTowardsPost.x = lanes[_curLane].position.x;
        transform.position =
            Vector3.MoveTowards(transform.position, moveTowardsPost, moveSpeed * Time.deltaTime);
    }
}