using System;
using UnityEngine;
public abstract class Singleton<T>: MonoBehaviour where T: Singleton<T>{
	static T instance;

	bool persist = false;

	public bool Persist {
		get { return persist; }
		protected set { persist = value; }

	}
		
	public static T Instance {
		get {
			// This would only EVER be null if some other MonoBehavior requests the instance
			// in its' Awake method.
			if(instance == null) {
//				Debug.Log("[UnitySingleton] Finding instance of '" + typeof(T).ToString() + 
//					"' object.");
				instance = FindObjectOfType(typeof(T)) as T;
				// This should only occur if 'T' hasn't been attached to any game
				// objects in the scene.
				if(instance == null) {
//					Debug.LogError("[UnitySingleton] No instance of " + typeof(T).ToString()
//						+ "found!");
					return null;
				}

				instance.Init();
			}
			return instance;
		}
	}

	void Awake() {
//		Debug.Log("[UnitySingleton] Awake");
		if(instance == null) {
//			Debug.Log("[UnitySingleton] Initializing Singleton in Awake");
			instance = this as T;
			instance.Init();
			if(persist)
				DontDestroyOnLoad(gameObject);
		}
	}

	virtual protected void Init() { }

	void OnApplicationQuit() {
		instance = null;
	}



}