using System;
using System.Collections.Generic;
namespace LRUCache
{
	public class LRUCache<K,V>
	{
		private int capacity;
		private Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> cacheMap = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();
		private LinkedList<LRUCacheItem<K, V>> lruList = new LinkedList<LRUCacheItem<K, V>>();
		System.Action<V> destroyFunction;

		public LRUCache(int capacity, System.Action<V> destroyFunction = null)
		{
			this.capacity = capacity;
			this.destroyFunction = destroyFunction;
		}

		public V get(K key)
		{
			LinkedListNode<LRUCacheItem<K, V>> node;
			if (cacheMap.TryGetValue(key, out node))
			{
				V value = node.Value.value;
				lruList.Remove(node);
				lruList.AddLast(node);
				return value;
			}
			return default(V);
		}

		public bool contains(K key)
		{
			
			return cacheMap.ContainsKey(key);
		}


		public void add(K key, V val)
		{
			if (cacheMap.Count >= capacity)
			{
				RemoveFirst();
			}

			LRUCacheItem<K, V> cacheItem = new LRUCacheItem<K, V>(key, val);
			LinkedListNode<LRUCacheItem<K, V>> node = new LinkedListNode<LRUCacheItem<K, V>>(cacheItem);
			lruList.AddLast(node);
			cacheMap.Add(key, node);
		}

		private void RemoveFirst()
		{
			UnityEngine.Debug.LogFormat ("Removing {0}", lruList.First.Value.key);
			// Remove from LRUPriority
			LinkedListNode<LRUCacheItem<K,V>> node = lruList.First;
			lruList.RemoveFirst();

			// Remove from cache
			cacheMap.Remove(node.Value.key);
			if (destroyFunction != null) {
				destroyFunction (node.Value.value);
			}
		}
	}

	class LRUCacheItem<K,V>
	{
		public LRUCacheItem(K k, V v)
		{
			key = k;
			value = v;
		}
		public K key;
		public V value;
	}
}