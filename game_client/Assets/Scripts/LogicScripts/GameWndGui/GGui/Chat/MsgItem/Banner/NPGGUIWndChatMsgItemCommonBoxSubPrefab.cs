using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宝箱分享banner附加窗口
    /// </summary>
	public class NPGGUIWndChatMsgItemCommonBoxSubPrefab : _AGGUIWndChatMsgItemCommonBoxSubPrefabBase<NPGGUIMonoChatMsgItemCommonBoxSubPrefab>
	{
        public NPGGUIWndChatMsgItemCommonBoxSubPrefab(long _uiPathId, Transform _parent) : base(_uiPathId, _parent)
	    {
	    }

	    protected override void _onShowWndEx() { }

	    protected override void _onHideWndEx() { }

	    protected override void _onResetEx() { }

	    protected override void _onDiscardEx() { }

	    protected override void _onWndInitDoneEx() { }

		/// <summary>
		/// 设置信息
		/// </summary>
		/// <param name="_boxInfo"></param>
        public void setInfo(NPChatMsgCommonBoxInfo _boxInfo)
        {
			setBaseInfo(_boxInfo);
        }
    }
}