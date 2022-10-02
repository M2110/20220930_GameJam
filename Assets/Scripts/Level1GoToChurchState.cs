using System;
using UnityEngine;

public class Level1GoToChurchState : IStateInterface
{
    private PlayerScript playerScript;
    public void OnEnter(PlayerScript player)
    {
        playerScript = player;
    }

    public void OnExit()
    {
        
    }

    public void OnTalkToNPC(string name)
    {
        switch (name)
        {
            case "NPC_Level1_Tavern":
                playerScript.DisplayMessage("Good job on finding that key.", 3, true);
                playerScript.DisplayMessage("Now please go to the church and hand the letter over to the priest.", 4, true);
                break;
            case "NPC_Level1_Market":
                playerScript.DisplayMessage("Now that you have the key, you can enter the church.", 3, true);
                playerScript.DisplayMessage("You can find the church in the north.", 3, true);
                break;
            case "NPC_Level1_Church":
                break;
            default:
                Debug.LogWarning("Unknown npc!");
                break;
        }
    }

    public void OnDoorEntered(string name)
    {
        switch (name)
        {
            case "Door_Level1_Tavern":
                playerScript.EnterDoor("Level1_Inside1", -5, -2, 0);
                break;
            case "Door_Level1_Outside_Tavern":
                playerScript.EnterDoor("Level1_Outside", 36, 4, 2);
                break;
            case "Door_Level1_Church":
                playerScript.EnterDoor("Level1_Inside2", 0, -2, 0);
                break;
            case "Door_Level1_Outside_Church":
                playerScript.EnterDoor("Level1_Outside", -11, 23, 2);
                break;
            case "Door_Level1_House1":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            case "Door_Level1_House2":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            case "Door_Level1_House3":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            default:
                Debug.LogWarning("Unknown door!");
                break;
        }
    }

    public void OnLevelChange(string name)
    {
        playerScript.OnLevelChangeRejected();
    }

    public void OnLookIntoStorage(string name)
    {
        
    }
}
