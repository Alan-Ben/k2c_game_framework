using UnityEngine;
using UnityEngine.UI;
using ALPackage;

namespace GOE
{
    /************************
    * 尝试网络重联（菊花）
    **/
    public class NPPGUIWndReconnecting : _ANPGGUIBasicWnd<NPPGUIMonoReconnecting>
    {
        private static NPPGUIWndReconnecting _g_instance = new NPPGUIWndReconnecting();
        public static NPPGUIWndReconnecting instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new NPPGUIWndReconnecting();
                return _g_instance;
            }
        }

        protected NPPGUIWndReconnecting()
            : base(EALUIWndLayer.TOP)
        {
        }

        private int _m_inputMaskSerialize;
        
        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return NPPGUIMonoReconnecting.assetPath; } }
        protected override string _monoObjName { get { return NPPGUIMonoReconnecting.objName; } }
        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return PlatResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);
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
            //绑定进入按钮操作

        }

    }
}
