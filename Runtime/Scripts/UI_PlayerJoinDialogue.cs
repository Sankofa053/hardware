using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System;


public class UI_PlayerJoinDialogue : MonoBehaviour
{
    //TODO: Move into some config thing
    public Vector3 minArea; 
    public Vector3 maxArea;

    // Objected created
    [SerializeField] private TMP_Dropdown PrimitiveSelector;

    public TMP_InputField nameText;
    Vector3 pos = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    public ServerManager connection;
    public void Connect()
    {
        PlayerData playerData = new PlayerData();

        // assing position and username
        pos.x = Random.Range(minArea.x, maxArea.x);
        pos.y = Random.Range(minArea.y, maxArea.y);
        pos.z = Random.Range(minArea.z, maxArea.z);
        //playerData.position = pos;
        playerData.name = nameText.text;
        playerData.xPos = pos.x.ToString("F2");
        playerData.yPos = pos.y.ToString("F2");
        playerData.zPos = pos.z.ToString("F2");

        Debug.Log("My player data: " + playerData.name);
        Debug.Log("My position: (" + playerData.xPos + ", " + playerData.yPos + ", " + playerData.zPos + " )");
        // assign primitive
        switch (PrimitiveSelector.value)
        {
            case 1:
                playerData.primitive = "Cube";
                break;
            case 2:
                playerData.primitive = "Cylinder";
                break;
            case 3:
                playerData.primitive = "Sphere";
                break;
        }
        connection.Connect(playerData);
    }
}











































































//public void GetPlayers(string primitiveType, Vector3 playerPos)
//{
//    switch (primitiveType)
//    {
//        case "Cube":
//            CreatePlayerAvatar(PrimitiveType.Cube);
//            break;
//        case "Cylinder":
//            CreatePlayerAvatar(PrimitiveType.Cylinder);
//            break;
//        case "Sphere":
//            CreatePlayerAvatar(PrimitiveType.Sphere);
//            break;
//    }
//    primitive.transform.position = playerPos;
//}

