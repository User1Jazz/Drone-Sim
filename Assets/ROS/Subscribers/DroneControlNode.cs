using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.DroneSimMessages;

public class DroneControlNode : MonoBehaviour
{
    public DroneController droneControllerScript;
    public string topicName = "/D#/control";

    public void Init()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Drone_ControlMsg>(topicName, ListenerFunction);
    }

    void ListenerFunction(Drone_ControlMsg message)
    {
        droneControllerScript.linear.x = (float)message.twist.linear.x;     // Forward/Backward
        droneControllerScript.linear.y = (float)message.twist.linear.y;     // Left/Right
        droneControllerScript.linear.z = (float)message.twist.linear.z;     // Up/Down

        droneControllerScript.angular.x = (float)message.twist.angular.x;   // Roll
        droneControllerScript.angular.y = (float)message.twist.angular.y;   // Pitch
        droneControllerScript.angular.z = (float)message.twist.angular.z;   // Yaw (turn left/right)
    }
}
