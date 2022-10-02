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
    private bool isMovementLimited;

    [SerializeField] private float speed = 5f;
    
    public static event EventHandler<Door> DoorEntered;
    public static event EventHandler<UIText> DisplayUIText;
    public static event EventHandler<UIObjectInventory> DisplayObjectInventory;

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
        UIManager.MovementLimitation += OnLimitMovement;
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
        if (!isMoving && input.Player.Move.ReadValue<Vector2>() != Vector2.zero && !isMovementLimited)
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
    
    private void OnLimitMovement(object sender, UIManager.Move movement)
    {
        isMovementLimited = movement.GetMovementLimitation();
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
                case "Npc":
                    OnTalkToNPC(currentTrigger.name);
                    break;
                case "Door":
                    DoorEntered.Invoke(this, new Door(currentTrigger.name));
                    break;
                
                case "Chest":
                    DisplayObjectInventory.Invoke(this, new UIObjectInventory("test name",currentTrigger.GetComponentInChildren<Sprite>()));
                    break;    
                
                default:
                    Debug.LogWarning("Unknown trigger!");
                    break;
            }
        }
    }
    
    private void OnTalkToNPC(String npc)
    {
        switch (npc)
        {
            case "NPC_Level1_Tavern":
                DisplayUIText.Invoke(this, new UIText("Welcome to the tavern.", 5, true));
                break;
            default:
                Debug.LogWarning("Unknown npc!");
                break;
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
        private bool isMovementLimited;

        public UIText(string displayText, int duration, bool isMovementLimited = false)
        {
            this.displayText = displayText;
            this.duration = duration;
            this.isMovementLimited = isMovementLimited;
        }
        public string GetDisplayText()
        {
            return displayText;
        }

        public int GetDuration()
        {
            return duration;
        }

        public bool GetMovementLimitation()
        {
            return isMovementLimited;
        }
    }

    public class UIObjectInventory : EventArgs
    {
        private string name;
        private Sprite sprite;
        public UIObjectInventory(string name, Sprite sprite)
        {
            this.name = name;
            this.sprite = sprite;
        }
        
        public string GetName(){return name;}

        public Sprite GetSprite()
        {
            return sprite;
        }
    }

}
