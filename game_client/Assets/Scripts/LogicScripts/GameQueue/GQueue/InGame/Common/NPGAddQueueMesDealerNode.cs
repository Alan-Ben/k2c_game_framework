using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGAddQueueMesDealerNode : UIQueueBaseNode
    {
        private _ANPMesDealer _m_mdMesDealer;

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return true; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        public override bool NeedRemovePreAutoRemove { get { return false; } }
        
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        private long _m_lSerialize;
        
        public NPGAddQueueMesDealerNode(_ANPMesDealer _mesDealer)
            : base(EUIQueueStageType.MAIN)
        {
            _m_mdMesDealer = _mesDealer;
            _m_lSerialize = ALSerializeOpMgr.next();
            if(_mesDealer != null)
                setNodeTag(_mesDealer.uiNodeTag);
        }

        public override void EnterNode()
        {
            //处理进入展示
            if (null != _m_mdMesDealer)
            {
                //判断是否需要背景蒙版
                if (_m_mdMesDealer.needTransBk)
                {
                    if (null != _m_wTransBk)
                    {
                        _m_wTransBk.showWnd();
                        _m_mdMesDealer.dealShowMes();
                    }
                    else
                    {
                        _m_lSerialize = ALSerializeOpMgr.next();
                        long serialize = _m_lSerialize;
                        _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                            () =>
                            {
                                _m_mdMesDealer?.dealClickTransBk();
                                QueueMgr.instance.forceCloseNode(this);
                            }
                            , () =>
                            {
                                if(serialize != _m_lSerialize)
                                    return;
                                if (null != _m_wTransBk && _m_wTransBk.wnd != null)
                                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.wnd.transform);
                                
                                _m_mdMesDealer.dealShowMes();
                            });
                    }
                }
                else
                {
                    _m_mdMesDealer.dealShowMes();
                }


            }
        }

        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;

            if (null != _m_mdMesDealer)
            {
                //消息已完成，直接隐藏
                if (_m_mdMesDealer.isDone)
                {
                    _m_mdMesDealer.dealHideMes();
                }
                //消息未完成，先处理关闭
                else
                {
                    //处理关闭消息
                    _m_mdMesDealer.dealCloseMes();
                    //隐藏
                    _m_mdMesDealer.dealHideMes();
                    //设置阶段完成
                    _m_mdMesDealer.setDealerDone();
                }
                
            }
        }
    }
}
