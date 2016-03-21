using System;
using System.Collections.Generic;


public class EventComparer: IComparer<MusicEvent>
{
	public int Compare(MusicEvent first, MusicEvent second){
		return (int) (first.startTime - second.startTime);
	}
}

public delegate void HandleEvent(MusicEvent musicEvent);

public class EventIterator{
	int i = 0;

	List<MusicEvent> events;
	EventComparer comparer = new EventComparer();
	public EventIterator (List<MusicEvent> events)
	{
		this.events = events;
	}

	//start at this ms
	private int findAfter(long ms){
		int index = events.BinarySearch (MusicEvent.DummyEvent (ms), comparer);
		if (index < 0) {
			//BinarySearch returns the bitwise complement
			//of next largest if not found
			int nextLargest = ~index;
			return i = nextLargest;
		} else {
			return i = index;		
		}
	}

	public void seek(long ms){
		i = findAfter (ms);
	}

	public void start(){
		i = 0;
	}

	public MusicEvent peek(){
		if (i == events.Count) {
			return null;
		}
		return events [i];
	}

	public MusicEvent poll(){
		if (i < events.Count) {
			int j = i;//dummy
			i += 1;
			return events [j];
		} else {
			return null;
		}
	}

	public void process(long ms, long lookahead, HandleEvent handler){
		long end = lookahead + ms;
		while (true) {
			MusicEvent e = peek ();
			if (e != null && e.startTime < end) {
				handler (poll ());
			} else {
				break;
			}
		}
	}
}