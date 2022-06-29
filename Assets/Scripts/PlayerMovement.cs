using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    private Vector2 _moveInput;
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;
    private CapsuleCollider2D _myBodyCollider;
    private BoxCollider2D _myFeetCollider;

    private bool _isAlive = true;

    private float _defaultGravityScale;

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _myBodyCollider = GetComponent<CapsuleCollider2D>();
        _myFeetCollider = GetComponent<BoxCollider2D>();

        _defaultGravityScale = _myRigidbody.gravityScale;
    }

    private void Update()
    {
        if (!_isAlive) return;

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void OnMove(InputValue value)
    {
        if (!_isAlive) return;

        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!_isAlive) return;
        if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;

        if (value.isPressed)
        {
            // do stuff
            _myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        FireBullet();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveInput.x * runSpeed, _myRigidbody.velocity.y);
        _myRigidbody.velocity = playerVelocity;

        bool isRunning = Mathf.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;
        _myAnimator.SetBool("isRunning", isRunning);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_myRigidbody.velocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        if (!_myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            _myRigidbody.gravityScale = _defaultGravityScale;

            _myAnimator.SetBool("isClimbing", false);

            return;
        }

        Vector2 climbVelocity = new Vector2(_myRigidbody.velocity.x, _moveInput.y * climbSpeed);
        _myRigidbody.velocity = climbVelocity;

        _myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(_myRigidbody.velocity.y) > Mathf.Epsilon;
        _myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void Die()
    {
        if (_myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            _isAlive = false;
            _myAnimator.SetTrigger("Dying");
            _myRigidbody.velocity = deathKick;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void FireBullet()
    {
        if (!_isAlive) return;

        Instantiate(bullet, gun.position, transform.rotation);
    }
}
