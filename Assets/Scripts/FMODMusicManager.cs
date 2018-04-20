using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODMusicManager : MonoBehaviour {

    [Range(0f, 1f)]
    public float volume;
	
	void Start () {
		
	}
	
	void Update () {
		GetComponent<StudioEventEmitter>().EventInstance.setVolume(volume);
	}
}
