using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carEnter : MonoBehaviour
{
	public Transform camTarg;

	bool isdriving;
	CarUserControl carUserControl;
	move moveScript;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if (Physics.Raycast(camTarg.position, camTarg.TransformDirection(Vector3.forward), out hit, 3)) // 3 = max distance
			{
				print(hit.transform);

				if (hit.rigidbody.gameObject.tag == "car")
				{

					carUserControl = hit.rigidbody.GetComponent<CarUserControl>();

					moveScript = GetComponent<move>();
					moveScript.enableMove = false;
					moveScript.GetComponent<Rigidbody>().isKinematic = true;
					moveScript.GetComponentInChildren<Collider>().isTrigger = true;

					carUserControl.enabled = true;
					isdriving = true;
					transform.SetParent(hit.transform);
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;

					// transfer Ownership
					carUserControl.GetComponent<PhotonView>().TransferOwnership(GetComponent<PhotonView>().Owner);

				}
			}
			else
			{
				if (isdriving)
				{
					transform.SetParent(null);
					isdriving = false;
					carUserControl.enabled = false;
					moveScript.enableMove = true;
					moveScript.GetComponent<Rigidbody>().isKinematic = false;
					moveScript.GetComponentInChildren<Collider>().isTrigger = false;
					transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
				}
			}
		}

	}
}
