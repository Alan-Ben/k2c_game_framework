using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 最后一击结果弹窗
    /// </summary>
    public class GGUIWndEveningDungeonFinalAttackResult : _ANPGGUIBasicWnd<GGUIMonoEveningDungeonFinalAttackResult>
    {
        private static GGUIWndEveningDungeonFinalAttackResult _g_instance;
        public static GGUIWndEveningDungeonFinalAttackResult instance { get { return _g_instance ??= new GGUIWndEveningDungeonFinalAttackResult(); } }
        
        private NPGGUIWndPlayerIcon _m_wPlayerInfo;//最后一击玩家信息
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;//奖励列表

        private long _m_lShowSerialize;//显示的序列号，用于防止自动关闭回调串号
        
        public GGUIWndEveningDungeonFinalAttackResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonFinalAttackResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonFinalAttackResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.monoPlayerInfo != null)
                _m_wPlayerInfo = new NPGGUIWndPlayerIcon(wnd.monoPlayerInfo);
            
            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);

            _m_wPlayerInfo?.discard();
            _m_wPlayerInfo = null;
            
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            // 每次显示时刷新序列号，使上一次的自动关闭任务回调失效
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            // 刷新序列号，使未触发的自动关闭回调失效
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_wPlayerInfo?.hideWnd();
            _m_wRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerInfo?.resetWnd();
            _m_wRewardContainer?.resetWnd();
        }

        /// <param name="_autoCloseDelay">自动关闭延迟秒数，小于等于0表示不自动关闭</param>
        public void setShowInfo(List<NPCommon.NPCommon_ItemInfo> _rewardList, float _autoCloseDelay = -1f)
        {
            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(_rewardList.toItemDataList());       
            }

            if (_m_wPlayerInfo != null)
            {
                _m_wPlayerInfo.showWnd();
                _m_wPlayerInfo.setSelfInfo();
            }

            // 若有自动关闭要求，延迟指定时间后关闭窗口
            if (_autoCloseDelay > 0)
            {
                long serialize = _m_lShowSerialize;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    // 序列号不一致说明窗口已被手动关闭或重新打开，不再执行自动关闭
                    if (serialize != _m_lShowSerialize)
                        return;

                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_KILLED_RESULT);
                }, _autoCloseDelay);
            }
        }
        
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_KILLED_RESULT);
        }
    }
}