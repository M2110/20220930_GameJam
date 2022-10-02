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
        StartCoroutine(LoadNewScene(door.GetDoorName(), door.GetX(), door.GetY(), door.GetDirection()));
    }

    IEnumerator LoadNewScene(string scene, int x, int y, int direction)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != scene)
        {
            yield return null;
        }
        TurnPlayer.Invoke(this, new PlayerDirection(direction));
        player.transform.position = new Vector2(x, y);
        SceneLoaded.Invoke(this, EventArgs.Empty);
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
