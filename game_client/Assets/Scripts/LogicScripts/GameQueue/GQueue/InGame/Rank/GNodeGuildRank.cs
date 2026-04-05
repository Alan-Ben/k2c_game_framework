using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 常驻联盟排行榜
    /// </summary>
    public class GNodeGuildRank : BaseQueueNode
    {
        //排行榜id列表
        private List<long> _m_rankFixedIdList;
        //当前排行榜所在列表位置
        private int _m_curIdx = -1;
        private long _m_uiPathId = UIResPathConst.C_COMMON_RANK_GUILD_NORMAL_RES_ID;

        //多个排行榜
        public GNodeGuildRank(List<long> _rankFixedIdList,int _curIdx, long _uiPathId = UIResPathConst.C_COMMON_RANK_GUILD_NORMAL_RES_ID) : base(EUIQueueStageType.MAIN, UINodeTagConst_Rank.C_MAIN_GUILD_RANK_FIXED_NODE)
        {
            _m_rankFixedIdList = _rankFixedIdList;
            _m_curIdx = _curIdx;
            _m_uiPathId = _uiPathId;
        }

        //单个排行榜
        public GNodeGuildRank(long _rankFixedId, long _uiPathId = UIResPathConst.C_COMMON_RANK_GUILD_NORMAL_RES_ID) : base(EUIQueueStageType.MAIN, UINodeTagConst_Rank.C_MAIN_GUILD_RANK_FIXED_NODE)
        {
            _m_rankFixedIdList = new List<long> { _rankFixedId };
            _m_curIdx = 0;
            _m_uiPathId = _uiPathId;
        }

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
            GGUIWndGuildRankFixedDetail.instance.load();
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            GGUIWndGuildRankFixedDetail.instance.discard();
            _m_rankFixedIdList = null;
            _m_curIdx = -1;
        }

        public override void EnterNode()
        {
            if (null == _m_rankFixedIdList || _m_curIdx >= _m_rankFixedIdList.Count || _m_curIdx < 0)
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Rank.C_MAIN_GUILD_RANK_FIXED_NODE);
                return;
            }

            GUISceneMain.instance.showMainScene(NPGMainGUIAddSceneEmpty.instance);
            GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);

            if (GGUIWndGuildRankFixedDetail.instance.uiResId != _m_uiPathId)
            {
                //资源不同，强制释放
                GGUIWndGuildRankFixedDetail.instance.forceDiscard();
                GGUIWndGuildRankFixedDetail.instance.uiResId = _m_uiPathId;
                //重新加载
                GGUIWndGuildRankFixedDetail.instance.load();
            }

            GGUIWndGuildRankFixedDetail.instance.regLoadDoneDelegate(() => {
                GGUIWndGuildRankFixedDetail.instance.showWnd();
                GGUIWndGuildRankFixedDetail.instance.setInfo(_m_rankFixedIdList, _m_curIdx);
            });
        }

        public override void QuitNode()
        {
            _m_curIdx = GGUIWndGuildRankFixedDetail.instance.cutRankIndex;
            GGUIWndGuildRankFixedDetail.instance.hideWnd();
        }
    }
}
