using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


namespace GOE
{
    public class NPGGUIWndInstanceCommonBack : _ANPGGUILoadUIWnd<NPGGUIMonoCommonBack>
    {
        /// <summary>
        /// 展示一个透明背景，展示操作请务必在此进行
        /// </summary>
        /// <param name="_clickDelegate"></param>
        public static void showCommonBack(EALUIWndLayer _layer, Action _clickDelegate, long _backResPathId = UIResPathConst.WIN_COMMON_BACK)
        {
            NPUIInstanceCommonBackController.instance.showCommonBack(_layer, _clickDelegate, _backResPathId);
        }

        private Action _m_dOnBtnClick;
        //对应资源样式id
        private long _m_uiResPathId;
        
        public NPGGUIWndInstanceCommonBack(long _uiResPathId)
            : base()
        {
            _m_uiResPathId = _uiResPathId;
            _m_dOnBtnClick = null;
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiResPathId); } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            _m_dOnBtnClick = null;
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_dOnBtnClick = null;
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.backBtn, _onClickBack);
        }

        /*************
         * 设置点击操作
         **/
        public void setClickOp(Action _clickOp)
        {
            _m_dOnBtnClick = _clickOp;
        }

        /***************
         * 点击背景时触发的操作
         **/
        protected void _onClickBack(GameObject _go)
        {
            if(null == _m_dOnBtnClick)
                return;

            _m_dOnBtnClick();
        }

        /// <summary>
        /// 获取父容器节点，使用add层对象
        /// </summary>
        /// <returns></returns>
        protected override Transform _getParentTransForm()
        {
            return _AALMonoMain.instance.getUILayerRootTrans(EALUIWndLayer.NORMAL);
        }

        /// <summary>
        /// 移动到对应层级
        /// </summary>
        public void moveToLayer(EALUIWndLayer _layer)
        {
            if(null == wnd)
                return;

            wnd.transform.SetParent(_AALMonoMain.instance.getUILayerRootTrans(_layer));
            //放到最后
            GCommon.moveTransformToLastAndRefreshLayer(wnd);
        }
    }
}
