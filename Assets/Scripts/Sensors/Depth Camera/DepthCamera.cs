using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DepthCamera : MonoBehaviour
{
	Camera cam;
	Material mat;
	public bool invertColor = true;
	bool changesFlag = false;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cam == null)
		{
			cam = this.GetComponent<Camera>();
			cam.depthTextureMode = DepthTextureMode.DepthNormals;
		}
		if(mat == null || changesFlag != invertColor)
		{
			if(invertColor)
			{
				mat = new Material(Shader.Find("Hidden/InvertedDepthNormalShader"));
			}else
			{
				mat = new Material(Shader.Find("Hidden/DepthNormalShader"));
			}
			changesFlag = invertColor;
		}
    }
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, mat);
	}
}
