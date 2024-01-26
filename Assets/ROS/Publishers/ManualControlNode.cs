using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.DroneSimMessages;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;

public class ManualControlNode : MonoBehaviour
{
    // ROS Variables
    ROSConnection ros;                                  // ROS Connection
    public string topicName = "/D#/control";            // Topic name
    public float publishMessageFrequency = 0.01f;       // Publish the message every N seconds
    private float timeElapsed;                          // Used to determine how much time has elapsed since the last message was published
    public bool manualMode = false;
    public bool started = false;
    Vector3 linear = Vector3.zero;
    Vector3 angular = Vector3.zero;

    public void Init()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Drone_SensorsMsg>(topicName);
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (manualMode && started)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > publishMessageFrequency)
            {
                HandleManualInput();

                PublishData();
                timeElapsed = 0;
            }
        }
    }

    void HandleManualInput()
    {
        linear.x = Input.GetAxis("Vertical");
        linear.y = -Input.GetAxis("Horizontal");

        /*if (Input.GetKey("w"))
        {
            cyclic.x = 1f;
        }
        if (Input.GetKey("s"))
        {
            cyclic.x = -1f;
        }
        if (Input.GetKeyUp("w") || Input.GetKeyUp("s"))
        {
            cyclic.x = 0f;
        }

        if (Input.GetKey("a"))
        {
            cyclic.y = 1f;
        }
        if (Input.GetKey("d"))
        {
            cyclic.y = -1f;
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            cyclic.y = 0f;
        }*/

        if (Input.GetKey("q"))
        {
            angular.z = -1f;
        }
        if (Input.GetKey("e"))
        {
            angular.z = 1f;
        }
        if (Input.GetKeyUp("q") || Input.GetKeyUp("e"))
        {
            angular.z = 0f;
        }

        if (Input.GetKey("r"))
        {
            linear.z = 1f;
        }
        if (Input.GetKey("f"))
        {
            linear.z = -1f;
        }
        if (Input.GetKeyUp("r") || Input.GetKeyUp("f"))
        {
            linear.z = 0f;
        }
    }

    void PublishData()
    {
        TwistMsg twister = new TwistMsg(
            new Vector3Msg(linear.x, linear.y, linear.z), new Vector3Msg(angular.x, angular.y, angular.z)
            );
        Drone_ControlMsg message = new Drone_ControlMsg(
            new HeaderMsg(),
            twister
            );
        ros.Publish(topicName, message);
    }
}
