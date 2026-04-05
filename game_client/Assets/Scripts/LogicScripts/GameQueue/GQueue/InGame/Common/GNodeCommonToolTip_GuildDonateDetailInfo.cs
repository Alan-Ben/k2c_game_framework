using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠详情 跟随窗口
    /// </summary>
    public class GNodeCommonToolTip_GuildDonateDetailInfo : BaseQueueNode
    {
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }

        /// <summary>
        /// 是否在进入任何Node时都需要退出的节点
        /// </summary>
        public override bool isAllNodeOpQuitNode { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }
        public override bool NeedRemovePreAutoRemove { get { return false; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return false; } }

        private string _m_sAssetPath;
        private string _m_sAssetName;
        private RectTransform _m_targetTransRoot;
        private float _m_intervalX;
        private float _m_intervalY;

        private GGUIWndCommonToolTip_GuildDonateDetailInfo _m_wndToolTip;
        
        public GNodeCommonToolTip_GuildDonateDetailInfo(string _assetPath, string _assetName, RectTransform _targetTransRoot, float _intervalX, float _intervalY)
            : base(EUIQueueStageType.NONE)
        {
            _m_sAssetPath = _assetPath;
            _m_sAssetName = _assetName;
            _m_targetTransRoot = _targetTransRoot;
            _m_intervalX = _intervalX;
            _m_intervalY = _intervalY;

        }

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
            if (null == _m_wndToolTip)
            {
                _m_wndToolTip = new GGUIWndCommonToolTip_GuildDonateDetailInfo(_m_sAssetPath, _m_sAssetName);
                _m_wndToolTip.load();
                _m_wndToolTip.regLoadDoneDelegate(() =>
                {
                    _m_wndToolTip.showWnd();
                    _m_wndToolTip.setInfo(_m_targetTransRoot, _m_intervalX, _m_intervalY);
                });
            }

            _m_wndToolTip.regLoadDoneDelegate(() =>
            {
                _m_wndToolTip.showWnd();
            });
        }

        public override void QuitNode()
        {
            if (null != _m_wndToolTip)
            {
                _m_wndToolTip.discard();
                _m_wndToolTip = null;
            }
        }
    }
}
