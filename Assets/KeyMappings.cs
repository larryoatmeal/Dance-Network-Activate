using System;

using System.Collections.Generic;
static public class KeyMappings
{
	public static Dictionary<int, MusicEventTypes> map = new Dictionary<int, MusicEventTypes> () {
		{ RealtimeInputListener.A, MusicEventTypes.Down },
		{ RealtimeInputListener.S, MusicEventTypes.Up },
		{ RealtimeInputListener.D, MusicEventTypes.Left },
		{ RealtimeInputListener.F, MusicEventTypes.Right }
	};

	public static MusicEventTypes keyToEvent(int key){
		return map[key];
	}
}

