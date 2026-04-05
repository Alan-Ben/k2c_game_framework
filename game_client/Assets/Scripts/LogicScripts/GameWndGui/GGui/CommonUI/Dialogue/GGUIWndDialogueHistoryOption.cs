using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 对话历史回顾选项窗口
    /// </summary>
    public class GGUIWndDialogueHistoryOption : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoDialogueHistoryOption>
    {
        private long _m_lResPathId;//资源路径id
        private GGUIWndDialogueHistoryOptionContainer _m_wOptionContainer;//选项列表

        public GGUIWndDialogueHistoryOption(long _resPathId, Transform _parent)
            : base(_parent)
        {
            _m_lResPathId = _resPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wOptionContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOptionContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wOptionContainer?.discard();
            _m_wOptionContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoOptionContainer != null)
                _m_wOptionContainer = new GGUIWndDialogueHistoryOptionContainer(wnd.monoOptionContainer);
        }


        public void setInfo(List<NPDialogueResponseOptionRefObj> _optionList, int _selectIndex)
        {
            if (wnd == null || _optionList == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, NPPlayer.instance.playerInfo.PlayerName);
            if (_m_wOptionContainer != null)
            {
                _m_wOptionContainer.showWnd();
                _m_wOptionContainer.showItemList(_optionList, _selectIndex);
            }
        }
    }
}