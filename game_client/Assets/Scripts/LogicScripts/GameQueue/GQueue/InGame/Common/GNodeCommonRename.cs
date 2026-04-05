using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 通用取名节点
    /// </summary>
    public class GNodeCommonRename : BaseQueueNode
    {
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        private bool _m_bNeedShowBg;
        private GGUIWndCommonRename _m_wRenameWnd;
        private string _m_sCurrentName;
        private List<NPCommonCostItem> _m_lCostItemList;
        private WCGIntRange _m_wNameRange;
        private Func<string> _m_getRandomNameFunc;
        private Action<string> _m_confirmAction;
        private Action _m_aOnShow;
        private bool _m_bCanRollBackQuit;
        private long _m_lSerialize;

        public GNodeCommonRename(long _renameAccessPathId, bool _needShowBg, string _currentName, List<NPCommonCostItem> _costItemList, WCGIntRange _nameRange, Func<string> _getRandomNameFunc, Action<string> _confirmAction, Action _onShow, bool _canRollBack) : base(EUIQueueStageType.MAIN, UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_RENAME_NODE)
        {
            _m_wRenameWnd = new GGUIWndCommonRename(_renameAccessPathId);
            _m_bNeedShowBg = _needShowBg;
            _m_sCurrentName = _currentName;
            _m_lCostItemList = _costItemList;
            _m_wNameRange = _nameRange;
            _m_getRandomNameFunc = _getRandomNameFunc;
            _m_confirmAction = _confirmAction;
            _m_aOnShow = _onShow;
            _m_bCanRollBackQuit = _canRollBack;
        }

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
        public override bool IsCanRollBackQuit { get { return _m_bCanRollBackQuit; } }

        public override void onEnterQueue()
        {
            if (_m_wRenameWnd != null)
                _m_wRenameWnd.load();
        }

        public override void onClose()
        {
            if (_m_wRenameWnd != null)
                _m_wRenameWnd.discard();
            _m_wRenameWnd = null;
        }

        public override void EnterNode()
        {
            if (_m_bNeedShowBg)
            {
                if (null != _m_wTransBk)
                {
                    _m_wTransBk.showWnd();
                }
                else
                {
                    _m_lSerialize = ALSerializeOpMgr.next();
                    long serialize = _m_lSerialize;
                    _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                        _m_wRenameWnd
                        , () => { QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_RENAME_NODE); }
                        , () =>
                        {
                            if (serialize != _m_lSerialize)
                                return;

                            _afterTransBkAction();
                        });
                }
            }
            else
            {
                _afterTransBkAction();
            }
        }

        public override void QuitNode()
        {
            _m_lSerialize = ALSerializeOpMgr.next();
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;
        }

        public override void doQuitNode(Action _dealOnQuitDone)
        {
            _m_wRenameWnd?.hideWnd(() =>
            {
                base.doQuitNode(_dealOnQuitDone);
            });
        }

        /// <summary>
        /// 透明背景处理后的实际窗口显示处理
        /// </summary>
        protected void _afterTransBkAction()
        {
            _m_wRenameWnd?.regLoadDoneDelegate(() =>
            { 
                //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                if (_m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), _m_wRenameWnd.getGameObj());
                else
                    //将窗口移到最前
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wRenameWnd.getGameObj());

                _m_wRenameWnd?.showWnd(_m_aOnShow);
                _m_wRenameWnd?.setData(_m_sCurrentName, _m_lCostItemList, _m_wNameRange, _m_getRandomNameFunc, _m_confirmAction);
            });
        }
    }
}