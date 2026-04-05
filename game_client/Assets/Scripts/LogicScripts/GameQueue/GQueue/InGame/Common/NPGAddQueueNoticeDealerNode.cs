using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGAddQueueNoticeDealerNode : UIQueueBaseNode
    {
        private NPUINoticeMgr._ANPUINoticeDealer _m_ndNoticeDealer;
        //Notice显示序列号，避免重置的时候设置完成
        private long _m_lNoticeDealSerialize;

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return null == _m_ndNoticeDealer ? true : _m_ndNoticeDealer.noticeCanDoESC; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return null == _m_ndNoticeDealer ? false : _m_ndNoticeDealer.needAutoRemove; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return null == _m_ndNoticeDealer ? false : _m_ndNoticeDealer.isNoticeFullScreen; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        //是否是纯ui界面
        public override bool isOnlyUINode  { get { return null == _m_ndNoticeDealer ? false : _m_ndNoticeDealer.isOnlyUINode; } }

        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        private long _m_lSerialize;

        public NPGAddQueueNoticeDealerNode(NPUINoticeMgr._ANPUINoticeDealer _noticeDealer)
            : base(EUIQueueStageType.MAIN, _noticeDealer.nodeTag)
        {
            _m_ndNoticeDealer = _noticeDealer;
            _m_lNoticeDealSerialize = _m_ndNoticeDealer.noticeDealSerialize;
            _m_lSerialize = ALSerializeOpMgr.next();
        }

        public override void EnterNode()
        {
            //处理进入展示
            if (null != _m_ndNoticeDealer)
            {
                //判断是否需要背景蒙版
                if (_m_ndNoticeDealer.needTransBk)
                {
                    if (null != _m_wTransBk)
                    {
                        _m_wTransBk.showWnd();
                        _m_ndNoticeDealer.dealShowNotice();
                    }
                    else
                    {
                        _m_lSerialize = ALSerializeOpMgr.next();
                        long serialize = _m_lSerialize;
                        _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                            _m_ndNoticeDealer.uiObj,
                            _m_ndNoticeDealer.clickBkAction
                            , () =>
                            {
                                if(serialize != _m_lSerialize)
                                    return;
                                
                                if (null != _m_wTransBk && _m_wTransBk.wnd != null)
                                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.wnd.transform);
                                
                                //TODO 这边窗口需要每次load跟discard，不然hide再show层级会有问题
                                _m_ndNoticeDealer.dealShowNotice();
                            });
                    }
                }
                else
                {
                    _m_ndNoticeDealer.dealShowNotice();
                }

                // //设置是否可以esc
                // if(!_m_ndNoticeDealer.noticeCanDoESC)
                //     QueueMgr.instance.CloseRollBack(NodeESC_Const.C_QUEUE_ESC_CONTROLLER_NOTICE);
            }
        }

        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
            
            if(null != _m_ndNoticeDealer)
            {
                _m_ndNoticeDealer.dealHideNotice();
            }

            // //设置是否可以esc
            // QueueMgr.instance.OpenRollBack(NodeESC_Const.C_QUEUE_ESC_CONTROLLER_NOTICE);
        }

        //TODO 这边setDealerDone的时候，还有回问题被规避了，因为notice底层NeedAutoRemove配的是False，如果是True的话两个notice存在，然后开一个node的情况，还是会notice覆盖node出来
        public override void onClose()
        {
            //设置阶段完成
            if (_m_ndNoticeDealer != null) 
                _m_ndNoticeDealer.setDealerDone(_m_lNoticeDealSerialize);
        }

        public override void onCannotEscBack()
        {
            if (_m_ndNoticeDealer != null) 
                _m_ndNoticeDealer.onCannotEscBack();
        }
    }
}
