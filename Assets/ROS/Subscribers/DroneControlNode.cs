using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class DroneControlNode : MonoBehaviour
{
    public DroneController droneControllerScript;
    public string topicName = "/control";

    public void Init()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<TwistMsg>(topicName, ListenerFunction);
    }

    void ListenerFunction(TwistMsg message)
    {
        Debug.Log(message.linear);
        Debug.Log(message.angular);
    }
}
