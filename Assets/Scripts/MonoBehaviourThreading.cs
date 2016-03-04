using System;
using UnityEngine;
using System.Collections.Generic;


public delegate void Op();

public delegate void Op2(object obj);

public class Lauren{

	Op2 op;
	object parameters;

	public Lauren(Op2 op, object parameters){
		this.op = op;
		this.parameters = parameters;
	}

	public void call(){
		op (parameters);
	}

}

public interface ThreadMessage{
	void execute (MonoBehaviourThreading caller);
}

public class MonoBehaviourThreading: MonoBehaviour
{
	Queue<Op> ops = new Queue<Op>();
	Queue<Lauren> laurens = new Queue<Lauren> ();


	public virtual void Update()
	{
		if (ops.Count > 0) {
			ops.Dequeue()();		
		}
		if (laurens.Count > 0){
			laurens.Dequeue ().call ();	
		}
	}

	public void callOnMainThread(Op op){
		ops.Enqueue (op);
	}

	public void callOnMainThread(Lauren lauren){
		laurens.Enqueue (lauren);
	}
}

