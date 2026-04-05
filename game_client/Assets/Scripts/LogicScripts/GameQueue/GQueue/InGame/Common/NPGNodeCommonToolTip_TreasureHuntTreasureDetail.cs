using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 奇物详情ToolTip节点
    /// </summary>
    public class NPGNodeCommonToolTip_TreasureHuntTreasureDetail : BaseQueueNode
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
        public override bool NeedAutoRemove { get { return true; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return false; } }

        private NPCommonAssetPathInfo _m_iAssetPathInfo;
        private _ITreasureHuntTreasureInfo _m_treasureInfo;
        private RectTransform _m_targetTransRoot;
        private Vector2 _m_interval;

        private NPGGUIWndCommonToolTip_TreasureHuntTreasureDetail _m_wndToolTip;
        
        public NPGNodeCommonToolTip_TreasureHuntTreasureDetail(NPCommonAssetPathInfo _assestPathInfo, _ITreasureHuntTreasureInfo _treasureInfo, RectTransform _targetTransRoot, Vector2 _interval)
            : base(EUIQueueStageType.NONE)
        {
            _m_iAssetPathInfo = _assestPathInfo;
            _m_treasureInfo = _treasureInfo;
            _m_targetTransRoot = _targetTransRoot;
            _m_interval = _interval;
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
                _m_wndToolTip = new NPGGUIWndCommonToolTip_TreasureHuntTreasureDetail(_m_iAssetPathInfo?.asset_path, _m_iAssetPathInfo?.obj_name);
                _m_wndToolTip.load();
                _m_wndToolTip.regLoadDoneDelegate(() =>
                {
                    _m_wndToolTip.setInfo(_m_treasureInfo, _m_targetTransRoot, _m_interval);
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
