using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 未解锁妃子详细信息
    /// </summary>
    public class GNodeUnLockConsortDetail : BaseQueueNode
    {
        [NotNull] private List<GGottenConsortInfo> _m_lAllConsortShowInfoList = new List<GGottenConsortInfo>();//所有妃子信息
        private int _m_iCurConsortIndex;//当前选中妃子下标
        private EConsortTdShowAniType _m_eShowAniType;//进入动画类型
        
        private GNodeUnLockConsortDetail(List<GGottenConsortInfo> _allConsortShowInfoList, int _curConsortIndex, EUnLockConsortDetailWndTabType _curTabType = EUnLockConsortDetailWndTabType.NONE, EConsortTdShowAniType _showAniType = EConsortTdShowAniType.ENTER): base(EUIQueueStageType.MAIN, UINodeTagConst.C_UNLOCK_CONSORT_DETAIL)
        {
            if(_allConsortShowInfoList != null)
                _m_lAllConsortShowInfoList.AddRange(_allConsortShowInfoList);
            _m_iCurConsortIndex = _curConsortIndex;
            _m_eShowAniType = _showAniType;

            GMainGUIMainSceneConsortDetail.instance.consortDetailWndSelectTabType = _curTabType;//设置选中的tab类型
        }

        private GNodeUnLockConsortDetail(GGottenConsortInfo _consortInfo, EUnLockConsortDetailWndTabType _curTabType = EUnLockConsortDetailWndTabType.NONE, EConsortTdShowAniType _showAniType = EConsortTdShowAniType.ENTER): base(EUIQueueStageType.MAIN, UINodeTagConst.C_UNLOCK_CONSORT_DETAIL)
        {
            _m_lAllConsortShowInfoList.Add(_consortInfo);
            _m_iCurConsortIndex = 0;
            _m_eShowAniType = _showAniType;

            GMainGUIMainSceneConsortDetail.instance.consortDetailWndSelectTabType = _curTabType;//设置选中的tab类型
        }

        #region 添加Node方法

        public static void addConsortNode(List<GGottenConsortInfo> _allConsortShowInfoList, int _curConsortIndex, EUnLockConsortDetailWndTabType _curTabType = EUnLockConsortDetailWndTabType.NONE, EConsortTdShowAniType _showAniType = EConsortTdShowAniType.ENTER)
        {
            // 尝试找到最后一个 GNodeUnLockConsortDetail，找到了就直接退到这个 node
            if (QueueMgr.instance.findLastNode(typeof(GNodeUnLockConsortDetail)) is GNodeUnLockConsortDetail spaceNode)
            {
                QueueMgr.instance.QuitUntilCanStop(_node => _node == spaceNode);
                QueueMgr.instance.forceCloseLastNode();//关闭最后一个节点
            }

            GNodeUnLockConsortDetail unLockConsortDetail = new GNodeUnLockConsortDetail(_allConsortShowInfoList, _curConsortIndex, _curTabType, _showAniType);
            QueueMgr.instance.AddNode(unLockConsortDetail);
        }
        
        public static void addConsortNode(GGottenConsortInfo _consortInfo, EUnLockConsortDetailWndTabType _curTabType = EUnLockConsortDetailWndTabType.NONE, EConsortTdShowAniType _showAniType = EConsortTdShowAniType.ENTER)
        {
            // 尝试找到最后一个 GNodeUnLockConsortDetail，找到了就直接退到这个 node
            if (QueueMgr.instance.findLastNode(typeof(GNodeUnLockConsortDetail)) is GNodeUnLockConsortDetail spaceNode)
            {
                QueueMgr.instance.QuitUntilCanStop(_node => _node == spaceNode);
                QueueMgr.instance.forceCloseLastNode();//关闭最后一个节点
            }

            GNodeUnLockConsortDetail unLockConsortDetail = new GNodeUnLockConsortDetail(_consortInfo, _curTabType, _showAniType);
            QueueMgr.instance.AddNode(unLockConsortDetail);
        }
        
        public static void addConsortNode(List<GGottenConsortInfo> _allConsortShowInfoList, long _consortId, EUnLockConsortDetailWndTabType _curTabType = EUnLockConsortDetailWndTabType.NONE, EConsortTdShowAniType _showAniType = EConsortTdShowAniType.ENTER)
        {
            int curConsortIndex = _allConsortShowInfoList?.FindIndex((_consortInfo) => { return _consortInfo != null && _consortInfo.consortId == _consortId; }) ?? 0;
            addConsortNode(_allConsortShowInfoList, curConsortIndex, _curTabType, _showAniType);
        }
        
        /// <summary>
        /// 这个方法一定进入交互页面
        /// </summary>
        /// <param name="_allConsortShowInfoList"></param>
        /// <param name="_curConsortIndex"></param>
        /// <param name="_curTabType"></param>
        public static void addConsortNodeInteraction(List<GGottenConsortInfo> _allConsortShowInfoList, int _curConsortIndex, EUnlockConsortDetailWndInteractionPageTabType _interactionPageTab = EUnlockConsortDetailWndInteractionPageTabType.NONE)
        {
            // 尝试找到最后一个 GNodeUnLockConsortDetail，找到了就直接退到这个 node
            if (QueueMgr.instance.findLastNode(typeof(GNodeUnLockConsortDetail)) is GNodeUnLockConsortDetail spaceNode)
            {
                QueueMgr.instance.QuitUntilCanStop(_node => _node == spaceNode);
                QueueMgr.instance.forceCloseLastNode();//关闭最后一个节点
            }
            
            GMainGUIMainSceneConsortDetail.instance.interactionPageTabType = _interactionPageTab;//设置选中的交互页面tab类型
            GNodeUnLockConsortDetail unLockConsortDetail = new GNodeUnLockConsortDetail(_allConsortShowInfoList, _curConsortIndex, EUnLockConsortDetailWndTabType.INTERACTION);
            QueueMgr.instance.AddNode(unLockConsortDetail);
        }
        
        /// <summary>
        /// 这个方法一定进入交互页面
        /// </summary>
        public static void addConsortNodeInteraction(GGottenConsortInfo _consortInfo, EUnlockConsortDetailWndInteractionPageTabType _interactionPageTab = EUnlockConsortDetailWndInteractionPageTabType.NONE)
        {
            // 尝试找到最后一个 GNodeUnLockConsortDetail，找到了就直接退到这个 node
            if (QueueMgr.instance.findLastNode(typeof(GNodeUnLockConsortDetail)) is GNodeUnLockConsortDetail spaceNode)
            {
                QueueMgr.instance.QuitUntilCanStop(_node => _node == spaceNode);
                QueueMgr.instance.forceCloseLastNode();//关闭最后一个节点
            }
            
            GMainGUIMainSceneConsortDetail.instance.interactionPageTabType = _interactionPageTab;//设置选中的交互页面tab类型
            GNodeUnLockConsortDetail unLockConsortDetail = new GNodeUnLockConsortDetail(_consortInfo, EUnLockConsortDetailWndTabType.INTERACTION);
            QueueMgr.instance.AddNode(unLockConsortDetail);
        }
        
        /// <summary>
        /// 这个方法一定进入交互页面
        /// </summary>
        /// <param name="_allConsortShowInfoList"></param>
        /// <param name="_curConsortIndex"></param>
        /// <param name="_curTabType"></param>
        public static void addConsortNodeInteraction(List<GGottenConsortInfo> _allConsortShowInfoList, long _consortId, EUnlockConsortDetailWndInteractionPageTabType _interactionPageTab = EUnlockConsortDetailWndInteractionPageTabType.NONE)
        {
            int curConsortIndex = _allConsortShowInfoList?.FindIndex((_consortInfo) => { return _consortInfo != null && _consortInfo.consortId == _consortId; }) ?? 0;
            addConsortNodeInteraction(_allConsortShowInfoList, curConsortIndex, _interactionPageTab);
        }
        
        #endregion
        

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
            //初始化显示数据
            GMainGUIMainSceneConsortDetail.instance.initShowData(_m_lAllConsortShowInfoList, _m_iCurConsortIndex, _m_eShowAniType);
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            // 重置妃子详情页面选中的tab类型
            GMainGUIMainSceneConsortDetail.instance.consortDetailWndSelectTabType = EUnLockConsortDetailWndTabType.NONE;
            // 重置妃子详情页面选中的妃子下标
            GMainGUIMainSceneConsortDetail.instance.consortDetailWndShowConsortIndex = 0;
            // 重置妃子详情页面 经营技能页面选中的技能id
            GMainGUIMainSceneConsortDetail.instance.consortDetailBusinessPageSelectSkillId = 0;

            // 重置妃子详情页面 互动页面选中的tab类型
            GMainGUIMainSceneConsortDetail.instance.interactionPageTabType = EUnlockConsortDetailWndInteractionPageTabType.NONE;
            // 重置妃子详情页面 互动页面 送礼页面选中的礼物下标
            GMainGUIMainSceneConsortDetail.instance.interactionSendGiftPageSelectIndex = -1;
        }
        
        public override void EnterNode()
        {
            //显示对应UIScene
            GUISceneMain.instance.showMainScene(GMainGUIMainSceneConsortDetail.instance);
        }

        /// <summary>
        /// 所有场景加载完毕后的处理
        /// </summary>
        private void _onAllSceneInited()
        {
        }

        public override void QuitNode()
        {
        }
        
        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }
    }
}