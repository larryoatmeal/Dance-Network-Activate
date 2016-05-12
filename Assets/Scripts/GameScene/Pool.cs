using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Pool : MonoBehaviour {

	HashSet<GameObject> objsActive = new HashSet<GameObject>();
	Queue<GameObject> objsInactive = new Queue<GameObject>();

	public int initialCount = 20;
	public GameObject objPrefab;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < initialCount; i++) {
			//			GameObject obj = Instantiate (objPrefab).GetComponent<GameObject>();
			//			obj.transform.parent = transform;
			//
			//			obj.gameObject.SetActive (false);
			objsInactive.Enqueue (createGameObject());
		}
	}

	GameObject createGameObject(){
		GameObject obj = Instantiate (objPrefab);
		obj.transform.parent = transform;

		obj.gameObject.SetActive (false);
		return obj;
	}

	public GameObject Commission(){
		if (objsInactive.Count > 0) {
			GameObject obj = objsInactive.Dequeue ();
			objsActive.Add (obj);
			obj.gameObject.SetActive (true);

			return obj;
		} else {
			GameObject obj = createGameObject ();
			objsActive.Add (obj);
			obj.gameObject.SetActive (true);
			return obj;
		}
	}

	public void Decommision(GameObject obj){
		obj.gameObject.SetActive (false);
		objsActive.Remove (obj);
		objsInactive.Enqueue (obj);
	}

	// Update is called once per frame
	void Update () {

	}
}
