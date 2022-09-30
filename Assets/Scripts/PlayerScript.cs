using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rb;
    private bool isMoving;
    private Grid grid;
    private float gridSize;
    private GameObject currentTrigger;

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
    }

    private void FixedUpdate()
    {
        if (!isMoving && input.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
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
        isMoving = true;
        Vector2 target = rb.position + new Vector2((float) Math.Round(direction.x), (float) Math.Round(direction.y)) * gridSize;
        float distance;
        float lastDistance = Vector2.Distance(rb.position, target);
        while ((distance = Vector2.Distance(rb.position, target)) > 0.05f)
        {
            if (lastDistance < distance)
            {
                break;
            }
            rb.velocity = direction * speed;
            yield return new WaitForSeconds(1/60);
        }
        rb.velocity = Vector2.zero;
        rb.position = target;
        isMoving = false;
    }
}
