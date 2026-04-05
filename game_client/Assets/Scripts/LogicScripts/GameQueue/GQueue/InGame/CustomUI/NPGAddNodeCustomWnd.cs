using System;
using UnityEngine;
using ALPackage;

namespace GOE
{
    public class NPGAddNodeCustomWnd : UIQueueBaseNode
    {


        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return _m_bIsCanRollBackQuit; } }

        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return _m_bNeedAutoRemove; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return _m_bIsLastEnter; } }

        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        private NPGGUIWndCustom _m_wWnd;

        private EALUIWndLayer _m_eWndLayer;
        private bool _m_bIsCanRollBackQuit;
        private bool _m_bNeedAutoRemove;
        private bool _m_bIsLastEnter;
        private string _m_sAssetPath;
        private string _m_sObjName;
        //半透背景
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;

        private long _m_lSerialize;

        private Action _m_aOnCloseNode;

        public NPGAddNodeCustomWnd(string _monoAssetPath, string _monoObjName, Action _onCloseNode = null)
            : base(EUIQueueStageType.MAIN)
        {
            //跳转默认使用必关方式
            _m_bIsCanRollBackQuit = true;
            _m_bNeedAutoRemove = true;
            _m_bIsLastEnter = false;
            _m_sAssetPath = _monoAssetPath;
            _m_sObjName = _monoObjName;
            _m_aOnCloseNode = _onCloseNode;
        }
        
        public NPGAddNodeCustomWnd(string _monoAssetPath, string _monoObjName, EALUIWndLayer _wndLayer, Action _onCloseNode = null)
            : base(EUIQueueStageType.MAIN)
        {
            //跳转默认使用必关方式
            _m_bIsCanRollBackQuit = true;
            _m_bNeedAutoRemove = true;
            _m_bIsLastEnter = false;
            _m_sAssetPath = _monoAssetPath;
            _m_sObjName = _monoObjName;
            _m_eWndLayer = _wndLayer;
            _m_aOnCloseNode = _onCloseNode;
        }

        public override void EnterNode()
        {
            //显示背景
            if (null != _m_wTransBk)
            {
                _m_wTransBk.showWnd();
                _afterTransBkAction();
            }
            else
            {
                _m_lSerialize = ALSerializeOpMgr.next();
                long serialize = _m_lSerialize;
                
                _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                    _m_wWnd
                    , () => { QueueMgr.instance.forceCloseNode(this); }
                    , () =>
                    {
                        if(serialize != _m_lSerialize)
                            return;
                        _afterTransBkAction();
                    });
            }
        }

        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            //显示窗口
            _m_wWnd = NPGGUICustomWndMgr.instance.showWnd(_m_sAssetPath, _m_sObjName, _m_eWndLayer);
            if (null != _m_wWnd)
            {
                _m_wWnd.regLoadDoneDelegate(()=>
                {
                    //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                    if (_m_wTransBk != null)
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), _m_wWnd.getGameObj());
                    else
                        //只将窗口移到最前
                        GCommon.moveTransformToLastAndRefreshLayer(_m_wWnd.getGameObj());

                    _refreshNodeProperty();
                });
            }
        }

        /***********
         * 加载完成后，根据窗口配置设置本节点属性
         **/
        protected void _refreshNodeProperty()
        {
            if(null == _m_wWnd || null == _m_wWnd.wnd)
                return;

            _m_bIsCanRollBackQuit = !_m_wWnd.wnd.CanNotRollBackQuit;
            _m_bNeedAutoRemove = _m_wWnd.wnd.NeedAutoRemove;
            _m_bIsLastEnter = _m_wWnd.wnd.IsDoLastNodeEnter;
        }

        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();

            //隐藏背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;

            //隐藏窗口
            NPGGUICustomWndMgr.instance.hideWnd(_m_wWnd);
        }

        public override void onClose()
        {
            base.onClose();
            
            _m_aOnCloseNode?.Invoke();
            _m_aOnCloseNode = null;
        }
    }
}
