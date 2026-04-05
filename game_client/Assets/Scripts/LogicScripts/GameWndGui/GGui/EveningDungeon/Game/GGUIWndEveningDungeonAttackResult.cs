using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 攻击结果弹窗
    /// </summary>
    public class GGUIWndEveningDungeonAttackResult : _ANPGGUIBasicWnd<GGUIMonoEveningDungeonAttackResult>
    {
        private static GGUIWndEveningDungeonAttackResult _g_instance;
        public static GGUIWndEveningDungeonAttackResult instance { get { return _g_instance ??= new GGUIWndEveningDungeonAttackResult(); } }
        
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;//奖励列表

        private long _m_lShowSerialize;//显示的序列号
        
        public GGUIWndEveningDungeonAttackResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEveningDungeonAttackResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEveningDungeonAttackResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_wRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardContainer?.resetWnd();
        }
        
        public void setData(List<NPCommon.NPCommon_ItemInfo> _rewardList, long _damageValue, float _autoCloseDelay = -1f)
        {
            if(wnd == null)
                return;
            
            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(_rewardList.toItemDataList());       
            }

            ALUGUICommon.setLabelTxt(wnd.txtDamage, _damageValue.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            // 若有自动关闭窗口要求
            if (_autoCloseDelay > 0)
            {
                long serialize = _m_lShowSerialize;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialize != _m_lShowSerialize)
                        return;
                    
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_ATTACK_RESULT);
                }, _autoCloseDelay);
            }
        }
        
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EVENING_DUNGEON_ATTACK_RESULT);
        }
    }
}