using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_manager : MonoBehaviour
{

	public GameObject cross;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonUp(0))
			cross.SetActive(false);
		if (Input.GetMouseButtonDown(0))
			cross.SetActive(true);
	}
	private void OnGUI()
	{

		GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
		GUI.Label(new Rect(0, 100, 100, 100), Time.timeSinceLevelLoad.ToString());

	}

	//onmou
}
