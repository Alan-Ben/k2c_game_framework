using System;
using ALPackage;
using UnityEngine;


namespace GOE
{
	public abstract class NPBaseBtnClickSound : MonoBehaviour
	{
		[ALHeader("音效资源id")]
	    public long soundResID = 0;
	    [ALHeader("是否播放")]
	    public bool playSound = true;

	    public abstract bool IsPlaySound { get; }
	    public abstract long GetSoundResID { get; }
	}
}