using System;
using UnityEngine;

public class Level1FindKeyState : IStateInterface
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
                playerScript.DisplayMessage("Maybe you can find the key somewhere on the marketplace.", 3, true);
                break;
            case "NPC_Level1_Market":
                playerScript.DisplayMessage("The door to the church is locked?.", 2, true); 
                playerScript.DisplayMessage("I think I saw the priest in the morning at those chests over there, maybe you are lucky.", 4, true); 
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
