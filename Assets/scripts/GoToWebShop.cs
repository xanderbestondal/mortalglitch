using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWebShop : MonoBehaviour
{

	public Transform clickObject;



	public void enableCasetteMenu()
	{
		clickObject.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
	}


	public void disableCasetteMenu()
	{
		clickObject.gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
	}

}
