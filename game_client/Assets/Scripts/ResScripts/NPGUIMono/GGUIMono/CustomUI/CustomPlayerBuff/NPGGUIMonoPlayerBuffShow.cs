using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

#if NP_GAME
using GOE;
#endif


namespace GOE
{
    /// <summary>
    /// buff展示自定义脚本
    /// </summary>
	public class NPGGUIMonoPlayerBuffShow : _AALBasicUIWndMono
	{
	    public Text leftTimeText;
	    public int buffID;

	    //当前显示的时间
	    private int _m_iLeftTimeS;

#if NP_GAME
	    //当前显示的buff对象
	    private NPPlayerBuffInfo _m_pbPlayerBuf = null;
#endif

	    public void Update()
	    {
#if NP_GAME
	        //获取用户Buff
	        if(null == _m_pbPlayerBuf)
	            _m_pbPlayerBuf = NPPlayer.instance.playerBuffComp.lookup(buffID);

	        if(null != _m_pbPlayerBuf)
	        {
	            if(_m_iLeftTimeS == _m_pbPlayerBuf.curLeftTimeMS)
	                return;

	            _m_iLeftTimeS = (int)(_m_pbPlayerBuf.curLeftTimeMS / 1000);
	            //刷新时间
	            ALUGUICommon.setLabelTxt(leftTimeText, _m_iLeftTimeS);

	            //超出时间则设置空
	            if(0 == _m_iLeftTimeS)
	                _m_pbPlayerBuf = null;
	        }
#endif
	    }
	}
}