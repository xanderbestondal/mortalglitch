using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.Networking;
using System.Globalization;

public class saveData : MonoBehaviour
{
	//public TextMeshProUGUI debug;

	public string username;

	string sendstring;
	int timer;

	public Transform camRot;
	public Transform campos;

	string appUrl;

	private void Start()
	{
		appUrl = Application.dataPath;
		
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0, 50, 100, 100), appUrl);
	}

	public void updatePosRotMessage()
	{

		sendstring += Time.time.ToString() + ";";
		//save player pos data
		sendstring += transform.position.ToString("F2") + ";";
		sendstring += transform.rotation.y.ToString() + ";";
		//save cam data
		sendstring += campos.localPosition.z.ToString() + ";";
		sendstring += camRot.rotation.x.ToString() + "\n";
		timer++;

		if (timer == 100)
		{ // every 100 frames: send string with new positions
			StartCoroutine(Upload("posrotdata_" + username, sendstring));
			sendstring = "";
			timer = 0;
		}

	}

	public void appendFileAndSend(string filename , string data)
	{
		StartCoroutine(Upload(filename, data));
	}


	IEnumerator Upload(string filename,  string data)
	{
		WWWForm form = new WWWForm();
		form.AddField("filename", filename);
		form.AddField("data", data + "\n");
		
		UnityWebRequest www = UnityWebRequest.Post(appUrl+"/userdata/postText.php", form);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			//Debug.Log("Form upload complete!");
		}
	}
	
}
