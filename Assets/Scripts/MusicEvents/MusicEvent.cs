using System;

public class MusicEvent{
	public readonly MusicEventTypes eventType;

	public readonly long startTime;
	public readonly long endTime;

//	public bool isLastEvent = false;

	public MusicEvent (MusicEventTypes eventType, long startTime, long endTime)
	{
		this.eventType = eventType;
		this.startTime = startTime;
		this.endTime = endTime;
	}

	public MusicEvent (MusicEventTypes eventType, long startTime)
	{
		this.eventType = eventType;
		this.startTime = startTime;
		this.endTime = startTime;
	}
	public bool isHeldEvent(){
		return this.startTime != this.endTime;
	}

	public static MusicEvent DummyEvent(long startTime){
		return new MusicEvent (MusicEventTypes.Down, startTime);
	}

	public override string ToString ()
	{
		return string.Format ("[MusicEvent: eventType={0}, startTime={1}, endTime={2}, isHeldEvent={3}]", eventType, startTime, endTime, isHeldEvent());
	}
	
}