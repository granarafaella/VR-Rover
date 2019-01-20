using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net;
using System;

public class PointCloudGetter : MonoBehaviour
{

    private const int k_MaxPointCount = 61440;

    private MqttClient client;

    private Vector3[] m_Points = new Vector3[k_MaxPointCount];

    // Use this for initialization
    void Start()
    {
        client = new MqttClient(IPAddress.Parse("192.168.43.112"), 1883, false, null);

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);
    }

    // Update is called once per frame
    void Update()
    {
        if (Frame.PointCloud.IsUpdatedThisFrame)
        {
            // Copy the point cloud points for mesh verticies.
            for (int i = 0; i < Frame.PointCloud.PointCount; i++)
            {
                SendPoint(Frame.PointCloud.GetPointAsStruct(i), Frame.Pose);
            }
        }
    }

    void SendPoint(Vector3 point, Pose worldPose)
    {
        var strPoint = point.x.ToString()+","+ point.y.ToString() + ","+ point.z.ToString();
        var pose = worldPose.position;
        var rot = worldPose.rotation.eulerAngles;
        var strPosition = pose.x.ToString() + "," + pose.y.ToString() + "," + pose.z.ToString();
        var strRot = rot.x.ToString() + "," + rot.y.ToString() + "," + rot.z.ToString();
        var finalString = strPoint + "," + strPosition + "," + strRot;
        client.Publish("pointcloud", System.Text.Encoding.UTF8.GetBytes(strPoint), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }

}