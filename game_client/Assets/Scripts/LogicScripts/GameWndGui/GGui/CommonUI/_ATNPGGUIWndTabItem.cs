using ALPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 页签选择对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class _ATNPGGUIWndTabItem<T> : _ATNPBasicSimpleUISubWnd<T> where T : NPGGUIMonoCommonTab
    {
        protected Action<bool> _m_dClickDelegate;
        protected Action _m_onUnableClickDelegate;//在不可用时, 点击触发事件
        
        protected NPGGUIWndCommonRedTip _m_wRedTipWnd;//红点
        protected bool _m_bIsOn;//当前选中状态

        //是否有效
        protected bool _m_bIsEnable;

        /// <summary>
        /// 初始尺寸
        /// </summary>
        protected RectTransform _m_rtSizeTrans = null;
        protected Vector2 _m_vInitSize = Vector2.zero;

        public bool isEnable { get { return _m_bIsEnable; } }
        public bool isOn { get { return _m_bIsOn; } }
        public NPGGUIWndCommonRedTip redTipWnd { get { return _m_wRedTipWnd; } }
        public Action<bool> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }
        public Action onUnableClickDelegate { get{return _m_onUnableClickDelegate;} set{_m_onUnableClickDelegate = value;} }


        public _ATNPGGUIWndTabItem(T _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd() { }

        protected override void _onHideWnd() { }


        protected override void _onReset()
        {
            if(wnd == null)
                return;

            if(_m_wRedTipWnd != null)
                _m_wRedTipWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(_m_wRedTipWnd != null)
                _m_wRedTipWnd.discard();
            _m_wRedTipWnd = null;

            _m_dClickDelegate = null;
            _m_onUnableClickDelegate = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickSelectButton);

            if(wnd.monoRedTip != null)
                _m_wRedTipWnd = new NPGGUIWndCommonRedTip(wnd.monoRedTip);

            //获取窗口对象尺寸
            _m_rtSizeTrans = wnd.GetComponent<RectTransform>();
            if(null != _m_rtSizeTrans)
            {
                _m_vInitSize = _m_rtSizeTrans.sizeDelta;
            }

            //默认有效
            _m_bIsEnable = false;
            setEnable(true);
        }

        //设置选中状态
        public virtual void setSelected(bool _isSelect)
        {
            //无效无法操作
            if(null == wnd || !_m_bIsEnable)
                return;

            _m_bIsOn = _isSelect;

            //根据选中与否顺序有所不同
            if(_isSelect)
            {
                ALUGUICommon.setGameObjEnable(wnd.isOffShow, false);
                ALUGUICommon.setGameObjEnable(wnd.isOnShow, true);

                //根据尺寸
                if(wnd.selectChgSizeScale)
                    ALUGUICommon.setUIObjSize(_m_rtSizeTrans, _m_vInitSize);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.isOnShow, false);
                ALUGUICommon.setGameObjEnable(wnd.isOffShow, true);

                //根据尺寸
                if(wnd.selectChgSizeScale)
                    ALUGUICommon.setUIObjSize(_m_rtSizeTrans, _m_vInitSize * wnd.noSelectSizeScale);
            }
        }

        /// <summary>
        /// 设置有效
        /// </summary>
        public void setEnable(bool _enable)
        {
            if(null == wnd || _m_bIsEnable == _enable)
                return;

            _m_bIsEnable = _enable;

            //设置显示清空
            if(_m_bIsEnable)
            {
                //有效则隐藏无效显示
                ALUGUICommon.setGameObjEnable(wnd.disableShow, false);
                GGameCommonInfo.grayImage(wnd.disableGrayList, false);

                setSelected(_m_bIsOn);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.isOffShow, false);
                ALUGUICommon.setGameObjEnable(wnd.isOnShow, false);

                //无效则显示无效显示
                ALUGUICommon.setGameObjEnable(wnd.disableShow, true);
                GGameCommonInfo.grayImage(wnd.disableGrayList, true);//无效置灰
            }
        }

        //选择种族
        protected virtual void _onClickSelectButton(GameObject _go)
        {
            if (wnd == null)
                return;

            if (!_m_bIsEnable)
            {
                _m_onUnableClickDelegate?.Invoke();
                return;
            }

            if(null != _m_dClickDelegate)
            {
                _m_dClickDelegate(!_m_bIsOn);
            }
        }

        //显示隐藏小红点
        public void showRedTipNum(int _num)
        {
            if(_m_wRedTipWnd != null)
                _m_wRedTipWnd.showRedTipNum(_num);
        }
    }
}
