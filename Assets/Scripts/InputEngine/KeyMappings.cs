using System;

using System.Collections.Generic;
static public class KeyMappings
{
	public static Dictionary<StandardKeyCodes, StandardControls> map = new Dictionary<StandardKeyCodes, StandardControls> () {
		{ StandardKeyCodes.LEFT, StandardControls.LEFT },
		{ StandardKeyCodes.DOWN, StandardControls.DOWN },
		{ StandardKeyCodes.UP, StandardControls.UP },
		{ StandardKeyCodes.RIGHT, StandardControls.RIGHT }
	};

	public static Dictionary<StandardControls, StandardKeyCodes> reverseMap = new Dictionary<StandardControls, StandardKeyCodes>(){
		{ StandardControls.LEFT, StandardKeyCodes.LEFT},
		{ StandardControls.DOWN, StandardKeyCodes.DOWN},
		{ StandardControls.UP, StandardKeyCodes.UP},
		{ StandardControls.RIGHT, StandardKeyCodes.RIGHT}
	};

	public static StandardControls keyToControl(StandardKeyCodes key){
		return map[key];
	}
	public static StandardKeyCodes controlToKey(StandardControls control){
		return reverseMap[control];
	}
}

