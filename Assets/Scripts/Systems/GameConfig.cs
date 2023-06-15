
[System.Serializable]
public class GameConfig
{
    public string roomName;
    public bool isPrivate;
    public int roomSize;
    public int roundTime;
    public int numRounds;
    public string password;
    public GameConfig()
    {
        roomName = "WaitingRoom";
        isPrivate = false;
        roomSize = 2;
        roundTime = 30;
        numRounds = 1;
        password = null;
    }
    public GameConfig(string roomName, bool isPrivate, int roomSize, int roundTime, int numRounds, string password)
    {
        this.roomName = roomName;
        this.isPrivate = isPrivate;
        this.roomSize = roomSize;
        this.roundTime = roundTime;
        this.numRounds = numRounds;
        this.password = password;
    }
    
}
