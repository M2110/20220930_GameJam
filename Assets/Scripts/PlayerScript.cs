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
    private float gridSize = 1;
    private GameObject currentTrigger;
    private Animator[] animators;

    [SerializeField] private float speed = 5f;
    
    public static event EventHandler<Door> DoorEntered;
    public static event EventHandler<UIText> DisplayUIText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Player.Action.performed += OnActionPerformed;
        SceneManager.SceneLoaded += OnSceneLoaded;
        SceneManager.TurnPlayer += OnTurnPlayer;
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Action.performed -= OnActionPerformed;
        SceneManager.SceneLoaded -= OnSceneLoaded;
        SceneManager.TurnPlayer -= OnTurnPlayer;
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
        if (other.CompareTag("Outside"))
        {
            //Check for level completion
        }
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
    
    private void OnSceneLoaded(object sender, EventArgs e)
    {
        grid = FindObjectOfType<Grid>();
        if (grid is not null)
        {
            gridSize = grid.cellSize.x;
        }
    }
    
    private void OnTurnPlayer(object sender, SceneManager.PlayerDirection direction)
    {
        //SetAnimation("OnStop");
        switch (direction.GetDirection())
        {
            case 0:
                SetAnimation("OnTurnUp");
                break;
            case 1:
                SetAnimation("OnTurnRight");
                break;
            case 2:
                SetAnimation("OnTurnDown");
                break;
            case 3:
                SetAnimation("OnTurnLeft");
                break;
            default:
                Debug.LogWarning("Unknown direction!");
                break;
        }
    }

    private void OnActionPerformed(InputAction.CallbackContext callback)
    {
        if (currentTrigger != null)
        {
            switch (currentTrigger.tag)
            {
                case "Sign":
                    OnReadSign(currentTrigger.name);
                    break;
                case "Door":
                    DoorEntered.Invoke(this, new Door(currentTrigger.name));
                    break;
                default:
                    Debug.LogWarning("Unknown trigger!");
                    break;
            }
        }
    }
    
    private void OnReadSign(String sign)
    {
        switch (sign)
        {
            case "Sign_Level1_Tavern":
                DisplayUIText.Invoke(this, new UIText("To the tavern.", 3));
                break;
            default:
                Debug.LogWarning("Unknown sign!");
                break;
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
            yield return new WaitForSeconds(1f / 60);
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
    
    public class Door : EventArgs
    {
        private string doorName;
        
        public Door(string doorName)
        {
            this.doorName = doorName;
        }
        public string GetDoorName()
        {
            return doorName;
        }
    }
    
    public class UIText : EventArgs
    {
        private string displayText;
        private int duration;

        public UIText(string displayText, int duration)
        {
            this.displayText = displayText;
            this.duration = duration;
        }
        public string GetDisplayText()
        {
            return displayText;
        }

        public int GetDuration()
        {
            return duration;
        }
    }
}
