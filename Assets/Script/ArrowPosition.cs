using UnityEngine;
using System.Collections;

public class ArrowPosition : MonoBehaviour {
    private GameObject arrowPlane;
    private GameObject trackingObject;
    private GameManager managerObject;
    private bool hideFlag;

    void Start ()
    {
        hideFlag = false;
        arrowPlane = GameObject.Find("ArrowPlane");
        managerObject = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

	void Update () {
        if (trackingObject != null)
        {
            if (!managerObject.arrowVisible && hideFlag)
            {
                Vector3 v3Pos = Camera.main.WorldToViewportPoint(trackingObject.transform.position);
                if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f && v3Pos.z > 0)
                {
                    hideFlag = false;
                }
            }
            else
                PositionArrow();
        }
	}

    public void SetTrackingObject(GameObject obj)
    {
        trackingObject = obj;
    }

    public void SetHideFlag(bool hide)
    {
        hideFlag = hide;
    }

    void PositionArrow()
    {
        GetComponent<Renderer>().enabled = false;

        Vector3 v3Pos = Camera.main.WorldToViewportPoint(trackingObject.transform.position);
        if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f && v3Pos.z > 0)
        {
            StopAllCoroutines();
            return; // Object center is visible
        }

        if (managerObject.arrowVisible)
            GetComponent<Renderer>().enabled = true;
        else
            StartCoroutine(SetArrowVisible());

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

            if (Mathf.Clamp(transform.localPosition.z, -2.55f, 0) == -2.55f)
            {
                transform.localEulerAngles = new Vector3(90.0f, angle, 0.0f);
            }
            else
            {
                transform.localEulerAngles = new Vector3(90.0f, 360 - angle, 0.0f);
            }
                
        }
    }

    IEnumerator SetArrowVisible()
    {
        GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(2.0f);
        GetComponent<Renderer>().enabled = false;
        hideFlag = true;
    }

    IEnumerator SetArrowHide()
    {
        yield return new WaitForSeconds(2.0f);
        GetComponent<Renderer>().enabled = false;
        hideFlag = true;
    }
}
