using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    private GameObject player;
    private Light2D globalLight;
    private Vector2 playerPosition;
    private string currentRoom;
    private float lightIntensity;

    public static event EventHandler SceneLoaded;
    public static event EventHandler<PlayerDirection> TurnPlayer; 

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] enemies;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        PlayerScript.DoorEntered += OnDoorTrigger;
    }

    private void OnDisable()
    {
        PlayerScript.DoorEntered -= OnDoorTrigger;
    }
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1_Outside");
        player = GameObject.Find("Player");
    }

    void OnDoorTrigger(object sender, PlayerScript.Door door)
    {
        player ??= (GameObject)sender;
        StartCoroutine(LoadNewScene(door.GetDoorName()));
    }

    IEnumerator LoadNewScene(String door)
    {
        bool hasLoadedScene = false;
        switch (door)
        {
            case "Door_Level1_Tavern":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1_Inside1");
                while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Level1_Inside1")
                {
                    yield return null;
                }
                TurnPlayer.Invoke(this, new PlayerDirection(0));
                player.transform.position = new Vector2(-5f, -2f);
                hasLoadedScene = true;
                break;
            case "Door_Level1_Outside_Tavern":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1_Outside");
                while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Level1_Outside")
                {
                    yield return null;
                }
                TurnPlayer.Invoke(this, new PlayerDirection(2));
                player.transform.position = new Vector2(36f, 4f);
                hasLoadedScene = true;
                break;
            case "Door_Level1_Church":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1_Inside2");
                while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Level1_Inside2")
                {
                    yield return null;
                }
                TurnPlayer.Invoke(this, new PlayerDirection(0));
                player.transform.position = new Vector2(0f, -2f);
                hasLoadedScene = true;
                break;
            case "Door_Level1_Outside_Church":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1_Outside");
                while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Level1_Outside")
                {
                    yield return null;
                }
                TurnPlayer.Invoke(this, new PlayerDirection(2));
                player.transform.position = new Vector2(-11f, 23f);
                hasLoadedScene = true;
                break;
            default:
                Debug.LogWarning($"No door with name {door} found.");
                break;
        }
        if (hasLoadedScene)
        {
            SceneLoaded.Invoke(this, EventArgs.Empty);
        }
    }

    public class PlayerDirection : EventArgs
    {
        private int direction;

        public PlayerDirection(int direction)
        {
            this.direction = direction;
        }

        public int GetDirection()
        {
            return direction;
        }
    }
}
