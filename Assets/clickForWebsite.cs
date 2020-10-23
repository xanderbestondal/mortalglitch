using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class clickForWebsite : MonoBehaviour
{

	AudioSource ass;

	private void Start()
	{
		ass = GetComponent<AudioSource>();
	}
	private void OnEnable()
	{
		StartCoroutine(playSoundDelayed());
	}

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
		ass.Play();
	}


	private IEnumerator playSoundDelayed()
	{
		yield return new WaitForSeconds(0.5f);
		ass.Play();
	}
}
