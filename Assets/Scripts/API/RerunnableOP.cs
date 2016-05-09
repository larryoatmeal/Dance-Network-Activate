using System;
using System.Collections.Generic;
using UnityEngine;
using LRUCache;
using System.Collections;

public abstract class RerunnableOP<K, V>{
	List<K> activeDownloads = new List<K>();
	Dictionary<K, List<System.Action<V>>> awaitingCallbacks = new Dictionary<K, List<System.Action<V>>> ();
	LRUCache<K, V> cache;
	MonoBehaviour owner;
	public RerunnableOP (LRUCache<K, V> cache, MonoBehaviour owner)
	{
		this.cache = cache;
		this.owner = owner;
	}

	void executeQueuedCallbacks(K key, V value){
		foreach(var c in awaitingCallbacks[key]){
			c (value);
		}
		awaitingCallbacks [key].Clear ();
	}

	public void run(K key, System.Action<V> callback){
		if (!awaitingCallbacks.ContainsKey (key)) {
			awaitingCallbacks [key] = new List<System.Action<V>> ();
			awaitingCallbacks [key].Add (callback);
		} else {
			awaitingCallbacks [key].Add (callback);
		}
		if (!activeDownloads.Contains (key)) {//don't download again, wait for previous download to finish and execute callback
			if (cache.contains (key)) {
				executeQueuedCallbacks(key, cache.get(key));
			} else {
				activeDownloads.Add (key);
				owner.StartCoroutine(download (key, result => {
					cache.add (key, result);
					activeDownloads.Remove(key);
					executeQueuedCallbacks(key, cache.get(key));
				}));
			}
		}
	}
	//you should override this
	protected abstract IEnumerator download(K key, System.Action<V> callback);
}