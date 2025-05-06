using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using NativeWebSocket;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;


public class ServerManager : MonoBehaviour
{
    public void Start() 
    {
        Application.targetFrameRate = 60;
        InvokeRepeating("Connect", 0.1f, 0.3f);
    }

 
            
    WebSocket websocket;
    public PlayerData LocalPlayer;
    public List<PlayerData> LocalPlayersList = new();
    GameObject primitive;
    string _lastmessage;
    public string lastMessage { set {
            if (value != _lastmessage) 
            {
                _lastmessage = value;
                OnNewMessage.Invoke(value);
            }
        }
    }
    public UnityEvent<string> OnNewMessage = new (); 
    public GamePacket lastpacket;
    // Start is called before the first frame update
     public void Connect(PlayerData player)
    {
        Debug.Log("Connecting to server");
       
        websocket = new WebSocket("ws://localhost:3000", new Dictionary<string, string>
        {
            {"player", player.name},
            {"primative", player.primitive},
            {"xPos", player.xPos},
            {"yPos", player.yPos},
            {"zPos", player.zPos}

        });

        websocket.OnOpen += () =>
        {
            if (websocket.State == WebSocketState.Open)
            {
               
                Debug.LogError("Joined Server");
            }
        };
        websocket.OnMessage += (bytes) =>
        {
            // Reading a plain text message
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            lastpacket = JsonConvert.DeserializeObject<GamePacket>(message);

            //TODO: One massive switch that if it's test will fire a debug log and pass in the parms


            switch (lastpacket.rpc)
            {
                case "Connect":
                    test(lastpacket.rpc);     
                    break;
                case "PlayerJoin":
                    test(lastpacket.rpc);
                    break;
                case "Left":
                    test(lastpacket.rpc);
                    break;
            }
            LocalPlayersList = lastpacket.Gamestate.players;
            lastMessage = lastpacket.message;

          
            foreach ( PlayerData player in LocalPlayersList)
            {
                CreatePlayerAvatar(player);
            }
            
        };
        websocket.OnError += (e) =>
        {
            Debug.LogError("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.LogError("ServerManager closed!");

        };

        websocket.Connect();
    }

    public void test(string parm) 
    {
        Debug.LogError(" hello test: " + parm);
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket!=null)
        {
            websocket.DispatchMessageQueue();
        }
        
#endif
        
    }
   
    public async void SendPlayerData()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Prepare the message data (from PlayerData)
            var message = new
            {
                //playerName = playerjoin.playerData.playerName,
                //primitiveObject = playerjoin.playerData.primitiveObject,
                //position = new
                //{
                //    x = playerjoin.playerData.position.x,
                //    y = playerjoin.playerData.position.y,
                //    z = playerjoin.playerData.position.z
                //}
            };

            // Serialize and send as JSON
            string json = JsonConvert.SerializeObject(message);
            await websocket.SendText(json);
            Debug.LogError("Sent player data: " + json);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    GameObject CreatePlayerAvatar(PlayerData player)
    {
        /// TODO : Make this actually use the right primitives
      
        switch (player.primitive)
        {
            case "Cube":
                primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case "Cylinder":
                 primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                break;
            case "Sphere":
                 primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
        }

        primitive.transform.position = new Vector3(float.Parse(player.xPos), float.Parse(player.yPos), float.Parse(player.zPos));

        GameObject instantiatedParent = Instantiate(Resources.Load<GameObject>("PlayerLabel"), new Vector3(0, 1, 0), Quaternion.identity);

        Transform textChildTransform = instantiatedParent.transform.Find("textVal");

        if (textChildTransform != null)
        {
            textChildTransform.GetComponent<TMP_Text>().text = player.name;
        }

        // Set instantiatedParent as the parent of the primitive
        instantiatedParent.transform.SetParent(primitive.transform);
        instantiatedParent.transform.localPosition = new Vector3(0f, 0.5f, 0.7f);
        instantiatedParent.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        player.primitive = primitive.name;
        return primitive;
    }
}
