using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.Networking;
using System.Globalization;

public class getData : MonoBehaviour
{
	

	public string appUrl;
	string[] dataFiles;
	CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
	public Material linemat;
	private void Start()
	{
		appUrl = Application.dataPath;

#if UNITY_WEBGL
		StartCoroutine(Download(appUrl + "/userdata/getText.php"));
#endif

#if UNITY_EDITOR
		StartCoroutine(Download("http://nahshop.oisoi.com/userdata/getText.php"));
#endif

		ci.NumberFormat.CurrencyDecimalSeparator = ".";
	}


	IEnumerator Download(string uri)
	{

		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();

			//string[] pages = uri.Split('/');
			//int page = pages.Length - 1;

			if (webRequest.isNetworkError)
			{
				Debug.Log("Error: " + webRequest.error);
			}
			else
			{
				//Debug.Log("Received: " + webRequest.downloadHandler.data);


				dataFiles = webRequest.downloadHandler.text.Split(';');

				foreach (string s in dataFiles)
				{
					print(s);
					if(s!="	")
						StartCoroutine(GetText(s));
				}

				

			}
		}
	}

	IEnumerator GetText(string filename)
	{
		UnityWebRequest webRequest = UnityWebRequest.Get("http://nahshop.oisoi.com/userdata/" + filename);

		yield return webRequest.SendWebRequest();

		if (webRequest.isNetworkError)
		{
			Debug.Log(webRequest.isNetworkError);
		}
		else
		{
			GameObject objToSpawn = new GameObject(filename + "_plottedData");
			objToSpawn.transform.SetParent(transform);
			LineRenderer lr = objToSpawn.AddComponent<LineRenderer>();
			lr.material = linemat;
			lr.startWidth = .01f;
			lr.endWidth = .01f;
			lr.startColor = new Color(Random.value, Random.value, Random.value);


			// Show results as text
			//print(webRequest.downloadHandler.text);
			string[] lines = webRequest.downloadHandler.text.Split('\n');

			print("plotting " + lines.Length + " saved positions for '" + filename+"'");
			lr.positionCount = lines.Length;

			int i = 0;
			foreach (string s in lines)
			{
				if (s == "")
					continue;

				string pos = s.Split(';')[1];

				if (pos.StartsWith("(") && pos.EndsWith(")"))
				{
					pos = pos.Substring(1, pos.Length - 2);
				}

				string[] sArray = pos.Split(',');

				Vector3 result = new Vector3(
					float.Parse(sArray[0], NumberStyles.Any, ci),
					float.Parse(sArray[1], NumberStyles.Any, ci),
					float.Parse(sArray[2], NumberStyles.Any, ci));

				lr.SetPosition(i, result);

				i++;
			}

			// Or retrieve results as binary data
			//byte[] results = www.downloadHandler.data;
		}
	}
}
