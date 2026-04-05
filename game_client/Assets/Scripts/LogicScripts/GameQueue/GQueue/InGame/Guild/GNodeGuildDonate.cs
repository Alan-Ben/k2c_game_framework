using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠Node
    /// </summary>
    public class GNodeGuildDonate : BaseQueueNode
    {
        //回退按钮
        private NPGGUIWndInstanceCommonBack _m_cbCommonBackObj;
        
        public GNodeGuildDonate() : base(EUIQueueStageType.MAIN)
        {
        }
        
        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return false; } }
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
        }
        
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
        }
        
        public override void EnterNode()
        {
            _showBackBtn();
            
            GUISceneMain.instance.showMainScene(GMainGUIAddSceneGuildDonate.instance, _onAllSceneInited);
        }

        /// <summary>
        /// 所有场景加载完毕后的处理
        /// </summary>
        private void _onAllSceneInited()
        {
            //放到最后
            if (_m_cbCommonBackObj != null)
                GCommon.moveTransformToLastAndRefreshLayer(_m_cbCommonBackObj.wnd);
        }

        public override void QuitNode()
        {
            if (null != _m_cbCommonBackObj)
            {
                NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
                _m_cbCommonBackObj = null;
            }
        }

        /// <summary>
        /// 显示回退按钮
        /// </summary>
        private void _showBackBtn()
        {
            if (null != _m_cbCommonBackObj)
            {
                NPUIInstanceCommonBackController.instance.hideCommonBack(_m_cbCommonBackObj, UIResPathConst.WIN_COMMON_BACK);
                _m_cbCommonBackObj = null;
            }
            //加载通用回退按钮
            _m_cbCommonBackObj = NPUIInstanceCommonBackController.instance.showCommonBack(EALUIWndLayer.NORMAL, () => { QueueMgr.instance.forceCloseNode(this); }, UIResPathConst.WIN_COMMON_BACK);
        }
    }
}