using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

//using VRC.Udon;
//using VRC.SDK3.Components;
using Photon.Pun;

public class CloneOnSurface : MonoBehaviour {

	public GameObject[] scatterobjects;
	public bool[] dontRotate;

	public bool Y_isSideways;

	int maxRows_y;

	public GameObject[] allTaggedObjects;

	public void update()
	{

		int grabableObjectCount = 0;
		allTaggedObjects = GameObject.FindGameObjectsWithTag("grabable");
		foreach (GameObject go in allTaggedObjects)
		{

			//doPhotonStuff(go);

			//go.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			//go.GetComponent<Renderer>().receiveShadows = false	;
			//go.GetComponent<Renderer>().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			//go.GetComponent<Collider>().isTrigger = true;
			
			//go.st

			grabableObjectCount++;
		}

		print("UPDATED " + grabableObjectCount + " grabable objects");
	}
	public void generate()
	{

		foreach (Transform t in GetComponentsInChildren<Transform>())
		{
			if(t.gameObject != gameObject)
				DestroyImmediate(t.gameObject);
		}

		Bounds b = GetComponent<MeshFilter>().sharedMesh.bounds;
		
		int objindex = 0;
		int objCount_x = 20;

		float xpos = 0;
		float ypos = 0;
		float prevRow_endX = b.min.x;
		Bounds objBounds = new Bounds();
		
		for (int x = 0; x < objCount_x; x++)
		{

			//objindex = x % scatterobjects.Length;

			float prevRow_endY = b.min.y;
			prevRow_endY = b.max.y;
			for (int y = 0; y < 20; y++)
			{
				
				objindex = objindex % scatterobjects.Length;
				//int objindex = Random.Range(0,scatterobjects.Length);

				//print(objindex);
				GameObject go = Instantiate(scatterobjects[objindex], transform, true);
				makeUniqueName(go);// go.name + (x + y).ToString();



				doPhotonStuff(go);



				bool round = false;
				if (checkRoundness(go) < .01f)
					round = true;


				objBounds = go.GetComponent<MeshFilter>().sharedMesh.bounds;
				xpos = prevRow_endX + objBounds.size.x / Random.Range(2f, 2.5f);
				ypos = prevRow_endY - objBounds.max.y;// / Random.Range(1.8f, 2.5f);
				float zpos = b.max.z; // + gob.size.z * z + gob.size.z/2;

				// if beyond edge, stop this row.
				if (ypos < b.min.y + objBounds.size.y / 2)
				{
					DestroyImmediate(go);
					break;
				}



				go.transform.localPosition = new Vector3(xpos , ypos, zpos);

				if (round)
					go.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
				else
					go.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 180 + Random.Range(-5, 5));

				if (dontRotate[objindex])
				{
					go.transform.localRotation = Quaternion.identity;
				}

				prevRow_endY = ypos - objBounds.size.y / 2;

				// not working....
				// check if new object is overlapping any other shit
				if(checkColliderOverlap(go)) 
					DestroyImmediate(go);
				//....



				// randomly remove objects to get a more active nightshop vibe
				if (Random.value > .8)
				{
					DestroyImmediate(go);
					continue;
				}
			}

			if (Random.value > .6f)
				objindex++;

			prevRow_endX = xpos + objBounds.size.x / 2;
			if (xpos > b.max.x - objBounds.size.x)
				break;
		}

	}

	void doPhotonStuff(GameObject go)
	{

		PhotonView phtnVw = go.GetComponent<PhotonView>();
		//if(phtnVw == null)
		//	phtnVw = go.AddComponent<PhotonView>();

		PhotonTransformView phtnTfVw = go.GetComponent<PhotonTransformView>();
		//if(phtnTfVw == null)
		//	phtnTfVw = go.AddComponent<PhotonTransformView>();

		// TESTING IF ITS NOT CRASHING WITHOUT transform view
		if (phtnVw != null)
			DestroyImmediate(phtnVw);
		if (phtnTfVw != null)
			DestroyImmediate(phtnTfVw);

		//PhotonRigidbodyView phtnRbVw = go.AddComponent<PhotonRigidbodyView>();
		//phtnRbVw.m_SynchronizeAngularVelocity = true;
		//ObservedComponents.Add(phtnRbVw);

		//List<Component> ObservedComponents = new List<Component>();
		//ObservedComponents.Add(phtnTfVw);
		//phtnVw.ObservedComponents = ObservedComponents;
		//phtnVw.Synchronization = ViewSynchronization.Off;
		//phtnVw.OwnershipTransfer = OwnershipOption.Takeover;
	}

	float checkRoundness(GameObject go)
	{
		float roundnessDeviation = 0;
		Bounds gob = go.GetComponent<Renderer>().bounds;
		float startWidth = gob.size.x;
		float volume = gob.size.x + gob.size.y + gob.size.z;

		int steps = 8;
		for (int r = 0; r < steps; r++)
		{
			go.transform.localRotation = Quaternion.Euler(0, 0, (360f/ steps) * r);
			gob = go.GetComponent<Renderer>().bounds;

			roundnessDeviation += Mathf.Abs(startWidth - gob.size.x);
		}

		float deviationNormalised = (roundnessDeviation) / steps;
		//print(deviationNormalised);
		return deviationNormalised;
	}

	void makeUniqueName(GameObject go)
	{
		string currentName = go.name;
		int i = 0;
		while (GameObject.Find(currentName) != null)
		{
			currentName = go.name + i;
			i++;
		}
		go.name = currentName;
	}

	bool checkColliderOverlap(GameObject obj) {
		Collider colA = obj.GetComponent<Collider>();
		if (colA == null)
			return false; // no collider found, so no overlap detected...

		foreach (Collider col in GetComponents<Collider>()) {
			if (col == colA)
				continue;
			if (colA.bounds.Intersects(col.bounds))
			{
				return true;
			}
		}

		return false;
	}

}
