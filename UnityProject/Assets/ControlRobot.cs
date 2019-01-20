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
    public MqttClient Client
    {
        get
        {
            if (client == null)
            {
                client = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);

                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
            }
            return client;
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        UpdateControllerState();
    }

    private void UpdateControllerState()
    {
        var thumbstickPosition = MotionControllerVisualizer.ThumbstickPosition;
        SendData(thumbstickPosition);
    }

    void SendData(Vector2 direction)
    {
        var directionstr = direction.x.ToString()+","+direction.y.ToString();
        Client.Publish("telemetry", System.Text.Encoding.UTF8.GetBytes(directionstr), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }
}
