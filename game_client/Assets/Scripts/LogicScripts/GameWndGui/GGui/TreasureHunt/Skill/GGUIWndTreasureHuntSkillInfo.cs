using System;
using System.Linq;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 技能信息子窗口
    /// </summary>
    public class GGUIWndTreasureHuntSkillInfo<T> : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntSkillInfo>
        where T : GGUIMonoTreasureHuntSkillInfo
    {
        protected _ITreasureHuntSkillInfo _m_iSkillInfo;
        protected NPCommonCostItem _m_iSkillPointItem; // 技能点item
        
        protected NPGGuiWndTexture _m_wSkillIcon;
        protected NPGGUIWndCommonItem _m_wSkillPointItem;

        public GGUIWndTreasureHuntSkillInfo(T _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<_ITreasureHuntSkillInfo> onSkillOpClick; // 当技能操作按钮被点击

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.skillIcon != null)
                _m_wSkillIcon = new NPGGuiWndTexture(wnd.skillIcon);

            if (wnd.monoSkillPointItem != null)
                _m_wSkillPointItem = new NPGGUIWndCommonItem(wnd.monoSkillPointItem);

            ALUGUICommon.combineBtnClick(wnd.btnOp, _onClickOp);
        }
        
        protected override void _onDiscard()
        {
            _m_iSkillInfo = null;
            _m_iSkillPointItem = null;
            
            onSkillOpClick = null;

            _m_wSkillIcon?.discardTexture();
            _m_wSkillIcon = null;

            _m_wSkillPointItem?.discard();
            _m_wSkillPointItem = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemCountChg);

            _m_wSkillIcon?.hideWnd();
            _m_wSkillPointItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSkillIcon?.discardTexture();
            _m_wSkillPointItem?.resetWnd();
        }

        public void setData(_ITreasureHuntSkillInfo _skillInfo)
        {
            _m_iSkillInfo = _skillInfo;
            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || _m_iSkillInfo == null || _m_iSkillInfo.skillRefObj == null)
                return;

            TreasureHuntSkillRefObj skillRefObj = _m_iSkillInfo.skillRefObj;
            ETreasureHuntSkillState skillState = _m_iSkillInfo.skillState;

            // 设置技能图标
            if (_m_wSkillIcon != null)
            {
                _m_wSkillIcon.showWnd();
                _m_wSkillIcon.setTexture(skillRefObj.icon);
            }

            // 设置当前等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level2_num, _m_iSkillInfo.level));

            // 设置升级消耗物品
            _refreshSkillPointItem();

            _onRefreshWnd();
            
            // 刷新技能状态显示
            wnd.refreshSkillState(skillState, out TreasureHuntSkillStateShow _stateShow);
            
            TreasureHuntSkillLevelRefObj skillLevelRefObj =
                (skillState is ETreasureHuntSkillState.UNLOCK_ACTIVATE or ETreasureHuntSkillState.UNLOCK_ACTIVATE_MAX_LEVEL)
                    ? _m_iSkillInfo.skillLevelRefObj : GRefdataCoreMgr.instance.getTreasureHuntSkillLevelRefObj(_m_iSkillInfo.skillRefObj.id, 1);
            // 设置当前等级技能和下一技能等级效果描述
            if (skillLevelRefObj != null)
            {
                string skillDesc = TextTranslate.instance.getLanguage(_m_iSkillInfo.skillRefObj.desc, skillLevelRefObj.skill_desc_args_list);
                ALUGUICommon.setLabelTxt(wnd.nowLevelSkillDesc, GCommon.addColorForRichText(skillDesc, _stateShow?.skillDescColor ?? Color.white));

                string nextLevelSkillDescKey = string.IsNullOrEmpty(wnd.nextLevelSkillDescKey) ? TransKeyConst.common_value : wnd.nextLevelSkillDescKey;
                ALUGUICommon.setLabelTxt(wnd.nextLevelSkillDesc, TextTranslate.instance.getLanguage(nextLevelSkillDescKey, skillLevelRefObj.next_level_add_value_desc));
            }
        }

        protected virtual void _onRefreshWnd()
        {
        }
        
        /// <summary>
        /// 刷新升级消耗物品
        /// </summary>
        private void _refreshSkillPointItem()
        {
            if (_m_wSkillPointItem == null || _m_iSkillInfo == null || 
                _m_iSkillInfo.skillRefObj == null || _m_iSkillInfo.skillLevelRefObj == null)
                return;

            if (_m_iSkillInfo.skillPointItem == null)
            {
                _m_wSkillPointItem.hideWnd();
                return;
            }
            
            // 设置升级需要消耗的物品
            if (_m_iSkillPointItem == null)
                _m_iSkillPointItem = new NPCommonCostItem(_m_iSkillInfo.skillPointItem, _m_iSkillInfo.skillLevelRefObj.upgrade_cost_num);
            else
            {
                _m_iSkillPointItem.item = _m_iSkillInfo.skillPointItem;
                _m_iSkillPointItem.count = _m_iSkillInfo.skillLevelRefObj.upgrade_cost_num;
            }
            
            _m_wSkillPointItem.showWnd();
            _m_wSkillPointItem.setItem(_m_iSkillPointItem, _m_iSkillInfo.skillPointNum);

            bool canLevelUp = TreasureHuntUtil.checkCanUpgradeSkill(_m_iSkillInfo);
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.canLevelUpShowGoList, canLevelUp);
                ALUGUICommon.setGameObjEnable(wnd.cannotLevelUpShowGoList, !canLevelUp);       
            }
        }

        /// <summary>
        /// 技能激活表现
        /// </summary>
        public void showSkillActive()
        {
            if(wnd == null || !isShow)
                return;
            
            _playAnimation(wnd.wndAnimation, wnd.activeAnimationName);
            refreshWnd();
        }

        /// <summary>
        /// 技能升级表现
        /// </summary>
        public void showSkillLevelUp()
        {
            if(wnd == null || !isShow)
                return;
            
            _playAnimation(wnd.wndAnimation, wnd.levelUpAnimationName);
            refreshWnd();
        }
        
        /// <summary>
        /// CommonItem数量变化时的回调
        /// </summary>
        private void _onCommonItemCountChg(params object[] _objs)
        {
            if(_m_iSkillInfo == null || _m_iSkillInfo.skillPointItem == null || _objs == null || _objs.Length < 3 || 
               !(_objs[0] is ENPItemType itemType) || !(_objs[1] is long subId) || !(_objs[2] is long count) || 
               _m_iSkillInfo.skillPointItem.itemType != itemType || _m_iSkillInfo.skillPointItem.itemId != subId)
                return;

            _refreshSkillPointItem();
        }
        
        private void _onClickOp(GameObject _go)
        {
            onSkillOpClick?.Invoke(_m_iSkillInfo);
        }

        #region 动画

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_aniName"></param>
        /// <param name="_playDone"></param>
        private void _playAnimation(Animation _ani, string _aniName, Action _playDone = null)
        {
            if (_ani == null || string.IsNullOrEmpty(_aniName))
            {
                _playDone?.Invoke();
                return;
            }

            _ani.Play(_aniName, _playDone);
        }

        private void _sampleAnimation(Animation _ani, string _aniName, float _normalizedTime = 0f)
        {
            if (_ani == null || string.IsNullOrEmpty(_aniName))
                return;

            _ani.Sample(_aniName, _normalizedTime);
        }

        #endregion
    }
}