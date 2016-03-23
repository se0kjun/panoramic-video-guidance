using UnityEngine;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour {
    private Dictionary<string, GameObject> outsideObject;
    private GameManager managerObject;

	void Start () {
        managerObject = GameObject.Find("GameManager").GetComponent<GameManager>();
        outsideObject = managerObject.outsideCameraObject;
    }
	
	void Update () {
        if (outsideObject.Count != 0)
        {
            //if (Input.GetKey(KeyCode.Space))
            //{
                foreach (string key in outsideObject.Keys)
                {
                    Transform mainCam = Camera.main.transform;
                    Transform faceObj = outsideObject[key].transform;
                Debug.DrawLine(Vector3.zero, faceObj.position - mainCam.position, Color.red);
                Debug.Log(mainCam.name);
                    Camera.main.transform.rotation = Quaternion.Slerp(
                        Camera.main.transform.rotation, Quaternion.LookRotation(faceObj.position - mainCam.position), 5.0f * Time.deltaTime);
                    //Debug.Log(Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.LookRotation(faceObj.position - mainCam.position), 5.0f * Time.deltaTime));
                    break;
                }
            //}
        }
    }
}
