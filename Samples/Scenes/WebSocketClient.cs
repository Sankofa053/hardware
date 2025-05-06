using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket ws;

    // Data to store button states
    private string pushBtn1;
    private string pushBtn2;
    private string pushBtn3;
    private string pushBtn4;
    private string pushBtn5;
    private string pushBtn6;
    private string pushBtn7;
    private string pushBtn8;
    private string pushBtn9;

    // WebSocket URL (make sure it matches your server's URL)
    private string serverUrl = "ws://localhost:8000";

    // Button data class to match the structure of your JSON data
    [System.Serializable]
    public class ButtonData
    {
        public int PushBtn1;
        public int PushBtn2;
        public int PushBtn3;
        public int PushBtn4;
        public int PushBtn5;
        public int PushBtn6;
        public int PushBtn7;
        public int PushBtn8;
        public int PushBtn9;
    }

    async void Start()
    {
        // Initialize WebSocket
        ws = new WebSocket("ws://localhost:8000", new Dictionary<string, string>
        {
            {"pushBtn1", pushBtn1},
            {"pushBtn2", pushBtn2},
            {"pushBtn3", pushBtn3},
            {"pushBtn4", pushBtn4},
            {"pushBtn5", pushBtn5},
            {"pushBtn6", pushBtn6},
            {"pushBtn7", pushBtn7},
            {"pushBtn8", pushBtn8},
            {"pushBtn9", pushBtn9}

        });
        // On Open - When WebSocket connection is established
        ws.OnOpen += () => {
            Debug.Log("Connected to WebSocket server.");
        };

        // On Message - When data is received from the server
        ws.OnMessage += (bytes) => {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received message: " + message);

            // Deserialize the message into ButtonData object using Newtonsoft.Json
            ButtonData buttonData = JsonConvert.DeserializeObject<ButtonData>(message);
// Update the button states
            pushBtn1 = buttonData.PushBtn1.ToString();
            pushBtn2 = buttonData.PushBtn2.ToString();
            pushBtn3 = buttonData.PushBtn3.ToString();
            pushBtn4 = buttonData.PushBtn4.ToString();
            pushBtn5 = buttonData.PushBtn5.ToString();
            pushBtn6 = buttonData.PushBtn6.ToString();
            pushBtn7 = buttonData.PushBtn7.ToString();
            pushBtn8 = buttonData.PushBtn8.ToString();
            pushBtn9 = buttonData.PushBtn9.ToString();

            // Log the updated button states
            Debug.Log($"Button States - Btn1: {pushBtn1}, Btn2: {pushBtn2}, Btn3: {pushBtn3}, Btn4: {pushBtn4},  Btn5: {pushBtn5}, Btn6: {pushBtn6}, Btn7: {pushBtn7}, Btn8: {pushBtn8}, Btn9: {pushBtn9}");
        };

        // On Error - If there is an error
        ws.OnError += (error) => {
            Debug.LogError("WebSocket Error: " + error);
        };

        // On Close - When WebSocket connection is closed
        ws.OnClose += (reason) => {
            Debug.Log("Disconnected from WebSocket server. Reason: " + reason);
        };

        // Connect to the server asynchronously
        await ws.Connect();
    }

    void Update()
    {
        // Keep the WebSocket alive and poll for updates
        if (ws.State == WebSocketState.Open)
        {
            ws.DispatchMessageQueue();
        }
    }

    private void OnDestroy()
    {
        // Close the WebSocket when the object is destroyed
        if (ws != null)
        {
            ws.Close();
        }
    }
}
