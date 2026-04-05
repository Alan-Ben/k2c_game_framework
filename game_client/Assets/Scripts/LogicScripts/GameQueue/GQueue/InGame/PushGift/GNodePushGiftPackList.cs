using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 推送礼包列表节点
    /// </summary>
    public class GNodePushGiftPackList : BaseQueueNode
    {
        // 显示的推送礼包类型列表
        private List<EPushGiftPackType> _m_lShowPushGiftPackTypeList;
        // 是否显示背景遮罩
        private bool _m_bNeedShowBk;
        // 关闭节点回调
        private Action _m_aOnCloseNode;
        
        // 背景遮罩窗口
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        // 操作序列号
        private long _m_lEnterNodeSerializeId;

        public GNodePushGiftPackList(List<EPushGiftPackType> _showPushGiftPackTypeList, bool _needShowBk, Action _onCloseNode = null) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_PUSH_GIFT_PACK_LIST)
        {
            _m_lShowPushGiftPackTypeList = _showPushGiftPackTypeList;
            _m_bNeedShowBk = _needShowBk;
            _m_aOnCloseNode = _onCloseNode;
            
            // 默认选择第一个礼包类型
            GGUIWndPushGiftPackList.instance.setSelectGiftPackIndex(0);
        }

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
        /// 当进入节点时做的操作
        /// </summary>
        public override void EnterNode()
        {
            _m_lEnterNodeSerializeId = ALSerializeOpMgr.next();
            if (_m_bNeedShowBk)
            {
                _showWithTransBk();
            }
            else
            {
                _afterShowBk();
            }
        }

        /// <summary>
        /// 当退出节点时做的操作
        /// </summary>
        public override void QuitNode()
        {
            _m_lEnterNodeSerializeId = ALSerializeOpMgr.next();
            
            // 隐藏背景遮罩
            if (_m_wTransBk != null)
            {
                NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
                _m_wTransBk = null;
            }

            // 隐藏窗口
            GGUIWndPushGiftPackList.instance.hideWnd();
        }

        /// <summary>
        /// 当进入队列时做的操作
        /// </summary>
        public override void onEnterQueue()
        {
            GGUIWndPushGiftPackList.instance.load();
        }

        public override void onClose()
        {
            _m_lShowPushGiftPackTypeList = null;
            GGUIWndPushGiftPackList.instance.discard();
            
            Action action = _m_aOnCloseNode;
            _m_aOnCloseNode = null;
            action?.Invoke();
        }

        public override void onCannotEscBack()
        {
            dealCloseNode();
        }

        /// <summary>
        /// 显示带背景遮罩的窗口
        /// </summary>
        private void _showWithTransBk()
        {
            if (_m_wTransBk != null)
            {
                _m_wTransBk.showWnd();
                _afterShowBk();
            }
            else
            {
                long serializeId = _m_lEnterNodeSerializeId;
                _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                    GGUIWndPushGiftPackList.instance,
                    () =>
                    {
                        // dealCloseNode();
                    },
                    () =>
                    {
                        if (serializeId != _m_lEnterNodeSerializeId)
                            return;

                        _afterShowBk();
                    });
            }
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        private void _afterShowBk()
        {
            long serializeId = _m_lEnterNodeSerializeId;
            GGUIWndPushGiftPackList.instance.regLoadDoneDelegate(() =>
            {
                // 序列号校验，避免异步回调时状态已变化
                if (serializeId != _m_lEnterNodeSerializeId)
                    return;
                
                // 模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                if (_m_bNeedShowBk && _m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), GGUIWndPushGiftPackList.instance.getGameObj());
                else
                    // 没有遮罩就只将窗口移到最前
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndPushGiftPackList.instance.getGameObj());
                
                GGUIWndPushGiftPackList.instance.setShowPushGiftPackTypeList(_m_lShowPushGiftPackTypeList);
                GGUIWndPushGiftPackList.instance.showWnd();
            });
        }

        /// <summary>
        /// 处理关闭节点
        /// </summary>
        public void dealCloseNode()
        {
            QueueMgr.instance.forceCloseNode(this);
        }
    }
}