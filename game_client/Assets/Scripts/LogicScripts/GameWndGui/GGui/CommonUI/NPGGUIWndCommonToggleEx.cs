
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 直接控制显隐的toggle
    /// </summary>
    public class NPGGUIWndCommonToggleEx : _ATNPBasicSimpleUISubWnd<NPGGUIMonoCommonToggleEx>
    {
        protected Action<NPGGUIWndCommonToggleEx> _m_dClickDelegate;
        protected bool _m_bIsOn;//当前选中状态
    
        public Action<NPGGUIWndCommonToggleEx> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }
        /// <summary>
        /// 当前是否选中
        /// </summary>
        public bool isOn
        {
            get { return _m_bIsOn; }
        }

        public NPGGUIWndCommonToggleEx(NPGGUIMonoCommonToggleEx _wnd)
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

            //使用默认值设置选中状态
            setSelected(wnd.isOn, true, false);
        }

        protected override void _onDiscard()
        {
            _m_dClickDelegate = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickSelectButton);
            ALUGUICommon.uncombineBtnClick(wnd.btnAdditionClick, _onClickSelectButton);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _m_dClickDelegate = default(Action<NPGGUIWndCommonToggleEx>);

            //绑定进入按钮操作
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickSelectButton);
            ALUGUICommon.combineBtnClick(wnd.btnAdditionClick, _onClickSelectButton);

            //使用默认值设置选中状态
            setSelected(wnd.isOn, true, false);
        }

        /// <summary>
        /// 设置选中状态，可以播放对应动画
        /// </summary>
        /// <param name="_isSelect"></param>
        /// <param name="_force"></param>
        /// <param name="_showAni"></param>
        public virtual void setSelected(bool _isSelect, bool _force = false, bool _showAni = true)
        {
            if(null == wnd)
                return;

            if(_force || _m_bIsOn != _isSelect)
            {
                _m_bIsOn = _isSelect;
                ALUGUICommon.setGameObjEnable(wnd.isOnShow, _isSelect);
                ALUGUICommon.setGameObjEnable(wnd.isOffShow, !_isSelect);

                if(null == wnd.selectAnimation)
                    return;

                //是否播放动画
                if (_showAni)
                {
                    //播放动画时候要延迟处理隐藏，类似窗口hide动画处理过程
                    if (_isSelect)
                    {
                        ALUGUICommon.setGameObjEnable(wnd.isOffShow, true);
                        _dealSelectAniAction(wnd.selectAniName, () =>
                        {
                            if(null == wnd)
                                return;
                            if(_m_bIsOn)
                                ALUGUICommon.setGameObjEnable(wnd.isOffShow, false);                    
                        });
                    }
                    else
                    {
                        ALUGUICommon.setGameObjEnable(wnd.isOnShow, true);
                        _dealSelectAniAction(wnd.disSelectAniName, () =>
                        {
                            if(null == wnd)
                                return;
                            if(!_m_bIsOn)
                                ALUGUICommon.setGameObjEnable(wnd.isOnShow, false);
                        });
                    }   
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.isOffShow, !_m_bIsOn);
                    ALUGUICommon.setGameObjEnable(wnd.isOnShow, _m_bIsOn);
                }
            }
        }

        /// <summary>
        /// 设置选中状态，直接设置对应动画到最后一帧
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setState(bool _isSelect)
        {
            if (wnd == null)
                return;

            _m_bIsOn = _isSelect;
            ALUGUICommon.setGameObjEnable(wnd.isOnShow, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.isOffShow, !_isSelect);

            if (_isSelect)
            {
                if(!string.IsNullOrEmpty(wnd.selectAniName))
                    wnd.selectAnimation.Sample(wnd.selectAniName,1);
            }
            else
            {
                if (!string.IsNullOrEmpty(wnd.disSelectAniName))
                    wnd.selectAnimation.Sample(wnd.disSelectAniName, 1);
            }
        }

        /***********
         * 在播放对应动作后执行指定函数
         **/
        private void _dealSelectAniAction(string _aniName, Action _aniDoneAction = null)
        {
            //处理动画，然后延迟处理函数
            if (null != wnd && null != wnd.selectAnimation)
            {
                AnimationClip clip = wnd.selectAnimation.GetClip(_aniName);
                if (null != clip)
                {
                    wnd.selectAnimation.ForcePlay(_aniName);

                    //延迟处理事件
                    ALCommonActionMonoTask.addMonoTask(_aniDoneAction, clip.length);
                }
                else
                {
                    if (null != _aniDoneAction)
                        _aniDoneAction();
                }
            }
            else
            {
                if (null != _aniDoneAction)
                    _aniDoneAction();
            }
        }

        //选择
        protected virtual void _onClickSelectButton(GameObject _go)
        {
            if(wnd == null)
                return;

            if(null != _m_dClickDelegate)
            {
                _m_dClickDelegate(this);
            }
        }

        public void clickSelectButton()
        {
            _onClickSelectButton(null);
        }
    }
}