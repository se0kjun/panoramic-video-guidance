using UnityEngine;
using UnityEngine.VR;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour {
    private Dictionary<string, GameObject> outsideObject;
    private GameManager managerObject;

    private GameObject mainCameraFrame;
    private Quaternion localQuaternion;
    private float movingSpeed;
    private bool movingFlag;
    private bool quaternionFlag;

    //moving camera settings
    private bool movingCameraAvailable;
    private bool movingCameraEase;

	void Start () {
        managerObject = GameObject.Find("GameManager").GetComponent<GameManager>();
        movingCameraAvailable = managerObject.moveCameraAvailable;
        movingCameraEase = managerObject.cameraEase;
        mainCameraFrame = GameObject.Find("TrackingSpace");
        outsideObject = managerObject.outsideCameraObject;
        movingSpeed = 1.0f;
        movingFlag = false;
        quaternionFlag = false;
    }
	
	void Update () {
        if (Input.GetKey(KeyCode.Space) && movingCameraAvailable)
        {
            if (!movingCameraEase && outsideObject.Count != 0)
            {
                List<GameObject> top = new List<GameObject>(outsideObject.Values);
                mainCameraFrame.transform.LookAt(top[0].transform.position);
                return;
            }

            movingFlag = true;
        }
    }

    void LateUpdate()
    {
        if (outsideObject.Count != 0)
        {
            if (movingFlag)
            {
                Transform faceObj = null;
                foreach (string key in outsideObject.Keys)
                {
                    faceObj = outsideObject[key].transform;
                    float angle = Vector3.Angle(Camera.main.transform.forward, faceObj.position);

                    if (!quaternionFlag)
                    {
                        //localQuaternion = Quaternion.LookRotation(faceObj.position - Camera.main.transform.forward);
                        localQuaternion = Quaternion.Euler(
                            Vector3.Angle(faceObj.right, Camera.main.transform.right),
                            Vector3.Angle(faceObj.up, Camera.main.transform.up),
                            Vector3.Angle(faceObj.forward, Camera.main.transform.forward)
                            );
                        quaternionFlag = true;
                    }

                    //Quaternion.
                    //mainCameraFrame.transform.eulerAngles = Vector3.Slerp(mainCameraFrame.transform.position, )
                    mainCameraFrame.transform.rotation = Quaternion.Slerp(
                        mainCameraFrame.transform.rotation, localQuaternion, movingSpeed * Time.deltaTime);
                    Debug.Log(localQuaternion);
                    Debug.Log(Quaternion.LookRotation(faceObj.position - Camera.main.transform.position));
                    //Debug.Log(Quaternion.Slerp(
                    //    mainCameraFrame.transform.rotation, Quaternion.LookRotation(faceObj.position - mainCameraFrame.transform.position), movingSpeed * Time.deltaTime));
                    break;
                }

                if (Vector3.Angle(faceObj.position - mainCameraFrame.transform.position, mainCameraFrame.transform.forward) < .1f)
                {
                    quaternionFlag = false;
                    movingFlag = false;
                }
            }
        }
    }
}
