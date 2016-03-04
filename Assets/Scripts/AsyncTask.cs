using System;
using System.Threading;
using UnityEngine;

public abstract class AsyncTask{
	private MonoBehaviourThreading caller;

	public AsyncTask(MonoBehaviourThreading caller){
		this.caller = caller;
	}

	//This runs in backbround
	abstract public void process ();

	public void start(){
		Thread thread = new Thread (new ThreadStart(process));
		thread.Start ();
	}

	//call notifyProgress to execute onProgressOp on main thread
	abstract public void onProgressOp (object progress);


	public void notifyProgress(int progress){
//		caller.callOnMainThread (onProgressOp);
		caller.callOnMainThread (new Lauren (onProgressOp, progress));
	}


	//call notifyComplete to execute onCompleteOn on main thread
	abstract public void onCompleteOp ();

	public void notifyComplete(){
		Debug.Log ("Calling notify complete");
		caller.callOnMainThread (onCompleteOp);
	}


}