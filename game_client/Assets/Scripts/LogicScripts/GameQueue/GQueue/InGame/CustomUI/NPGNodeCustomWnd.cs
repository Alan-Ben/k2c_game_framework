using System;
using UnityEngine;
using ALPackage;

namespace GOE
{
    public class NPGNodeCustomWnd : UIQueueBaseNode
    {


        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return _m_bIsCanRollBackQuit; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return _m_bNeedAutoRemove; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }

        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        private NPGGUIWndCustom _m_wWnd;

        private bool _m_bIsCanRollBackQuit;
        private bool _m_bNeedAutoRemove;
        private string _m_sAssetPath;
        private string _m_sObjName;

        public NPGNodeCustomWnd(string _monoAssetPath, string _monoObjName)
            : base(EUIQueueStageType.MAIN)
        {
            //跳转默认使用必关方式
            _m_bIsCanRollBackQuit = true;
            _m_bNeedAutoRemove = true;
            _m_sAssetPath = _monoAssetPath;
            _m_sObjName = _monoObjName;
        }

        public override void EnterNode()
        {
            //切换进入空视图
            GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneEmpty.instance
                , () =>
                {
                //显示窗口
                _m_wWnd = NPGGUICustomWndMgr.instance.showWnd(_m_sAssetPath, _m_sObjName);
                    if(null != _m_wWnd)
                    {
                        _m_wWnd.regLoadDoneDelegate(_refreshNodeProperty);
                    }
                });
        }

        /***********
         * 加载完成后，根据窗口配置设置本节点属性
         **/
        protected void _refreshNodeProperty()
        {
            if(null == _m_wWnd || null == _m_wWnd.wnd)
                return;

            _m_bIsCanRollBackQuit = _m_wWnd.wnd.CanNotRollBackQuit;
            _m_bNeedAutoRemove = _m_wWnd.wnd.NeedAutoRemove;
        }

        public override void QuitNode()
        {
            //隐藏窗口
            NPGGUICustomWndMgr.instance.hideWnd(_m_wWnd);
        }
    }
}
