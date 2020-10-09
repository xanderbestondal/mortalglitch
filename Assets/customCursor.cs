using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customCursor : MonoBehaviour
{

	Vector2 pos;

    // Start is called before the first frame update
    void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;

	}

	// Update is called once per frame
	void Update()
    {
		//transform.GetComponent<RectTransform>().Translate(Input.GetAxis("Mouse X") *Time.deltaTime * 400 , Input.GetAxis("Mouse Y") * Time.deltaTime * 400, 0);
		pos += new Vector2(Input.GetAxis("Mouse X") * Time.deltaTime * 400, Input.GetAxis("Mouse Y") * Time.deltaTime * 400);

		if (pos.x > Screen.width / 2)
			pos.x = Screen.width / 2;
		if (pos.x < -Screen.width / 2)
			pos.x = -Screen.width / 2;
		if (pos.y > Screen.height / 2)
			pos.y = Screen.height / 2;
		if (pos.y < -Screen.height / 2)
			pos.y = -Screen.height / 2;
		transform.GetComponent<RectTransform>().localPosition = pos;
	}
}
