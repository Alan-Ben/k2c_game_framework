using System;
using UnityEngine;
using ALPackage;
namespace GOE
{
    /// <summary>
    /// 不同状态显示GO
    /// </summary>
    public abstract class _ATNPGGUIWndStateShow<_T_STATE_ENUM, _T_STATE_SHOW_PARAM_MONO, _T_ITEM_MONO> : _ATALUGUIBasicGridItemWnd<_T_ITEM_MONO>
        where _T_STATE_ENUM : Enum
        where _T_STATE_SHOW_PARAM_MONO : _ATNPGGUIStateShowParam<_T_STATE_ENUM>
        where _T_ITEM_MONO : _ATNPGGUIMonoStateShow<_T_STATE_ENUM, _T_STATE_SHOW_PARAM_MONO>
    {
        private _T_STATE_ENUM _m_eCurState;//当前按钮状态

        protected _ATNPGGUIWndStateShow(_T_ITEM_MONO _mono) : base(_mono)
        {

        }

        /// <summary> 当前按钮状态 </summary>
        public _T_STATE_ENUM curState { get => _m_eCurState; }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {

        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        #region 窗体事件

        /// <summary>
        /// 刷新不同状态显示
        /// </summary>
        protected virtual void _refreshStateShow()
        {
            if (wnd == null)
                return;

            if (wnd.diffStateShowParamList != null)
            {
                _T_STATE_SHOW_PARAM_MONO finalShow = null;
                for (int i = 0; i < wnd.diffStateShowParamList.Count; i++)
                {
                    _T_STATE_SHOW_PARAM_MONO stateShowParam = wnd.diffStateShowParamList[i];
                    if (stateShowParam == null)
                        continue;
                    if (stateShowParam.type.Equals(_m_eCurState))
                        finalShow = stateShowParam;
                    else
                        ALUGUICommon.setGameObjEnable(stateShowParam.goList, false);
                }
                if (finalShow != null)
                    ALUGUICommon.setGameObjEnable(finalShow.goList, true);
            }
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置当前状态
        /// </summary>
        /// <param name="_state"></param>
        public void setState(_T_STATE_ENUM _state)
        {
            _m_eCurState = _state;
            _refreshStateShow();
        }

        #endregion

    }
}
