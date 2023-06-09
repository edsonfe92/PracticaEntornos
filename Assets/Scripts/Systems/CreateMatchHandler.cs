using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateMatchHandler : MonoBehaviour
{
    public GameConfig gameConfig;
    private void Start()
    {
        gameConfig = new GameConfig();
    }
    public void InputFieldRoomName(string name)
    {
        gameConfig.roomName = name;        
    }
    public void InputFieldPassword(string name)
    {
        gameConfig.password = name;
    }
    public void DropdownRoomSize(int index)
    {
        switch (index)
        {
            case 0: gameConfig.roomSize = 2; break;
            case 1: gameConfig.roomSize = 4; break;
            case 2: gameConfig.roomSize = 6; break;
        }
    }
    public void DropdownRoundTime(int index)
    {
        switch (index)
        {
            case 0: gameConfig.roundTime = 30; break;
            case 1: gameConfig.roundTime = 60; break;
            case 2: gameConfig.roundTime = 120; break;
        }
    }
    public void DropdownNumRounds(int index)
    {
        switch (index)
        {
            case 0: gameConfig.numRounds = 1; break;
            case 1: gameConfig.numRounds = 3; break;
            case 2: gameConfig.numRounds = 5; break;
        }
    }
    public void ToggleIsPrivate(bool b)
    {
        gameConfig.isPrivate = b;
    }
    public void ResetPassword()
    {
        gameConfig.password = null;
    }
}

