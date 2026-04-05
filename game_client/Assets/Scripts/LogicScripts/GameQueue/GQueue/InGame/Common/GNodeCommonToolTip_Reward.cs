using ALPackage;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    //奖励预览跟随Node
    public class GNodeCommonToolTip_Reward : BaseQueueNode
    {
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }

        /// <summary>
        /// 是否在进入任何Node时都需要退出的节点
        /// </summary>
        public override bool isAllNodeOpQuitNode { get { return false; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return false; } }
        public override bool NeedRemovePreAutoRemove { get { return false; } }

        //标题
        private string _m_titleStr;
        // 内容文本
        private string _m_contentStr;
        //奖励列表
        private List<NPCommonCostItem> _m_itemList;
        //加载路径id
        private long _m_resPathId;

        private RectTransform _m_targetTransRoot;
        private Vector2 _m_interval;
        private ECommonRewardType _m_rRewardType;

        private GGUIWndCommonItemToolTip_Reward _m_wndToolTip;

        public GNodeCommonToolTip_Reward(long _resPathId,string _titleStr, string _contentStr, List<NPCommonCostItem> _itemList, RectTransform _targetTransRoot, Vector2 _interval, ECommonRewardType _type = ECommonRewardType.NONE)
            : base(EUIQueueStageType.NONE)
        {
            _m_resPathId = _resPathId;
            _m_titleStr = _titleStr;
            _m_contentStr = _contentStr;
            _m_itemList = _itemList;
            _m_targetTransRoot = _targetTransRoot;
            _m_interval = _interval;
            _m_rRewardType = _type;
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
                _m_wndToolTip = new GGUIWndCommonItemToolTip_Reward(
                    UIResPathAssistant.getAssetPath(_m_resPathId),
                    UIResPathAssistant.getObjName(_m_resPathId)
                    );
                _m_wndToolTip.load();
                _m_wndToolTip.regLoadDoneDelegate(() =>
                {
                    _m_wndToolTip.setData(_m_titleStr, _m_contentStr, _m_itemList, _m_targetTransRoot, _m_interval, _m_rRewardType);
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
