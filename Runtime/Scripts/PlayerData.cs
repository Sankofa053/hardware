using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string name;
    public string primitive;
    public string xPos;
    public string yPos;
    public string zPos;

}
[System.Serializable]
public class Gamestate
{
    public List<PlayerData> players;
}

[System.Serializable]
public class GamePacket
{
    public Gamestate Gamestate;
    public string message;
    public string rpc;
    public List<string> rpcdata;
}