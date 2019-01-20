using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class ControlRobot : MonoBehaviour {

    [Tooltip("Name of the thumbstick axis to check for teleport and strafe.")]
    public XboxControllerMappingTypes HorizontalStrafe = XboxControllerMappingTypes.XboxLeftStickHorizontal;

    [Tooltip("Name of the thumbstick axis to check for movement forwards and backwards.")]
    public XboxControllerMappingTypes ForwardMovement = XboxControllerMappingTypes.XboxLeftStickVertical;

    [Tooltip("Name of the thumbstick axis to check for rotation.")]
    public XboxControllerMappingTypes HorizontalRotation = XboxControllerMappingTypes.XboxRightStickHorizontal;

    [Tooltip("Name of the thumbstick axis to check for rotation.")]
    public XboxControllerMappingTypes VerticalRotation = XboxControllerMappingTypes.XboxRightStickVertical;


    const float MovementAmount = 0.5f;
    const float RotationAmount = 45.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (InteractionManager.numSourceStates == 0)
        {
            HandleGamepad();
        }
    }

    Dictionary<string, float> Telemetry = new Dictionary<string, float>(2);

    private void HandleGamepad()
    {
        float leftX = Input.GetAxis(XboxControllerMapping.GetMapping(HorizontalStrafe));
        float leftY = Input.GetAxis(XboxControllerMapping.GetMapping(ForwardMovement));

        float rightY = Input.GetAxis(XboxControllerMapping.GetMapping(VerticalRotation));
        float rightX = Input.GetAxis(XboxControllerMapping.GetMapping(HorizontalRotation));

        if (leftY > 0.8 && Math.Abs(leftX) < 0.3)
        {
            Telemetry["forward"] = +MovementAmount;
            Debug.Log(Telemetry["forward"]);
        }
        else if (leftY < -0.8 && Math.Abs(leftX) < 0.3)
        {
            Telemetry["forward"] = -MovementAmount;
            Debug.Log(Telemetry["forward"]);
        }

        if (rightX < -0.8 && Math.Abs(rightY) < 0.3)
        {
            Telemetry["rotation"] = -RotationAmount;
            Debug.Log(Telemetry["rotation"]);
        }
        else if (rightX > 0.8 && Math.Abs(rightY) < 0.3)
        {
            Telemetry["rotation"] = +RotationAmount;
            Debug.Log(Telemetry["rotation"]);
        }

    }


}
