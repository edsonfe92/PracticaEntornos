using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateGameHndler : MonoBehaviour
{
    public string roomName;
    public bool isPrivate;
    public int roomSize;
    public int roundTime;
    public int numRounds;
    public string password;    

    private void Start()
    {
        roomName = "BattleRoom";
        roomSize = 2;
        roundTime = 30;
        numRounds = 1;
    }
    public void InputFieldRoomName(string name) 
    {
        roomName = name;
    }
    public void InputFieldPassword(string name)
    {
        password = name;
    }    
    public void DropdownRoomSize(int index) 
    {
        switch (index)
        {
            case 0: roomSize = 2; break;
            case 1: roomSize = 4; break;
            case 2: roomSize = 6; break;
        }
    }
    public void DropdownRoundTime(int index)
    {
        switch (index)
        {
            case 0: roomSize = 30; break;
            case 1: roomSize = 60; break;
            case 2: roomSize = 120; break;
        }
    }
    public void DropdownNumRounds(int index)
    {
        switch (index)
        {
            case 0: roomSize = 1; break;
            case 1: roomSize = 3; break;
            case 2: roomSize = 5; break;
        }
    }
    public void ToggleIsPrivate(bool b) 
    {
        isPrivate = b;
    }
    public void ResetPassword() 
    {
        password = null;        
    }

}
