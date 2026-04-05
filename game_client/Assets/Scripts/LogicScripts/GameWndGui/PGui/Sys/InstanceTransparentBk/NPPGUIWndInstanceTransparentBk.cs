using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


namespace GOE
{
    public class NPPGUIWndInstanceTransparentBk : _ANPGGUILoadUIWnd<NPPGUIMonoTransparentBk>
    {
        /// <summary>
        /// 展示一个透明背景，展示操作请务必在此进行
        /// </summary>
        /// <param name="_clickDelegate"></param>
        public static NPPGUIWndInstanceTransparentBk showTransparentBk(Action _clickDelegate, Action _afterBlurAction)
        {
            NPPGUIWndInstanceTransparentBk bk = NPUIInstanceTransparentBkController.instance.showInstanceTransparentBk(
                (_bk) => {//设置关联窗口
                    _bk._setAfterBlurAction(_afterBlurAction);
                }
                , _clickDelegate);

            return bk;
        }
        public static NPPGUIWndInstanceTransparentBk showTransparentBkByBlurUI(_IGGUIScreenBlurCurUI _uiObj, Action _clickDelegate, Action _afterBlurAction)
        {
            NPPGUIWndInstanceTransparentBk bk = NPUIInstanceTransparentBkController.instance.showInstanceTransparentBk(
                (_bk) => {//设置关联窗口
                    _bk._setUIObj(_uiObj);
                    _bk._setAfterBlurAction(_afterBlurAction);
                }
                , _clickDelegate);

            return bk;
        }

        /// <summary>
        /// 直接带入最基类的UI对象，根据是否继承了接口进行不同处理
        /// </summary>
        /// <param name="_uiObj"></param>
        /// <param name="_clickDelegate"></param>
        /// <param name="_afterBlurAction"></param>
        /// <returns></returns>
        public static NPPGUIWndInstanceTransparentBk showTransparentBk(_AALBasicLoadUIWndBasicClass _uiObj, Action _clickDelegate, Action _afterBlurAction)
        {
            if(null == _uiObj || !(_uiObj is _IGGUIScreenBlurCurUI))
            {
                return showTransparentBk(_clickDelegate, _afterBlurAction);
            }
            else
            {
                return showTransparentBkByBlurUI((_IGGUIScreenBlurCurUI)_uiObj, _clickDelegate, _afterBlurAction);
            }
        }

        //当前显示的UI对象
        private _IGGUIScreenBlurCurUI _m_uiUIObj;

        private Action _m_dOnBkClick;
        //在模糊渲染完成之后的处理操作，一般在这里显示窗口
        private Action _m_dAfterBlurFunc;

        /// <summary>
        /// 窗口开启时主相机的开启状态
        /// </summary>
        public bool lastMainCamEnable { get; set; }
        
        /// <summary>
        /// 窗口开启时RT主相机的开启状态
        /// </summary>
        public bool lastRTCamEnable { get; set; }

        public NPPGUIWndInstanceTransparentBk()
            : base()
        {
            _m_dOnBkClick = null;
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoTransparentBk.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoTransparentBk.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

        /// <summary>
        /// 对应控制显示的UI对象
        /// </summary>
        public _IGGUIScreenBlurCurUI uiObj { get { return _m_uiUIObj; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            // 开启模糊 需要在NPPGUIWndInstanceTransparentBk.showWnd之前（会关闭主相机和RT主相机），因为需要根据当前相机enable来确定渲染哪些相机
            ScreenBlurMgr.instance.showBlur(this, _m_dAfterBlurFunc);
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            // 关闭模糊
            ScreenBlurMgr.instance.hideBlur();
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            _m_uiUIObj = null;
            _m_dOnBkClick = null;

            if (null != wnd && null != wnd.imgRT)
            {
                wnd.imgRT.texture = null;
                wnd.imgRT.color = wnd.getRTDefaultColor();
            }
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_uiUIObj = null;
            _m_dOnBkClick = null;

            //重置未默认颜色
            if (null != wnd && null != wnd.imgRT)
            {
                wnd.imgRT.texture = null;
                wnd.imgRT.color = wnd.getRTDefaultColor();
            }
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.bkClickGo, _onClickBk);
            
            if (null != wnd && null != wnd.imgRT)
            {
                wnd.imgRT.texture = null;
                wnd.imgRT.color = wnd.getRTDefaultColor();
            }
        }

        /// <summary>
        /// 设置模糊之后的处理流程
        /// </summary>
        protected void _setAfterBlurAction(Action _afterBlurAction)
        {
            _m_dAfterBlurFunc = _afterBlurAction;
        }

        /// <summary>
        /// 设置当前模糊背景显示的UI对象
        /// </summary>
        /// <param name="_uiObj"></param>
        protected void _setUIObj(_IGGUIScreenBlurCurUI _uiObj)
        {
            _m_uiUIObj = _uiObj;
        }

        /*************
         * 设置点击操作
         **/
        public void setClickOp(Action _clickOp)
        {
            _m_dOnBkClick = _clickOp;
        }

        /***************
         * 点击背景时触发的操作
         **/
        protected void _onClickBk(GameObject _go)
        {
            if(null == _m_dOnBkClick)
                return;

            _m_dOnBkClick();
        }

        /// <summary>
        /// 开启模糊，如果是实时模糊则不需要传递RenderTexture
        /// </summary>
        /// <param name="_isRealTime">是否是实时模糊 or 静态模糊</param>
        /// <param name="_rt"></param>
        public void showBlur(RenderTexture _rt)
        {
            setTexture(_rt);
        }
        
        private void setTexture(RenderTexture _rt)
        {
            if (null != wnd && null != wnd.imgRT)
            {
                wnd.imgRT.texture = _rt;
            }
        }
        
        //单独设置颜色，有些业务的bk需要单独配置
        public void setColorInfo(float _alpha)
        {
            Color color = Color.white * _alpha;
            color.a = 1;
            if (null != wnd && null != wnd.imgRT)
            {
                wnd.imgRT.color = color;
            }
        }

        /// <summary>
        /// 开启Blur RT的渲染
        /// </summary>
        public void enableShow()
        {
            wnd.imgRT.enabled = true;
        }
        /// <summary>
        /// 关闭Blur RT的渲染
        /// </summary>
        public void disableShow()
        {
            wnd.imgRT.enabled = false;
        }

        /// <summary>
        /// 获取父容器节点，使用add层对象
        /// </summary>
        /// <returns></returns>
        protected override Transform _getParentTransForm()
        {
            return _AALMonoMain.instance.getUILayerRootTrans(EALUIWndLayer.ADDITION);
        }
    }
}
