using UnityEngine;
using System.Collections.Generic;

public class OVRMoveManager : MonoBehaviour {
    private GameObject trackingSpace;
    private GameManager gameManager;
    private List<MarkerXMLWrapper> spawningObject;

    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        trackingSpace = GameObject.Find("TrackingSpace");
        spawningObject = gameManager.GetSpawnObject();
	}
	
	void Update ()
    {
	}
}
