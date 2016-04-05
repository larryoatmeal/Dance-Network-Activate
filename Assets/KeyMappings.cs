using System;

using System.Collections.Generic;
static public class KeyMappings
{
	public static Dictionary<StandardKeyCodes, StandardControls> map = new Dictionary<StandardKeyCodes, StandardControls> () {
		{ StandardKeyCodes.D, StandardControls.LEFT },
		{ StandardKeyCodes.F, StandardControls.DOWN },
		{ StandardKeyCodes.J, StandardControls.UP },
		{ StandardKeyCodes.K, StandardControls.RIGHT }
	};
	public static Dictionary<StandardControls, StandardKeyCodes> reverseMap = new Dictionary<StandardControls, StandardKeyCodes>(){
		{ StandardControls.LEFT, StandardKeyCodes.D},
		{ StandardControls.DOWN, StandardKeyCodes.F},
		{ StandardControls.UP, StandardKeyCodes.J},
		{ StandardControls.RIGHT, StandardKeyCodes.K}
	};

	public static StandardControls keyToControl(StandardKeyCodes key){
		return map[key];
	}
	public static StandardKeyCodes controlToKey(StandardControls control){
		return reverseMap[control];
	}
}

