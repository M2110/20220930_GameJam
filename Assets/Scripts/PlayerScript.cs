using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rb;
    private bool isMoving, isColliding;
    private Grid grid;
    private float gridSize;
    private GameObject currentTrigger;
    private Animator[] animators;

    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        input = new PlayerInput();
        grid = FindObjectOfType<Grid>();
        gridSize = grid.cellSize.x;
    }

    private void OnEnable()
    {
        input.Player.Action.performed += OnActionPerformed;
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Action.performed -= OnActionPerformed;
        input.Player.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isMoving && input.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            isMoving = true;
            StartCoroutine(Movement(input.Player.Move.ReadValue<Vector2>()));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentTrigger = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        currentTrigger = null;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        isColliding = true;
    }

    private void OnActionPerformed(InputAction.CallbackContext callback)
    {
        if (currentTrigger != null)
        {
            switch (currentTrigger.tag)
            {
                default:
                    Debug.LogWarning("Unknown trigger!");
                    break;
            }
        }
    }

    private IEnumerator Movement(Vector2 direction)
    {
        if (direction.x > 0)
        {
            SetAnimation("OnWalkRight");
        }
        else if (direction.x < 0)
        {
            SetAnimation("OnWalkLeft");
        }
        else
        {
            if (direction.y > 0)
            {
                SetAnimation("OnWalkUp");
            }
            else
            {
                SetAnimation("OnWalkDown");
            }
        }

        if (Math.Abs(direction.x) > 0 && Math.Abs(direction.y) > 0)
        {
            direction.x = (float) Math.Round(direction.x);
            direction.y = 0;
        }

        Vector2 position = rb.position;
        Vector2 target = position + new Vector2((float) Math.Round(direction.x), (float) Math.Round(direction.y)) * gridSize;
        float distance;
        float lastDistance = Vector2.Distance(rb.position, target);
        while ((distance = Vector2.Distance(rb.position, target)) > 0.05f)
        {
            if (lastDistance < distance || isColliding)
            {
                break;
            }
            rb.velocity = direction * speed;
            lastDistance = distance;
            yield return new WaitForSeconds(1/60);
        }
        rb.velocity = Vector2.zero;
        if (isColliding)
        {
            rb.position = position;
            isColliding = false;
        }
        else
        {
            rb.position = target;
        }
        isMoving = false;
        if (input.Player.Move.ReadValue<Vector2>() == Vector2.zero)
        {
            SetAnimation("OnStop");
        }
    }

    private void SetAnimation(String command)
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger(command);
            animator.speed = 0.667f * speed / gridSize;
        }
    }
}
