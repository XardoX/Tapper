using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectPooler : MonoBehaviour 
{
	public static ObjectPooler Instance;
	public ObjectsToPool[] objectsToPool;
	

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		foreach(ObjectsToPool o in objectsToPool)
		{
			o.pooledObjects = new List<GameObject> ();
			for (int i = 0; i < o.amountToPool; i++) 
			{
				GameObject obj = (GameObject)Instantiate (o.objectToPool);
				obj.SetActive (false);
				o.pooledObjects.Add (obj);
				obj.transform.parent = o.objectsParent.transform;
				obj.transform.position = o.objectsParent.transform.position;
			}
        }
	}

	public GameObject GetPooledObject(int objectID)
	{
		for (int i = 0; i < objectsToPool[objectID].pooledObjects.Count; i++) 
		{
			if (!objectsToPool[objectID].pooledObjects[i].activeInHierarchy) 
			{
				return objectsToPool[objectID].pooledObjects [i];
			}
		}
		return null;
	}
}
[System.Serializable]
public class ObjectsToPool
{
	public GameObject objectToPool, objectsParent;
	public int amountToPool;
	[ReadOnly]
	public List<GameObject> pooledObjects;
}