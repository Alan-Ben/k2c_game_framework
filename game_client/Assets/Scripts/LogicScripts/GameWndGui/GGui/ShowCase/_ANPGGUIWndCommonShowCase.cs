
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// showcase通用子窗口
    /// </summary>
    public abstract class _ANPGGUIWndCommonShowCase<T> : _ATALBasicUISubWnd<T>
        where T : GGUIMonoCommonShowCase
    {
        //RT图片
        protected NPGGuiWndTexture _m_wIconBodyWnd;
        //showcase 对象
        protected ShowcaseInfo _m_showcaseInfo;
        //显示序列号
        private int _m_showSerialize;

        //资源加载完成之后的回调管理器
        private ALCommonStateDelegate _m_dLoadedDelegate;
        
        public _ANPGGUIWndCommonShowCase(T _wnd)
            : base(_wnd)
        {
            _m_dLoadedDelegate = new ALCommonStateDelegate();
            
            initWnd();
        }

        public void showWnd(_AShowCaseUnitInfoObj _unit, NPGShowcaseIndex _templateIndex = null)
        {
            showWnd(new _AShowCaseUnitInfoObj[]{_unit}, _templateIndex);
        }
        
        public void showWnd(_AShowCaseUnitInfoObj[] _unitList, NPGShowcaseIndex _templateIndex = null)
        {
            if(null == wnd)
                return;
            base.showWnd();
            
            _initShowcase(_unitList, _templateIndex);
        }

        //初始化showcase
        protected void _initShowcase(_AShowCaseUnitInfoObj[] _unitList, NPGShowcaseIndex _templateIndex)
        {
            if(null == wnd)
                return;
            
            //舞台如果没传用wnd默认配置的
            NPGShowcaseIndex templateIndex = _templateIndex == null ? wnd.templateIndex : _templateIndex;
            
            //添加附加配置的go展示
            if (null != wnd.loadGoMonoList && wnd.loadGoMonoList.Count > 0 && null != _unitList)
            {
                //按加载位置从大到小排序
                wnd.loadGoMonoList.Sort((_a, _b) =>
                {
                    return -_a.loadIndex.CompareTo(_b.loadIndex);
                });

                //获取最大的位置
                int maxLength = Math.Max(wnd.loadGoMonoList[0].loadIndex + 1, _unitList.Length);

                //重新构造数组大小
                System.Array.Resize(ref _unitList, maxLength);
                
                //资源上配置的附加go展示
                foreach (NPGGUIMonoCommonShowCase_GoMono monoCommonShowCaseGoMono in wnd.loadGoMonoList)
                {
                    if(null == monoCommonShowCaseGoMono)
                        continue;

                    //按策划要求，这个位置传参是空的才允许用ui上配置的资源填充
                    if (_unitList[monoCommonShowCaseGoMono.loadIndex] == null)
                    {
                        _unitList[monoCommonShowCaseGoMono.loadIndex] = new ShowCaseCommonResUnitInfoObj(monoCommonShowCaseGoMono.goIndex);
                    }
                }
            }
            
            //获取showcase对象
            if (null == _m_showcaseInfo)
                _m_showcaseInfo = ShowCaseMgr.instance.popAvailableInfo();

            if (_m_showcaseInfo == null)
                return;

            //显示的资源改变了，重置加载完成回调
            if (null != _m_dLoadedDelegate)
                _m_dLoadedDelegate.reset();
            
            int serialize = _m_showSerialize = ALSerializeOpMgr.next();
            void onShowcaseLoaded()
            {
                if (serialize != _m_showSerialize)
                    return;

                _onShowcaseLoaded();

                //显示rt图片
                if (wnd != null && _m_wIconBodyWnd != null && wnd.cameraType == EShowcaseCameraType.RTCamera)
                {
                    _m_wIconBodyWnd.showWnd();
                    _m_wIconBodyWnd.setTexture(_m_showcaseInfo.getRenderTexture());
                }
                //执行效果
                if (null != wnd && null != wnd.initShowEffect)
                {
                    wnd.initShowEffect.dealEffect();
                }
            }
            ALUGUICommon.setGameObjEnable(wnd.goLoading, true);

            _m_showcaseInfo.setUIViewCenter(wnd.viewCenterRect);
            _m_showcaseInfo.enableShowCase(templateIndex, _unitList, wnd.cameraType, _getDescriptor());
            _m_showcaseInfo.regAllDoneDelegate(onShowcaseLoaded);
            
            //表现准备完成再去掉菊花
            _m_showcaseInfo.regShowPrepareDoneDelegate(() =>
            {
                if (wnd != null) ALUGUICommon.setGameObjEnable(wnd.goLoading, false);
            });
        }
        
        /// <summary>
        /// 输入要旋转的角度（degree），旋转整个showcase，绕着 Y 轴旋转
        /// </summary>
        public void rotateUnit(int _unitIndex, float _angle)
        {
            if (_m_showcaseInfo == null)
                return;

            _m_showcaseInfo.rotateUnit(_unitIndex, _angle);
        }

        /// <summary>
        /// 重置旋转角度
        /// </summary>
        public void resetRotationUnit(int _unitIndex)
        {
            if (_m_showcaseInfo == null)
                return;

            _m_showcaseInfo.resetRotationUnit(_unitIndex);
        }

        protected sealed override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            _disableAll();
            
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            if (null != _m_wIconBodyWnd)
                _m_wIconBodyWnd.discard();
            _m_wIconBodyWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnDragArea, _onClickShowcase);
            ALUGUICommon.uncombineBeginDrag(wnd.btnDragArea, _onBeginDragShowcase);
            ALUGUICommon.uncombineEndDrag(wnd.btnDragArea, _onEndDragShowcase);
            ALUGUICommon.uncombineDrag(wnd.btnDragArea, _onDragShowcase);
            
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _m_wIconBodyWnd = new NPGGuiWndTexture(wnd.imageBodyAndBG);
            
            ALUGUICommon.combineBtnClick(wnd.btnDragArea, _onClickShowcase);
            ALUGUICommon.combineBeginDrag(wnd.btnDragArea, _onBeginDragShowcase);
            ALUGUICommon.combineEndDrag(wnd.btnDragArea, _onEndDragShowcase);
            ALUGUICommon.combineDrag(wnd.btnDragArea, _onDragShowcase);
            
            _onWndInitDoneEx();
        }

        protected virtual void _onShowcaseLoaded()
        {
            //调用回调
            if(null != _m_dLoadedDelegate)
            {
                _m_dLoadedDelegate.setInitDone();
            }
        }
        
        /// <summary>
        /// 注册一个加载完成之后的回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regInitDoneDelegate(Action _delegate)
        {
            if(null == _m_dLoadedDelegate)
                _m_dLoadedDelegate = new ALCommonStateDelegate();
            
            _m_dLoadedDelegate.regDelegate(_delegate);
        }

        private void _disableAll()
        {
            _m_showSerialize = ALSerializeOpMgr.next();
            
            if (null != _m_showcaseInfo)
            {
                if (_m_showcaseInfo.TemplateMono != null)
                {
                    _m_showcaseInfo.TemplateMono.transform.localRotation = Quaternion.identity;
                }
                
                _m_showcaseInfo.disableShowCase();
                ShowCaseMgr.instance.pushBackInfo(_m_showcaseInfo);
                _m_showcaseInfo = null;
            }

            if (null != _m_wIconBodyWnd)
            {
                _m_wIconBodyWnd.discardTexture();
                _m_wIconBodyWnd.hideWnd();
            }
            
            if(null != _m_dLoadedDelegate)
                _m_dLoadedDelegate.reset();
        }
        
        /// <summary>
        /// 获取RT图片设置
        /// </summary>
        /// <returns></returns>
        private RenderTextureDescriptor _getDescriptor()
        {
            //用默认大小设置
            if (null == wnd || null == wnd.imageBodyAndBG || wnd.cameraType != EShowcaseCameraType.RTCamera || !wnd.isResizeRTByRect)
                return CommonQualityMgr.instance.getRTDescriptor();
            
            var rect = wnd.imageBodyAndBG.rectTransform.rect;
            RenderTextureDescriptor rt = CommonQualityMgr.instance.getRTDescriptor();
            float scale = CommonQualityMgr.instance.screenRTScaleFactor;
            rt.width = (int) (rect.width * scale);
            rt.height = (int) (rect.height * scale);
            return rt;
        }
        
        /// <summary>
        /// 点击showcase
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickShowcase(GameObject _go)
        {
            _onClickShowcaseEx(_go);
        }

        /// <summary>
        /// showcase开始拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        private void _onBeginDragShowcase(PointerEventData _pointData)
        {
            _onBeginDragShowcaseEx(_pointData);
        }

        /// <summary>
        /// showcase拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        private void _onDragShowcase(PointerEventData _pointData)
        {
            if (wnd == null)
                return;

            if (wnd.isRotate)
            {
                foreach (int index in wnd.rotateUnitIndexList)
                {
                    rotateUnit(index, -_pointData.delta.x * wnd.rotateSpeed);
                }
            }
            
            _onDragShowcaseEx(_pointData);
        }

        /// <summary>
        /// showcase停止拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        private void _onEndDragShowcase(PointerEventData _pointData)
        {
            _onEndDragShowcaseEx(_pointData);
        }

        /// <summary>
        /// 播放人物动画
        /// </summary>
        /// <param name="_aniName"></param>
        /// <param name="_crossFadeTime"></param>
        public void playAnim(int _index, string _aniName)
        {
            if (_m_showcaseInfo == null)
                return;

            _m_showcaseInfo.playAnim(_index, _aniName);
        }

        //强制切换动画
        public void forceSetAni(int _index, string _aniName)
        {
            if(null == _m_showcaseInfo)
                return;

            _m_showcaseInfo.forceSetAni(_index, _aniName);
        }

        public bool isPlayingAni(int _unitIndex, string _aniName)
        {
            return _m_showcaseInfo?.isPlayingAni(_unitIndex, _aniName) ?? false;
        }
        
        /// <summary>
        /// 设置动画触发器
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_name"></param>
        public void setAnimTrigger(int _index, string _name)
        {
            if (_m_showcaseInfo == null)
                return;

            _m_showcaseInfo.setAnimTrigget(_index, _name);
        }
        
        /// <summary>
        /// 设置播放速度
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_speed"></param>
        public void setSpeed(int _index, float _speed)
        {
            if (_m_showcaseInfo == null)
                return;

            _m_showcaseInfo.setSpeed(_index, _speed);
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="_unitIndex"></param>
        /// <param name="_sfxId"></param>
        /// <param name="_normalizeForward"></param>
        /// <param name="_doneAction"></param>
        /// <returns></returns>
        public void playSfx(int _unitIndex, long _sfxId)
        {
            if (_m_showcaseInfo == null)
                return;

            _m_showcaseInfo.playSfx(_unitIndex, _sfxId);
        }
        
        /// <summary>
        /// 开启变暗效果
        /// </summary>
        public void maskShowMaterial(int _unitIndex, bool _isGray)
        {
            if(null != _m_showcaseInfo)
                _m_showcaseInfo.maskShowMaterial(_unitIndex, _isGray); 
        }

        public void darkMaterial(int _unitIndex, bool _isDark)
        {
            if (null != _m_showcaseInfo)
                _m_showcaseInfo.darkMaterial(_unitIndex, _isDark);
        }

        public void additionLoadUnitAutoRemove(_AShowCaseUnitInfoObj _unitInfo, int _index, float _discardTime, Action _discardAction)
        {
            if(null == _m_showcaseInfo)
                return;
            
            _m_showcaseInfo.additionLoadUnitAutoRemove(_unitInfo, _index, _discardTime, _discardAction);
        }

        public void enableRender(int _unitIndex, bool _isEnable)
        {
            if(null == _m_showcaseInfo)
                return;
            
            _m_showcaseInfo.enableRender(_unitIndex, _isEnable);
        }

        protected virtual void _onClickShowcaseEx(GameObject _gameObject) { }
        protected virtual void _onBeginDragShowcaseEx(PointerEventData _pointData) { }
        protected virtual void _onDragShowcaseEx(PointerEventData _pointData) { }
        protected virtual void _onEndDragShowcaseEx(PointerEventData _pointData) { }
        
        /******************
         * 显示窗口的事件函数
         **/
        protected abstract void _onShowWndEx();
        /******************
         * 隐藏窗口的事件函数
         **/
        protected abstract void _onHideWndEx();
        /******************
         * 重置窗口数据的事件函数
         **/
        protected abstract void _onResetEx();
        /******************
         * 释放资源时触发的事件
         **/
        protected abstract void _onDiscardEx();
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected abstract void _onWndInitDoneEx();
    }
}
