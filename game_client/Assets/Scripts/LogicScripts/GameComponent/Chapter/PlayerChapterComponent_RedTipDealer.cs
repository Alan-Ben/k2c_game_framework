using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 关卡数据组件 - 红点管理
    /// </summary>
    public partial class PlayerChapterComponent
    {
        // 红点管理器实例
        private RedTipDealer _m_redTipDealer;
        
        private class RedTipDealer
        {
            [NotNull] private readonly PlayerChapterComponent _m_comp;
            
            private _ARedTipNode _m_plotEntranceRedTipNode;
            
            public RedTipDealer([NotNull] PlayerChapterComponent _comp)
            {
                _m_comp = _comp;
            }
            
            /// <summary>
            /// 初始化红点
            /// </summary>
            public void init()
            {
                // 获取剧情入口红点节点
                _m_plotEntranceRedTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_CHAPTER_PLOT_ENTRANCE);
                
                // 初始刷新红点状态
                refreshPlotEntranceRedTip();
            }
            
            /// <summary>
            /// 清空红点
            /// </summary>
            public void clear()
            {
                _m_plotEntranceRedTipNode = null;
            }
            
            /// <summary>
            /// 刷新剧情入口红点
            /// </summary>
            public void refreshPlotEntranceRedTip()
            {
                if (_m_plotEntranceRedTipNode == null)
                    return;
                
                // 检查是否有未领取的剧情奖励
                bool hasUnclaimed = _m_comp.hasUnclaimedPlotReward();
                
                // 设置红点显示状态（1表示显示，0表示隐藏）
                _m_plotEntranceRedTipNode.setCount(hasUnclaimed ? 1 : 0);
            }
        }
    }
}
