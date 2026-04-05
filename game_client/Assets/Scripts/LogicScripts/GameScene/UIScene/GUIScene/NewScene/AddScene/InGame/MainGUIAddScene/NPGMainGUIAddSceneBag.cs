using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    public class NPGMainGUIAddSceneBag : _ANPGMainGUIAddSceneResBar<GGUIWndBagMain>
    {
        private static NPGMainGUIAddSceneBag _g_instance = new NPGMainGUIAddSceneBag();
        public static NPGMainGUIAddSceneBag instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGMainGUIAddSceneBag();

                return _g_instance;
            }
        }

        private bool _m_bSelectDefaultTab;//是否选中默认页签
        private EBagMainTabMonoType _m_eSelectTabType;//(在_m_bSelectDefaultTab为false时)指定选中的页签类型
        
        protected override GGUIWndBagMain _m_wnd { get { return GGUIWndBagMain.instance; } }

        protected override void _onShowScene()
        {
            if(!_m_bSelectDefaultTab)
                GGUIWndBagMain.instance.setSelectTab(_m_eSelectTabType);
        }

        /// <summary>
        /// 初始化的显示窗口操作
        /// </summary>
        public override void _dealShowScene(Action _delegate)
        {
            //发送处理消息
            WinMsg.SendMsg(WinMsgType.QUEST_CT_TEST_BAG);

            //调用基类处理
            base._dealShowScene(_delegate);
        }

        /// <summary>
        /// 离开视图时的处理
        /// </summary>
        /// <param name="_view"></param>
        public override void onSwitchHideScene()
        {
            NPPlayer.instance.bagComp.setBagViewed();

            //调用基类处理
            base.onSwitchHideScene();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_selectDefaultTab">是否选中默认tab</param>
        /// <param name="_selectTabType">_selectDefaultTab为false时, 需要选中的tab</param>
        public void setData(bool _selectDefaultTab, EBagMainTabMonoType _selectTabType)
        {
            _m_bSelectDefaultTab = _selectDefaultTab;
            _m_eSelectTabType = _selectTabType;
        }
    }
}
