using System;
using UnityEngine;

public class Level1GoBackToTavernState : IStateInterface
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
                playerScript.DisplayMessage("Thanks, that is what I needed.", 3, true);
                playerScript.DisplayMessage("Now you have seen parts of our town. Feel free to walk around now.", 4, true);
                playerScript.ChangeState(new Level1RoamState());
                break;
            case "NPC_Level1_Market":
                playerScript.DisplayMessage("Oh you spoke to the priest?", 2, true);
                playerScript.DisplayMessage("I guess you know where to find the tavern by now, don't you?", 4, true);
                break;
            case "NPC_Level1_Church":
                playerScript.DisplayMessage("Thank you my son.", 3, true);
                playerScript.DisplayMessage("Would you please bring the jar to the tavern?", 4, true);
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
            case "Door_Level1_House4":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            case "Door_Level1_House5":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            case "Door_Level1_House6":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            case "Door_Level1_House7":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            case "Door_Level1_House8":
                playerScript.DisplayMessage("There is nobody at home.", 1);
                break;
            case "Door_Level1_House9":
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
