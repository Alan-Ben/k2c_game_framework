
using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 游戏加载的附加窗口脚本
    /// </summary>
    public class NPGGUIWndLoading : _ANPGGUIBasicWnd<NPGGUIMonoLoading>, _ILoadingShow
    {
        private static NPGGUIWndLoading _g_instance = new NPGGUIWndLoading();
        public static NPGGUIWndLoading instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPGGUIWndLoading();
                return _g_instance;
            }
        }


        protected NPGGUIWndLoading()
            : base(EALUIWndLayer.TOP)
        {
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPGGUIMonoLoading.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoLoading.objName; } }

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

        //展开
        public void playExpanding(Action _doneDelegate)
        {
            if(null == wnd)
                return;
            if(null != wnd.anim)
            {
                wnd.anim.SetBool("isExpand", true);
            }

            if(null != _doneDelegate)
                ALCommonActionMonoTask.addMonoTask(_doneDelegate, wnd.expandTime);
        }

        //收起
        public void playCollapsing(Action _doneDelegate)
        {
            if(null == wnd)
                return;
            if(null != wnd.anim)
            {
                wnd.anim.SetBool("isExpand", false);
            }

            if(null != _doneDelegate)
                ALCommonActionMonoTask.addMonoTask(_doneDelegate, wnd.expandTime);
        }

        private long _m_lShowHideSerialize = 0;//解决在hide的时候再次调用show，会导致在hide方法的playExpanding回调中调用hideWnd方法, 直接关闭了窗口
        
        void _ILoadingShow.show(Action _doneDelegate)
        {
            long serialize = _m_lShowHideSerialize = ALSerializeOpMgr.next();
            load(() =>
            {
                if(serialize != _m_lShowHideSerialize)
                    return;
                
                //放到最后
                GCommon.moveTransformToLastAndRefreshLayer(wnd);
                
                showWnd(() =>
                {
                    if(serialize != _m_lShowHideSerialize)
                        return;
                    
                    playCollapsing(_doneDelegate);
                });
            });
        }
        void _ILoadingShow.hide(Action _doneDelegate)
        {
            long serialize = _m_lShowHideSerialize = ALSerializeOpMgr.next();

            // 这里的 hide 就不调用 discard 了，我们的加载界面常驻就好
            playExpanding(() =>
            {
                if(serialize != _m_lShowHideSerialize)
                    return;
                
                hideWnd(_doneDelegate);
            });
        }
    }
}
