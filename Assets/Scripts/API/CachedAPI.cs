using System;
using UnityEngine;
using LRUCache;
using System.Collections;
public class CachedAPI
{
	LRUCache<string, Texture2D> textureCache = new LRUCache<string, Texture2D>(20);


	public CachedAPI ()
	{
		
	}

	public IEnumerator downloadAndCreateTexture(string imagePath, System.Action<Texture2D> callback){

		if (textureCache.contains (imagePath)) {
			Debug.LogFormat ("Cache contains {0}", imagePath);
			callback (textureCache.get(imagePath));
			yield return null;
		} else {
			Debug.LogFormat ("Cache does not contain {0}", imagePath);
				
			yield return API.downloadAndCreateTexture (imagePath, texture => {
				textureCache.add (imagePath, texture);
				callback (texture);
			});
		}
	}

}


