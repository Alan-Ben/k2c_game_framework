using ALPackage;
using UnityEngine;
using UnityEngine.UI;
using GOE;
using System.Collections.Generic;


namespace GOE
{
	/// <summary>
	/// 用于展示文字标题和描述Tip
	/// </summary>
	public class NPGGUICustomMonoShowTitleDesc : _ANPGGUIMonoCustomBasicWnd
	{
		[ALHeader("展示的文本标题Key")]
		public string showTitleKeyTxt;
		
		[ALHeader("展示的文本描述Key")]
		public string showDescKeyTxt;

		[ALHeader("跟随的物体")]
		public RectTransform followTrans;

		[ALHeader("点击按钮")]
		public GameObject clickGo;

	    [ALHeader("横向偏移值")]
	    public float offSetX;

	    [ALHeader("纵向偏移值")]
	    public float offSetY;
	    
	    [ALHeader("弹出的ui资源id")]
	    public long uiPathId = UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT;

#if NP_GAME
#endif

	    protected override void _onCustomUIEnable()
	    {
#if NP_GAME
	        ALUGUICommon.combineBtnClick(clickGo, _clickGoDidClick);
#endif
	    }

	    protected override void _onCustomUIDisable()
	    {
#if NP_GAME
	        ALUGUICommon.uncombineBtnClick(clickGo, _clickGoDidClick);

#endif
	    }

#if NP_GAME

	    /// <summary>
	    /// 点击按钮
	    /// </summary>
	    private void _clickGoDidClick(GameObject _go)
	    {
		    if (null == followTrans)
			    return;

	        QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(uiPathId, 
		        TextTranslate.instance.getLanguage(showTitleKeyTxt), 
		        TextTranslate.instance.getLanguage(showDescKeyTxt), 
		        followTrans,
		        offSetX, 
		        offSetY));
	    }

#endif
	}
}