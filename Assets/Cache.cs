using UnityEngine;
using System.Collections;
using LRUCache;
public class Cache : MonoBehaviour {


	public LRUCache<string, Texture2D> textureCache = new LRUCache<string, Texture2D>(15);


}
