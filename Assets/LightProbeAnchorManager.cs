using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightProbeAnchorManager : MonoBehaviour
{


	public List<GameObject> objectsToShift;

	public bool makeForEachChildRenderer;
	public Vector3 offsetAnchor;

	Renderer[] childRenderers;
	Transform[] newAnchors;
	Transform[] renderTransforms;

	int count = 0;




	private void OnEnable()
	{
		GameObject player = FindObjectOfType<Com.Oisoi.NahShop.GameManager>().instantiatedPlayer;
		print(player);
		if (player != null)
		{
			if (!objectsToShift.Contains(player))
			{
				objectsToShift.Add(player);
			}
		}
		foreach (GameObject go in objectsToShift)
		{
			count += go.GetComponentsInChildren<Renderer>().Length;
		}

		childRenderers = new Renderer[count];
		newAnchors = new Transform[count];
		renderTransforms = new Transform[count];
		

		int i = 0;
		foreach (GameObject go in objectsToShift)
		{
			foreach (Renderer r in go.GetComponentsInChildren<Renderer>())
			{
				childRenderers[i] = r;
				renderTransforms[i] = r.GetComponent<Transform>();
				newAnchors[i] = new GameObject("lightProbeAnchor").transform;
				newAnchors[i].SetParent(renderTransforms[i]);
				newAnchors[i].position = renderTransforms[i].position + offsetAnchor;
				r.probeAnchor = newAnchors[i];
				i++;
			}
		}
    }


	private void OnDisable()
	{

		foreach (GameObject go in objectsToShift)
		{
			foreach (Renderer r in go.GetComponentsInChildren<Renderer>())
			{
				print("destroying " + r.probeAnchor.gameObject);
				Destroy(r.probeAnchor.gameObject);
				r.probeAnchor = null; // probably not nessecary since its destroyed already
			}
		}

	}


	// Update is called once per frame
	void Update()
    {
		//for (int i = 0; i < count; i++)
		//{
		//	newAnchors[i].position = childRenderers;
		//}
    }
}
