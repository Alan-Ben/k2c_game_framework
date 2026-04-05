using System;
using UnityEngine;
using ALPackage;
namespace GOE
{
    /// <summary>
    /// 自带不同显示状态的按钮
    /// </summary>
    public abstract class _ATNPGGUIWndStateBtn<_T_ENUM, _T_MONO> : _ANPGGUIBasicSubWnd<_T_MONO>
        where _T_ENUM : Enum
        where _T_MONO : _ATNPGGUIMonoStateBtn<_T_ENUM>
    {
        private _T_ENUM _m_eCurState;//当前按钮状态

        protected _ATNPGGUIWndStateBtn(_T_MONO _mono) : base(_mono)
        {

        }

        /// <summary> 点击回调 </summary>
        public event Action<_T_ENUM> onClick;
        /// <summary> 当前按钮状态 </summary>
        public _T_ENUM curState { get => _m_eCurState; }

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
            onClick = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBtn);
        }


        #region 点击事件

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="_go"></param>
        protected virtual void _onClickBtn(GameObject _go)
        {
            onClick?.Invoke(_m_eCurState);
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 刷新
        /// </summary>
        protected virtual void _refreshWnd()
        {
            if (wnd == null)
                return;

            if (wnd.btnStateShowParamList != null)
            {
                NPGGUIBtnStateShowParam<_T_ENUM> finalShow = null;
                for (int i = 0; i < wnd.btnStateShowParamList.Count; i++)
                {
                    NPGGUIBtnStateShowParam<_T_ENUM> temp = wnd.btnStateShowParamList[i];
                    if (temp == null)
                        continue;
                    if (temp.type.Equals(_m_eCurState))
                        finalShow = temp;
                    else
                        ALUGUICommon.setGameObjEnable(temp.goList, false);
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
        public void setState(_T_ENUM _state)
        {
            _m_eCurState = _state;
            _refreshWnd();
        }

        #endregion

    }

    public abstract class _ATNPGGUIWndStateBtn<_T_ENUM, _T_MONO, _T_SELF> : _ATNPGGUIWndStateBtn<_T_ENUM, _T_MONO>
        where _T_ENUM : Enum
        where _T_MONO : _ATNPGGUIMonoStateBtn<_T_ENUM>
        where _T_SELF : _ATNPGGUIWndStateBtn<_T_ENUM, _T_MONO, _T_SELF>
    {
        protected _ATNPGGUIWndStateBtn(_T_MONO _mono) : base(_mono)
        {

        }

        /// <summary> 点击回调 </summary>
        public event Action<_T_SELF> onClickBtn;

        protected override void _onClickBtn(GameObject _go)
        {
            base._onClickBtn(_go);
            onClickBtn?.Invoke((_T_SELF)this);
        }

        /// <summary>
        /// 释放函数需要补全变量释放处理
        /// </summary>
        protected override void _onDiscard()
        {
            onClickBtn = null;

            base._onDiscard();
        }
    }
}
