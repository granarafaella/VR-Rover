using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class PointcloudRenderer : MonoBehaviour {

    public GameObject pointPrefab;

    public ControlRobot controlRobotScript;
    public Camera RootCamera;

    private Vector3 cameraPos;
    private Vector3 newCameraPos;
    private Vector3 newCameraRot;

    private bool SetCamera = false;

    private MqttClient mqttClient;

    private Vector3 newPC;
    private Vector3 oldPC;

	// Use this for initialization
	void Start () {
        mqttClient = controlRobotScript.Client;
        mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;

        mqttClient.Subscribe(new string[] { "pointcloud" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        
    }

    private void Update()
    {
        if(newPC != oldPC)
        {
            RenderPoint(newPC);
            oldPC = newPC;
        }
        RootCamera.transform.position = newCameraPos;
        RootCamera.transform.rotation = Quaternion.Euler(newCameraRot);
    }

    private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.LogError("Received message 1");
        var vertices = System.Text.Encoding.UTF8.GetString(e.Message).ToString().Split(',');
        newPC = new Vector3(float.Parse(vertices[0]),
            float.Parse(vertices[1]),
            float.Parse(vertices[2]));
        newCameraPos = new Vector3(float.Parse(vertices[3]),
            float.Parse(vertices[4]),
            float.Parse(vertices[5]));
        newCameraRot = new Vector3(float.Parse(vertices[6]),
            float.Parse(vertices[7]),
            float.Parse(vertices[8]));
    }

    private void RenderPoint(Vector3 renderPos)
    {
        Instantiate(pointPrefab, renderPos, Quaternion.identity);
    }

    private void OnDestroy()
    {
        mqttClient.MqttMsgPublishReceived -= MqttClient_MqttMsgPublishReceived;
    }
}
