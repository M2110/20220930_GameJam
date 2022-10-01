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
    }

    void OnDoorTrigger(object sender, PlayerScript.Door door)
    {
        StartCoroutine(LoadNewScene(door.GetDoorName()));
    }

    IEnumerator LoadNewScene(String door)
    {
        bool hasLoadedScene = false;
        switch (door)
        {
            case "Door_Level1_Tavern":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1_Inside_1");
                while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Level1_Inside_1")
                {
                    yield return null;
                }
                player.transform.position = new Vector2(-5f, -2f);
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
}
