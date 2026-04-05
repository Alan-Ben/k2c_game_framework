using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 推送礼包弹出节点
    /// </summary>
    public class GNodePushGiftPackPop : BaseQueueNode
    {
        // 推送礼包信息
        private PushGiftPackInfo _m_pushGiftPackInfo;
        // 是否显示背景遮罩
        private bool _m_bNeedShowBk;
        private Action _m_aOnCloseNode;
        
        // 背景遮罩窗口
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        // 操作序列号
        private long _m_lEnterNodeSerializeId;

        public GNodePushGiftPackPop(PushGiftPackInfo _pushGiftPackInfo, bool _needShowBk, Action _onCloseNode) : base(EUIQueueStageType.MAIN, UINodeTagConst.C_PUSH_GIFT_PACK_POP)
        {
            _m_pushGiftPackInfo = _pushGiftPackInfo;
            _m_bNeedShowBk = _needShowBk;
            _m_aOnCloseNode = _onCloseNode;
            
            GGUIWndPushGiftPackPop.instance.setNode(this);
        }

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return false; } }//不允许回退退出

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
            GGUIWndPushGiftPackPop.instance.hideWnd();
        }

        /// <summary>
        /// 当进入队列时做的操作
        /// </summary>
        public override void onEnterQueue()
        {
            GGUIWndPushGiftPackPop.instance.load();
        }

        public override void onClose()
        {
            _m_pushGiftPackInfo = null;
            GGUIWndPushGiftPackPop.instance.discard();
            
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
                    GGUIWndPushGiftPackPop.instance,
                    () =>
                    {
                        dealCloseNode();
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
            GGUIWndPushGiftPackPop.instance.regLoadDoneDelegate(() =>
            {
                // 序列号校验，避免异步回调时状态已变化
                if (serializeId != _m_lEnterNodeSerializeId)
                    return;
                
                // 模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                if (_m_bNeedShowBk && _m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), GGUIWndPushGiftPackPop.instance.getGameObj());
                else
                    // 没有遮罩就只将窗口移到最前
                    GCommon.moveTransformToLastAndRefreshLayer(GGUIWndPushGiftPackPop.instance.getGameObj());
                
                GGUIWndPushGiftPackPop.instance.setData(_m_pushGiftPackInfo);
                GGUIWndPushGiftPackPop.instance.showWnd();
            });
        }

        
        
        public void dealCloseNode()
        {
            // 自动触发推送礼包展示规则修改, 不需要紧接着上一个礼包购买完毕后就立刻弹出新触发的礼包
            
            // // 若礼包购买次数已用完，尝试触发下一个推送礼包
            // if (_m_pushGiftPackInfo != null && _m_pushGiftPackInfo.leftCanBuyCount <= 0)
            // {
            //     long serializeId = _m_lEnterNodeSerializeId;
            //     _m_pushGiftPackInfo.tryAfterBuyAutoTriggerNextPushGiftPack(() =>
            //     {
            //         if(_m_lEnterNodeSerializeId != serializeId)
            //             return;
            //         
            //         // 若礼包所属礼包组的当前推送礼包无效，则关闭窗口
            //         if (_m_pushGiftPackInfo == null || _m_pushGiftPackInfo.pushGiftGroupInfo == null ||
            //             _m_pushGiftPackInfo.pushGiftGroupInfo.curPushGiftPackInfo == null || !_m_pushGiftPackInfo.pushGiftGroupInfo.curPushGiftPackInfo.isValid)
            //         {
            //             QueueMgr.instance.forceCloseNode(this);
            //         }
            //         else//否则
            //         {
            //             _m_pushGiftPackInfo = _m_pushGiftPackInfo.pushGiftGroupInfo.curPushGiftPackInfo;
            //             // 设置当前推送礼包信息并刷新窗口
            //             GGUIWndPushGiftPackPop.instance.setData(_m_pushGiftPackInfo);
            //             GGUIWndPushGiftPackPop.instance.showWnd();
            //         }
            //     });
            // }
            // else
            // {
                QueueMgr.instance.forceCloseNode(this);
            // }
        }
    }
}