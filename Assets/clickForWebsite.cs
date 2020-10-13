using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class clickForWebsite : MonoBehaviour
{

	[DllImport("__Internal")]
	private static extern void openWindow(string url);

	public void OpenLinkJSPlugin()
	{
#if !UNITY_EDITOR
		openWindow("https://nahstuff.bandcamp.com/album/mortal-glitch");
#endif
	}

	private void OnMouseDown()
	{
		//Application.OpenURL("https://nahstuff.bandcamp.com/");
		OpenLinkJSPlugin();
		gameObject.SetActive(false);
	}
}
