using UnityEngine;
using System.Collections;

public class OVRMoveManager : MonoBehaviour {
    private OVRCameraRig OVRrig;

	// Use this for initialization
	void Start () {
        OVRrig = GameObject.Find("GameManager").GetComponent<OVRCameraRig>();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
