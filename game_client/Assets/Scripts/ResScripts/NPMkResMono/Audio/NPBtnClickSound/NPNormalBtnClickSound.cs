using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GOE
{
	public class NPNormalBtnClickSound : NPBaseBtnClickSound
	{
	    public override long GetSoundResID { get { return soundResID; } }
	    
	    public override bool IsPlaySound { get { return playSound; } }
	}
}