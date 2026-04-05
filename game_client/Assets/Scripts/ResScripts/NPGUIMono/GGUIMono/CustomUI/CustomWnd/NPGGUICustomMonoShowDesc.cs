using ALPackage;
using UnityEngine;
using UnityEngine.UI;
using GOE;
using System.Collections.Generic;


namespace GOE
{
	/// <summary>
	/// 用于展示文字描述Tip
	/// </summary>
	public class NPGGUICustomMonoShowDesc : _ANPGGUIMonoCustomBasicWnd
	{
	    [ALHeader("展示的文本描述Key")]
	    public string showDescKeyTxt;

	    [ALHeader("点击按钮")]
	    public GameObject clickGo;

	    [ALHeader("是否横向跟随")]
	    public bool isFollow;

	    [ALHeader("横向偏移值")]
	    public float offSetX;

	    [ALHeader("纵向偏移值")]
	    public float offSetY;

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
	        long pathId = UIResPathConst.WIN_TOOL_TIP_TEXT;
	        if (isFollow)
	            pathId = UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW;

	        QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(UIResPathAssistant.getAssetPath(pathId), UIResPathAssistant.getObjName(pathId), TextTranslate.instance.getLanguage(showDescKeyTxt), gameObject.GetComponent<RectTransform>(), offSetX, offSetY));
	    }

#endif
	}
}