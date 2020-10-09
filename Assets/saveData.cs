using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.Networking;

public class saveData : MonoBehaviour
{
	//public TextMeshProUGUI debug;

	public string username;

	string sendstring;
	int timer;
	


	// Start is called before the first frame update
	void Start()
    {
		
		
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void updatePosRotMessage()
	{

		sendstring += Time.time.ToString();
		sendstring += transform.position.ToString();
		sendstring += transform.position.ToString() + ";";
		timer++;

		if (timer == 100)
		{ // every 100 frames: send string with new positions
			StartCoroutine(Upload("posrotdata", sendstring));
			sendstring = "";
			timer = 0;
		}

	}

	public void updateChatMessage(string s)
	{
		StartCoroutine(Upload("chatMessages", s));
	}


	IEnumerator Upload(string dataType,  string data)
	{
		WWWForm form = new WWWForm();
		form.AddField("username", username + "_"  + dataType);
		form.AddField("data", data);
		
		UnityWebRequest www = UnityWebRequest.Post("https://nahshop.oisoi.com/userdata/postText.php", form);
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
