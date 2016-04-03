using UnityEngine;
using UnityEngine.VR;
using System.Collections.Generic;

public class MoveCamera : MonoBehaviour {
    private Dictionary<string, GameObject> outsideObject;
    private GameManager managerObject;

    private GameObject mainCameraFrame;
    private Quaternion localQuaternion;
    private Coroutine spinCoroutine;
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
                Vector3 targetPos = top[0].transform.position;
                targetPos.y = 0;
                float angle = Vector3.Angle(Camera.main.transform.forward, targetPos);
                Vector3 cross  = Vector3.Cross(Camera.main.transform.forward, targetPos);
                if (cross.y < 0) angle = -angle;

                if (Mathf.Abs(angle) > 3.0f)
                    mainCameraFrame.transform.localEulerAngles = new Vector3(0, mainCameraFrame.transform.localEulerAngles.y + Mathf.Floor(angle), 0);
                return;
            }

            movingFlag = true;
            quaternionFlag = false;
        }
    }

    void FixedUpdate()
    {
        Debug.Log(Camera.main.transform.rotation);
        Debug.Log(mainCameraFrame.transform.rotation);
        //Camera.main.transform.localEulerAngles = new Vector3(0, 0, 90);
        if (outsideObject.Count != 0)
        {
            if (movingFlag)
            {
                Transform faceObj = null;
                Vector3 targetPos = Vector3.zero;
                foreach (string key in outsideObject.Keys)
                {
                    faceObj = outsideObject[key].transform;
                    targetPos = faceObj.position;
                    targetPos.y = 0;

                    float angle = Vector3.Angle(Camera.main.transform.forward, targetPos);
                    Vector3 cross = Vector3.Cross(Camera.main.transform.forward, targetPos);
                    if (cross.y < 0)
                        angle = -angle;

                    if (!quaternionFlag)
                    {
                        localQuaternion = Quaternion.Euler(new Vector3(0, angle, 0));
                        quaternionFlag = true;
                    }

                    mainCameraFrame.transform.rotation = Quaternion.Slerp(
                        mainCameraFrame.transform.rotation,
                        localQuaternion,
                        movingSpeed * Time.deltaTime);
                    break;
                }

                if (Vector3.Angle(Camera.main.transform.forward, targetPos) < .1f)
                {
                    quaternionFlag = false;
                    movingFlag = false;
                }
            }
        }
    }

    System.Collections.IEnumerator SpinObject(float angle)
    {
        float duration = angle;
        float elapsed = 0f;
        float spinSpeed = 50f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Debug.Log(string.Format("elapsed: {0}, duration: {1}", elapsed, duration));
            mainCameraFrame.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

}
