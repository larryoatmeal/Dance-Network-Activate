using System;

using System.Collections.Generic;
static public class KeyMappings
{
	public static Dictionary<StandardKeyCodes, StandardControls> map = new Dictionary<StandardKeyCodes, StandardControls> () {
		{ StandardKeyCodes.A, StandardControls.DOWN },
		{ StandardKeyCodes.S, StandardControls.UP },
		{ StandardKeyCodes.D, StandardControls.LEFT },
		{ StandardKeyCodes.F, StandardControls.RIGHT }
	};
	public static Dictionary<StandardControls, StandardKeyCodes> reverseMap = new Dictionary<StandardControls, StandardKeyCodes>(){
		{ StandardControls.DOWN, StandardKeyCodes.A},
		{ StandardControls.UP, StandardKeyCodes.S},
		{ StandardControls.LEFT, StandardKeyCodes.D},
		{ StandardControls.RIGHT, StandardKeyCodes.F}
	};

	public static StandardControls keyToControl(StandardKeyCodes key){
		return map[key];
	}
	public static StandardKeyCodes controlToKey(StandardControls control){
		return reverseMap[control];
	}
}

