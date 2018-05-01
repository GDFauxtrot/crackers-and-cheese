using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamDepthTexture : MonoBehaviour {
	void Start () {
		Camera.main.depthTextureMode = DepthTextureMode.Depth;
	}
}
