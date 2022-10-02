using UnityEngine;

public interface IStateInterface
{
    void OnEnter(PlayerScript playerScript);
    void OnExit();
    void OnTalkToNPC(string name);
    void OnDoorEntered(string name);
    void OnLevelChange(string name);
    void OnLookIntoStorage(string name);
}
