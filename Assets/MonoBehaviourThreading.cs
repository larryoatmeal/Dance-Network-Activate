using System;
using UnityEngine;
using System.Collections.Generic;


public delegate void Op();

public interface ThreadMessage{
	void execute (MonoBehaviourThreading caller);
}

public class MonoBehaviourThreading: MonoBehaviour
{
//	Queue<ThreadMessage> messages = new Queue<ThreadMessage>(); 
	Queue<Op> ops = new Queue<Op>();

	public virtual void Update()
	{
		if (ops.Count > 0) {
			ops.Dequeue()();		
		}
	}
//
//	public void callOnMainThread(ThreadMessage threadMessage){
//		messages.Enqueue (threadMessage);
//	}
	public void callOnMainThread(Op op){
		ops.Enqueue (op);
	}

}

