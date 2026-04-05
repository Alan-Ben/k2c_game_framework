using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /*******************
     * 加载过程背景窗口对象
     **/
    public class NPPGUIWndLoadingBk : _ANPGGUIBasicWnd<NPPGUIMonoLoadingBk>
    {
        private static NPPGUIWndLoadingBk _g_instance = new NPPGUIWndLoadingBk();
        public static NPPGUIWndLoadingBk instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPPGUIWndLoadingBk();
                return _g_instance;
            }
        }

        //刷新操作序列号，确保设置之后原先刷新对象不会有效
        private long _m_lRefreshSerialize;
        //开启的任务对象
        private ALCommonEnableTaskController _m_tcTaskController;

        protected NPPGUIWndLoadingBk()
            : base(EALUIWndLayer.ADDITION)
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();

            _m_tcTaskController = new ALCommonEnableTaskController();
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoLoadingBk.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoLoadingBk.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

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
            //窗口隐藏则直接停止当前所有处理
            //刷新操作序列号
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            //关闭原任务
            _m_tcTaskController.setDisable();
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        /// <summary>
        /// 设置当前刷新对象
        /// </summary>
        /// <param name="_refresher"></param>
        public void setRefresher(_INPPGUILoadingBkProcessRefresher _refresher)
        {
            //刷新操作序列号
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            //赋值到临时对象
            long tmpRefreshSerialize = _m_lRefreshSerialize;

            //关闭原任务
            _m_tcTaskController.setDisable();

            //判断对象是否有效，未显示也不处理
            if (null == _refresher || !isShow)
                return;

            //开启定时任务
            _m_tcTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(
                () => {
                    //判断序列号是否一致
                    if (tmpRefreshSerialize != _m_lRefreshSerialize)
                        return;

                    //调用刷新对象处理
                    setProcess(_refresher.curProcess, _refresher.curOPTxt);
                }, _refresher.refreshDuration);
        }

        /**************
         * 设置进度条
         **/
        public void setProcess(float _process)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setSliderTransScale(wnd.sldProgress, _process);
            ALUGUICommon.setSliderScale(wnd.sldProgress2, _process);
            
            if (!string.IsNullOrEmpty(wnd.txtLoadingProgressKey))
            {
                int integerPart = (int)_process;
                float decimalPart = _process - integerPart;
                int firstDecimal = (int)(decimalPart * 10);//获取第一位小数
                
                ALUGUICommon.setLabelTxt(wnd.txtLoadingProgress, TextTranslate.instance.getLanguage(wnd.txtLoadingProgressKey, integerPart, firstDecimal));
            }
        }
        public void setProcess(float _process, string _txt)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setSliderTransScale(wnd.sldProgress, _process);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, _txt);
            ALUGUICommon.setSliderScale(wnd.sldProgress2, _process);

            if (!string.IsNullOrEmpty(wnd.txtLoadingProgressKey))
            {
                _process = _process * 100;
                int integerPart = (int)_process;
                float decimalPart = _process - integerPart;
                int firstDecimal = (int)(decimalPart * 10);//获取第一位小数
                
                ALUGUICommon.setLabelTxt(wnd.txtLoadingProgress, TextTranslate.instance.getLanguage(wnd.txtLoadingProgressKey, integerPart, firstDecimal));
            }
        }

        /**************
         * 设置上面的文字信息
         **/
        public void setLoadingTxt(string _txt)
        {
            if(null == wnd)
                return;

            ALUGUICommon.setGameObjEnable(wnd.txtLoadingTxt, true);
            ALUGUICommon.setLabelTxt(wnd.txtLoadingTxt, _txt);
        }
    }
}
