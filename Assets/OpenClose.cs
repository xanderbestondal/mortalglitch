using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{

	bool open;

	Vector3 closedAngle;

	Transform cam;


	// Start is called before the first frame update
	void Start()
    {
		closedAngle = transform.TransformDirection(0, 1, 0);
		cam = Camera.main.transform;
	}

    // Update is called once per frame
    void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			// Does the ray intersect any objects
			if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, 4)) //  = max distance
			{
				if (hit.transform == transform)
				{
					opendoor();
				}
			}
		}
    }

	private void opendoor()
	{
		if(Vector3.Angle(closedAngle, transform.TransformDirection(0,1,0)) < 60)
			GetComponent<Rigidbody>().AddTorque(0, 100, 0);
		else
		{
			GetComponent<Rigidbody>().AddTorque(0, -100, 0);
		}
	}
}
