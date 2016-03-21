using UnityEngine;

public class ArrowPosition : MonoBehaviour {
    private GameObject arrowPlane;
    private GameObject trackingObject;

    void Start ()
    {
        arrowPlane = GameObject.Find("ArrowPlane");
    }

	void Update () {
        if (trackingObject != null)
        {
            PositionArrow();
        }
	}

    public void SetTrackingObject(GameObject obj)
    {
        trackingObject = obj;
    }

    void PositionArrow()
    {
        GetComponent<Renderer>().enabled = false;

        Vector3 v3Pos = Camera.main.WorldToViewportPoint(trackingObject.transform.position);
        if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f && v3Pos.z > 0)
        {
            return; // Object center is visible
        }

        GetComponent<Renderer>().enabled = true;

        Vector3 v1 = trackingObject.transform.position - Camera.main.transform.position;
        Vector3 v2 = Vector3.Project(v1, Camera.main.transform.forward);
        Vector3 v3 = Camera.main.transform.position + (v1 - v2);

        Vector3 projectPoint = arrowPlane.transform.InverseTransformPoint(v3);
        projectPoint.y = 0;
        Vector3 raycastingPoint = arrowPlane.transform.TransformPoint(projectPoint);
        Vector3 raycastingDir = raycastingPoint - arrowPlane.transform.position;
        Ray ray = new Ray(raycastingPoint, -raycastingDir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            float angle = Vector3.Angle(Camera.main.transform.right, v3);

            if (hit.transform.tag == "PlaneEdge")
            {
                transform.position = hit.point;
            }
            else if (hit.transform.tag == "PlaneEdgeBack")
            {
                ray.direction = raycastingDir;
                Physics.Raycast(ray, out hit);
                transform.position = hit.point;
            }

            Debug.Log(transform.localPosition.z);
            if (Mathf.Clamp(transform.localPosition.z, -2.55f, 0) == -2.55f)
            {
                transform.localEulerAngles = new Vector3(90.0f, angle, 0.0f);
                Debug.Log("test");
            }
            else
            {
                transform.localEulerAngles = new Vector3(90.0f, 360 - angle, 0.0f);
                Debug.Log("tejiowjwie");
            }
                
        }
    }
}
