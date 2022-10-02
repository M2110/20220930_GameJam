using System;
using UnityEngine;

public class Level1ChurchLocked1State : IStateInterface
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
                playerScript.DisplayMessage("Welcome to the tavern.", 2, true);
                playerScript.DisplayMessage("I heard you are new to this town.", 3, true);
                playerScript.DisplayMessage("You can get to know this place better by helping me.", 3, true);
                playerScript.DisplayMessage("I need to talk to the priest but I can't leave the tavern.", 3, true);
                playerScript.DisplayMessage("It would be great if you can hand him over this letter.", 3, true);
                playerScript.DisplayMessage("The church is locked you say?", 2, true);
                playerScript.DisplayMessage("Maybe you can find the key somewhere on the marketplace.", 2, true);
                playerScript.DisplayMessage("You received a letter for the priest.", 3);
                playerScript.ChangeState(new Level1FindKeyState());
                break;
            case "NPC_Level1_Market":
                playerScript.DisplayMessage("Hello there.", 2, true);
                playerScript.DisplayMessage("Are you new to this city?.", 3, true);
                playerScript.DisplayMessage("If you want to find out more about our small town you should visit the tavern.", 4, true);
                playerScript.DisplayMessage("Where to find the tavern? It is located east from the marketplace.", 4, true);
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
                playerScript.DisplayMessage("This door seems locked.", 1);
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
