using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class ControlRobot : MonoBehaviour {

    public MotionControllerVisualizer MotionControllerVisualizer;

    private MqttClient client;

    // Use this for initialization
    void Start () {
        client = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);

        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        client.Subscribe(new string[] { "telemetry/x" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { "telemetry/y" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        
    }
	
	// Update is called once per frame
	void Update () {
        UpdateControllerState();
    }

    Dictionary<string, float> Telemetry = new Dictionary<string, float>(2);

    private void UpdateControllerState()
    {

        var thumbstickPosition = MotionControllerVisualizer.ThumbstickPosition;
        Telemetry["x"] = thumbstickPosition.x;
        Telemetry["y"] = thumbstickPosition.y;

        Debug.LogError("Sending data");
        SendData();
    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
    }

    void SendData()
    {
        client.Publish("telemetry/x", System.Text.Encoding.UTF8.GetBytes(Telemetry["x"].ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        client.Publish("telemetry/y", System.Text.Encoding.UTF8.GetBytes(Telemetry["y"].ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }
}
