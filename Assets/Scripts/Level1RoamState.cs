using System;
using UnityEngine;

public class Level1RoamState : IStateInterface
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
                playerScript.DisplayMessage("Thank you. Feel free to look around.", 3, true);
                break;
            case "NPC_Level1_Market":
                playerScript.DisplayMessage("I recommend to explore the area now.", 3, true);
                break;
            case "NPC_Level1_Church":
                playerScript.DisplayMessage("Thanks for your help, sadly I have no time to lead you around the town.", 4, true);
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
        playerScript.DisplayMessage("You didn't find anything interresting.", 3);
    }
}
