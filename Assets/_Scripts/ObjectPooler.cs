using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour 
{
	public static ObjectPooler Instance;

	public List<GameObject> pooledObjects;
	public GameObject objectToPool, objectsParent;
	public int amountToPool;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < amountToPool; i++) 
		{
			GameObject obj = (GameObject)Instantiate (objectToPool);
			obj.SetActive (false);
			pooledObjects.Add (obj);
			obj.transform.parent = objectsParent.transform;
            obj.transform.position = objectsParent.transform.position;
        }
	}

	public GameObject GetPooledObject()
	{
		for (int i = 0; i < pooledObjects.Count; i++) 
		{
			if (!pooledObjects[i].activeInHierarchy) 
			{
				return pooledObjects [i];
			}
		}
		return null;
	}
}
