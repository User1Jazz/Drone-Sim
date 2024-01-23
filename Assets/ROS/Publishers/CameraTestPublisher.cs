using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

public class CameraTestPublisher : MonoBehaviour
{
    ROSConnection ros;                                  // ROS COnnection
    public string topicName = "/cam";                   // Topic name
    public float publishMessageFrequency = 0.5f;        // Publish the message every N seconds
    private float timeElapsed;                          // Used to determine how much time has elapsed since the last message was published

    public Camera cam;

    Texture2D texture;
    RenderTexture renderTexture;
    List<Color> pixels;

    public void Init()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<CompressedImageMsg>(topicName);

        // Make sure the camera is rendering to the Render Texture
        renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        cam.targetTexture = renderTexture;
    }

    // Update is called once per frame
    void Update()
    {
        // You can make a Camera render to a RenderTexture using Camera.targetTexture
        // Then, you can convert your RenderTexture into a Texture2D
        // Finally, you can get an array of pixels from a Texture2D by calling Texture2D.GetPixels

        /*renderTexture = new RenderTexture(cam.activeTexture); RenderTexture.active = renderTexture;
        texture = new Texture2D(renderTexture.width, renderTexture.height); texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0); texture.Apply();
        pixels = new List<Color>(texture.GetPixels());
        Debug.Log("Pixel 0: R: " + pixels[0].r + ", G: " + pixels[0].g + ", B: " + pixels[0].b);*/

        timeElapsed += Time.deltaTime;

        // Create a new texture to read the pixels into
        if (timeElapsed > publishMessageFrequency)
        {
            Texture2D screenShot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            RenderTexture.active = null;
            byte[] bytes = screenShot.EncodeToJPG();
            //string filePath = Application.dataPath + "/Screenshots/screenshot.png";
            //System.IO.File.WriteAllBytes(filePath, bytes);
            //Debug.Log("File saved to " + filePath);

            SendImage(bytes);
            //Debug.Log("Image sent!");

            Destroy(screenShot);

            timeElapsed = 0;
        }
    }

    void SendImage(byte[] image)
    {
        // Create and populate message
        CompressedImageMsg message = new CompressedImageMsg(new HeaderMsg(), "png", image);

        // Finally send the message to server_endpoint.py running in ROS
        ros.Publish(topicName, message);
    }
}
