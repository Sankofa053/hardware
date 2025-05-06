using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;
using System.Net.Sockets;




public class SocketConnection : MonoBehaviour
{
    WebSocket websocket;
    public GameObject player;
    public PlayerJoinedData playerData;

   

    // Start is called before the first frame update
    async void Start()
    {
        // websocket = new WebSocket("ws://echo.websocket.org");
        websocket = new WebSocket("ws://localhost:3000");

        websocket.OnOpen += () =>
        {
            Debug.Log("ServerManager open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("ServerManager closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            // Reading a plain text message
           string myPlayerData = JsonConvert.SerializeObject(playerData);
           //var message = System.Text.Encoding.UTF8.GetString(bytes);
           Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + myPlayerData);
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        await websocket.Connect();
    }

    void Update()
    {


#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
           
                //Grab player current position and rotation data
                playerData.xPos = player.transform.position.x;
                playerData.yPos = player.transform.position.y;
                playerData.zPos = player.transform.position.z;

                System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
                double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
                //Debug.Log(timestamp);
                playerData.timestamp = timestamp;

                string playerDataJSON = JsonConvert.SerializeObject(playerData);
                var message = System.Text.Encoding.UTF8.GetBytes(playerDataJSON);
                websocket.Send(message);
                Debug.Log("Received OnMessage! " + message);
            

        }

    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}