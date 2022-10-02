using System;
using UnityEngine;

public class Level1GoToLockedChurchState : IStateInterface
{
    private PlayerScript playerScript;
    public void OnEnter(PlayerScript player)
    {
        playerScript = player;
        playerScript.DisplayMessage("You received a letter for the priest.", 3);
    }

    public void OnExit()
    {
        
    }

    public void OnTalkToNPC(string name)
    {
        switch (name)
        {
            case "NPC_Level1_Tavern":
                playerScript.DisplayMessage("Please go to the church an hand the letter over to the priest.", 4, true);
                break;
            case "NPC_Level1_Market":
                playerScript.DisplayMessage("Hello, you again.", 2, true);
                playerScript.DisplayMessage("Did you talk to the bartender?", 3, true);
                playerScript.DisplayMessage("Ah to the church he said?", 3, true);
                playerScript.DisplayMessage("You can find the church in the north.", 3, true);
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
                playerScript.DisplayMessage("The door seems locked.", 1);
                playerScript.ChangeState(new Level1ChurchLocked2State());
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
