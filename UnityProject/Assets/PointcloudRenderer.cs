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

    //public GameObject Rover;
    //private GameObject roverObj;

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

        //roverObj = Instantiate(Rover, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        cameraPos = RootCamera.transform.position;
        if(newPC != oldPC)
        {
            RenderPoint(newPC);
            oldPC = newPC;
        }

        //roverObj.transform.position = Vector3.Lerp(roverObj.transform.position, newCameraPos, 0.5f);
    }

    private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        var vertices = System.Text.Encoding.UTF8.GetString(e.Message).ToString().Split(',');
        newPC = new Vector3(float.Parse(vertices[0]),
            float.Parse(vertices[1]),
            float.Parse(vertices[2]));
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
