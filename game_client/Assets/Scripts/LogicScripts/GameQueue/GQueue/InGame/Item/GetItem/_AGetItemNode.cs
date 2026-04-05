using System;
using System.Collections.Generic;
using ALPackage;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 获取物品队列节点抽象基类
    /// 用于非Notice方式展示获取物品结果, 参考_ATNPNoticeDealer_GetReward的写法
    /// </summary>
    public abstract class _AGetItemNode<_T_MONO, _T_WND> : BaseQueueNode
        where _T_MONO : NPGGUIMonoGetItem
        where _T_WND : _ANPGGUIWndBaseGetItem<_T_MONO>
    {
        // 关闭回调
        private Action _m_closeAction;
        // 奖励物品列表
        protected List<NPCommon_ItemInfo> _m_lRewardItemList;
        // 标题key
        protected string _m_sTitleKey;
        // 是否需要背景遮罩
        private bool _m_bNeedTransBk;
        // 背景遮罩窗口
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        // 操作序列号
        private long _m_lSerializeId;

        protected _AGetItemNode(List<NPCommon_ItemInfo> _itemList, string _titleKey = TransKeyConst.common_getreward_tip, bool _needTransBk = true, Action _closeAction = null) 
            : base(EUIQueueStageType.MAIN, UINodeTagConst.C_GET_ITEM)
        {
            _m_lRewardItemList = _itemList;
            _m_sTitleKey = _titleKey;
            _m_bNeedTransBk = _needTransBk;
            _m_closeAction = _closeAction;
        }

        /// <summary>
        /// 窗口实例
        /// </summary>
        protected abstract _T_WND _wnd { get; }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }

        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }

        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return true; } }

        /// <summary>
        /// 当进入队列时做的操作
        /// </summary>
        public override void onEnterQueue()
        {
            // 预加载窗口
            _wnd?.load();
        }

        /// <summary>
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            _m_lSerializeId = ALSerializeOpMgr.next();
            
            if (_m_bNeedTransBk)
            {
                _showWithTransBk();
            }
            else
            {
                _showWnd();
            }
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            _m_lSerializeId = ALSerializeOpMgr.next();
            
            // 隐藏背景遮罩
            if (_m_wTransBk != null)
            {
                NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
                _m_wTransBk = null;
            }

            // 隐藏窗口
            _wnd?.hideWnd();
        }

        /// <summary>
        /// 当关闭时做的操作
        /// </summary>
        public override void onClose()
        {
            // 销毁窗口
            _wnd?.discard();
            
            // 执行关闭回调
            Action closeAction = _m_closeAction;
            _m_closeAction = null;
            closeAction?.Invoke();
        }

        /// <summary>
        /// 显示带背景遮罩的窗口
        /// </summary>
        private void _showWithTransBk()
        {
            if (_wnd == null)
                return;
            
            if (_m_wTransBk != null)
            {
                _m_wTransBk.showWnd();
                _showWnd();
            }
            else
            {
                long serializeId = _m_lSerializeId;
                _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                    _wnd,
                    () => { QueueMgr.instance.forceCloseNode(this); },
                    () =>
                    {
                        // 序列号校验
                        if (serializeId != _m_lSerializeId)
                            return;

                        _showWnd();
                    });
            }
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        private void _showWnd()
        {
            if (_wnd == null)
                return;

            long serializeId = _m_lSerializeId;
            _wnd.regLoadDoneDelegate(() =>
            {
                // 序列号校验
                if (serializeId != _m_lSerializeId)
                    return;
                    
                if (_wnd == null)
                    return;
                    
                // 移动背景遮罩到窗口后面
                if (_m_bNeedTransBk && _m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), _wnd.getGameObj());
                else
                    GCommon.moveTransformToLastAndRefreshLayer(_wnd.getGameObj());
                    
                _wnd.setItemListAndShow(_m_lRewardItemList, _m_sTitleKey);
            });
        }
    }
}