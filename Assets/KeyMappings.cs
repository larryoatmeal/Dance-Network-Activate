using System;

using System.Collections.Generic;
static public class KeyMappings
{
	public static Dictionary<int, MusicEventTypes> map = new Dictionary<int, MusicEventTypes> () {
		{ StandardKeyCodes.A, MusicEventTypes.Down },
		{ StandardKeyCodes.S, MusicEventTypes.Up },
		{ StandardKeyCodes.D, MusicEventTypes.Left },
		{ StandardKeyCodes.F, MusicEventTypes.Right }
	};

	public static MusicEventTypes keyToEvent(int key){
		return map[key];
	}
}

