using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public struct PlatformTime
    {
        public GameObject platform;
        public float time;

        public PlatformTime(GameObject p, float t) {
            platform = p;
            time = t;
        }
    }

    public class SyncListPlatformTime : SyncListStruct<PlatformTime> { }

    [SyncVar]
    public bool gameIsRunning;

    [SyncVar]
    public GameObject player1, player2;

    [SyncVar]
    public float roundTime;
    
    public SyncListPlatformTime movingPlatformTimers = new SyncListPlatformTime();

	void Start () {
        // Add platforms to struct (this should run only on server once at start)
        foreach (MovingPlatform platform in FindObjectsOfType<MovingPlatform>()) {
            if (GetPlatformInfoFromStruct(platform.gameObject).Key == -1) {
                movingPlatformTimers.Add(new PlatformTime(platform.gameObject, 0));
            }
        }
	}
	
	void Update () {
		if (gameIsRunning) {
            roundTime += Time.deltaTime;
        }

        for (int i = 0; i < movingPlatformTimers.Count; ++i) {
            PlatformTime pt = movingPlatformTimers[i];
            if (pt.platform.GetComponent<MovingPlatform>() != null) {
                MovingPlatform mp = pt.platform.GetComponent<MovingPlatform>();
                switch (mp.movementType) {
                    case PlatformMovementType.Normal:
                        if (!pt.platform.GetComponent<MovingPlatform>().activated) {
                            pt.time += Time.deltaTime;
                            movingPlatformTimers[i] = pt;
                        }
                        break;
                    case PlatformMovementType.NormalInvertedActivation:
                        if (pt.platform.GetComponent<MovingPlatform>().activated) {
                            pt.time += Time.deltaTime;
                            movingPlatformTimers[i] = pt;
                        }
                        break;
                    case PlatformMovementType.StartToEnd:
                        if (pt.platform.GetComponent<MovingPlatform>().activated) {
                            pt.time = Mathf.Clamp(pt.time + pt.platform.GetComponent<MovingPlatform>().speed, 0, 1);
                        } else {
                            pt.time = Mathf.Clamp(pt.time - pt.platform.GetComponent<MovingPlatform>().speed, 0, 1);
                        }
                        movingPlatformTimers[i] = pt;
                        break;
                }
            }
        }
	}

    public int GetPlayerNumber(GameObject p) {
        return (p == player1 ? 1 : p == player2 ? 2 : 0);
    }

    public void BeginGame(GameObject p1, GameObject p2) {
        player1 = p1;
        player2 = p2;
        gameIsRunning = true;

        foreach (MovingPlatform platform in FindObjectsOfType<MovingPlatform>()) {
            movingPlatformTimers.Add(new PlatformTime(platform.gameObject, 0));
            Debug.Log("found");
        }
    }

    /// <summary>
    /// Returns index and time for platform in the struct.
    /// 
    /// (This shit still doesn't have properly proper generic-length tuples or "first/second" pairs, so just go with it)
    /// </summary>
    public KeyValuePair<int, float> GetPlatformInfoFromStruct(GameObject platform) {
        for (int i = 0; i < movingPlatformTimers.Count; ++i) {
            if (movingPlatformTimers[i].platform == platform) {
                return new KeyValuePair<int, float>(i, movingPlatformTimers[i].time);
            }
        }
        return new KeyValuePair<int, float>(-1, 0);
    }
}
