using System.Collections.Generic;

public class Pattern{
	public long startTime = 0;
	readonly List<MusicEvent> events;
	readonly EventIterator iterator;
	public bool started = false;
	const int LookAhead = 3000;
	public void Play(long time){
		started = true;
		startTime = time;
		iterator.start ();
	}

	public void Stop(){
		started = false;
	}



	public Pattern (List<MusicEvent> events)
	{
		this.events = events;
		events.Add (new MusicEvent (MusicEventTypes.End, events [events.Count - 1].startTime + 4000));
		this.iterator = new EventIterator (events);
//		events [events.Count - 1].isLastEvent = true;
	}

	public bool isFinished(){
		return iterator.peek () == null;
	}



	public void Process(long absTime, HandleEvent handler){
		long songTime = absTime - startTime;
		MusicEvent e = iterator.peek ();
		if (e != null && songTime + LookAhead > e.startTime) {
			//			Debug.Log (e);
			iterator.poll ();
			handler (e);
		} 
	}

}
