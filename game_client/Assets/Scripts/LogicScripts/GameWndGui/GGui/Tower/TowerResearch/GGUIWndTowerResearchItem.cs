using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndTowerResearchItem : _ATALBasicUISubWnd<GGUIMonoTowerResearchItem>
    {
        private TowerChapterRefObj _m_chapter;
        private bool _m_showLevel;
        private NPGGuiWndTexture _m_bgImg;
        private GGUIWndTowerResearchLevelItemContainer _m_levelContainer;
        private GGUIWndCommonRewardContainer _m_rewardContainer;
        private NPGGUIWndCommonTab _m_toggleShowLevel;
        private ECommonRewardType _m_rewardType = ECommonRewardType.NOT_GET_REWARD;

        public GGUIWndTowerResearchItem(GGUIMonoTowerResearchItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_TOWER_ACTIVE_POS_CHG, _onActivePosChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_TOWER_ACTIVE_POS_CHG, _onActivePosChg);
            _m_bgImg?.discardTexture();
        }

        protected override void _onReset()
        {
            _m_bgImg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_levelContainer?.discard();
            _m_levelContainer = null;
            _m_rewardContainer?.discard();
            _m_rewardContainer = null;
            _m_toggleShowLevel?.discard();
            _m_toggleShowLevel = null;
            _m_bgImg?.discard();
            _m_bgImg = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onBtnGetReward);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if(wnd.levelContainer != null)
                _m_levelContainer = new GGUIWndTowerResearchLevelItemContainer(wnd.levelContainer);
            if(wnd.rewardContainer != null)
                _m_rewardContainer = new GGUIWndCommonRewardContainer(wnd.rewardContainer);
            if (wnd.toggleShowLevel != null)
            {
                _m_toggleShowLevel = new NPGGUIWndCommonTab(wnd.toggleShowLevel);
                _m_toggleShowLevel.clickDelegate += _onToggleShowLevel;
            }
            if(wnd.texBg != null)
                _m_bgImg = new NPGGuiWndTexture(wnd.texBg);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onBtnGetReward);
        }

        private void _onBtnGetReward(GameObject _obj)
        {
            if(_m_chapter == null)
                return;
            switch (_m_rewardType)
            {
                case ECommonRewardType.NOT_GET_REWARD:
                case ECommonRewardType.HAS_GET_REWARD:
                    if(wnd != null)
                        QueueMgr.instance.AddNode(
                            new GNodeCommonToolTip_Reward( 5317, "", "",_m_chapter.research_finish_reward, wnd.toolTipsRoot, Vector2.zero, _m_rewardType));
                    break;
                case ECommonRewardType.CAN_GET_REWARD:
                    NPPlayer.instance.towerComp.reqTowerDrawResearchReward(_m_chapter.id, _refreshReward);
                    break;
            }
        }

        private void _onToggleShowLevel(bool _isOn)
        {
            _m_showLevel = !_m_showLevel;
            _refreshLevelShow();
        }

        /// <summary>
        /// 当激活位置发生变化时调用
        /// </summary>
        private void _onActivePosChg()
        {
            _refreshProgress();
        }

        public void setInfo(TowerChapterRefObj _data, bool _showLevel)
        {
            _m_chapter = _data;
            _m_showLevel = _showLevel;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd || null == _m_chapter)
                return;
           
            if (_m_bgImg != null)
            {
                _m_bgImg.showWnd();
                _m_bgImg.setTexture(_m_chapter.banner_tex);
            }

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_chapter.name));

            _refreshReward();
            
            _refreshProgress();
            
            _refreshLevelShow();
        }

        /// <summary>
        /// 刷新领奖状态
        /// </summary>
        private void _refreshReward()
        {
            if (wnd == null || _m_chapter == null)
                return;

            _m_rewardType = ECommonRewardType.NOT_GET_REWARD;
            if (NPPlayer.instance.towerComp.canGetResearchReward(_m_chapter.id))
            {
                if(NPPlayer.instance.towerComp.hasGetResearchReward(_m_chapter.id))
                    _m_rewardType = ECommonRewardType.HAS_GET_REWARD;
                else
                    _m_rewardType = ECommonRewardType.CAN_GET_REWARD;
            }
            NPCommonEnumStatInfo<ECommonRewardType>.setStat(wnd.rewardStatList, _m_rewardType);
            _m_rewardContainer?.setRewardList(_m_chapter.research_finish_reward, _m_rewardType);

        }

        /// <summary>
        /// 刷新激活进度
        /// </summary>
        private void _refreshProgress()
        {
            if (null == wnd || null == _m_chapter)
                return;
            // 计算激活的研究加成，以及是否全部激活
            TowerPosInfo posInfo = NPPlayer.instance.towerComp.activatePosInfo;
            TowerPosInfo achievePosInfo = NPPlayer.instance.towerComp.maxAchievedPosInfo;
            long chapterBonus = 0;
            bool isActivateAll = false;
            if (posInfo.chapterId > _m_chapter.id)
            {
                var researchRef = _m_chapter.research_list.GetLast();
                if (researchRef != null) chapterBonus = researchRef.building_profit_add_per;
                isActivateAll = true;
            }
            else if(posInfo.chapterId == _m_chapter.id)
            {
                isActivateAll = true;
                foreach (TowerResearchRefObj researchRef in _m_chapter.research_list)
                {
                    if (researchRef == null) continue;

                    if (researchRef.level <= posInfo.level)
                    {
                        chapterBonus = researchRef.building_profit_add_per;
                    }
                    else
                    {
                        isActivateAll = false;
                        break;
                    }
                }
            }

            TowerChapterRefObj preChapterRef = GRefdataCoreMgr.instance.getTowerPreChapterRef(_m_chapter.id);
            if (preChapterRef != null && chapterBonus != 0)
            {
                chapterBonus -= preChapterRef.research_list.GetLast().building_profit_add_per;
            }

            float progress = 0f;
            if (achievePosInfo.chapterId > _m_chapter.id)
            {
                progress = 1f;
            }
            else if (achievePosInfo.chapterId == _m_chapter.id)
            {
                int totalCount = _m_chapter.research_list.Count;
                int activeCount = 0;
                
                TowerResearchRefObj lastResearchRefObj = null;
                float lastPos = 0;
                foreach (TowerResearchRefObj researchRef in _m_chapter.research_list)
                {
                    if (researchRef == null) continue;
                    if (achievePosInfo.level < researchRef.level)
                    {
                        if(lastResearchRefObj == null)
                            lastPos = (float)(achievePosInfo.level) / researchRef.level * wnd.firstItemHeight;
                        else
                            lastPos = (float)(achievePosInfo.level - lastResearchRefObj.level) / (researchRef.level - lastResearchRefObj.level) * wnd.commonItemHeight;
                        break;
                    }
                    lastResearchRefObj = researchRef;
                    activeCount++;
                }
                if(activeCount <= 0)
                {
                    progress = 0f;
                }
                else if (activeCount >= totalCount)
                {
                    progress = 1f;
                }
                else
                {
                    progress = (wnd.firstItemHeight + (activeCount - 1) * wnd.commonItemHeight + lastPos) / (wnd.firstItemHeight + (totalCount - 1) * wnd.commonItemHeight);

                }
                
            }

            ALUGUICommon.setGameObjEnable(wnd.canActivateShowGos, NPPlayer.instance.towerComp.getChapterActiveState(_m_chapter.id) == ETowerResearchActiveType.CAN_ACTIVATE);
            ALUGUICommon.setGameObjEnable(wnd.activateAllShowGos, isActivateAll);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.tower_research_item_addPropPer_num, chapterBonus/100f));
            if (wnd.progressSlider != null) wnd.progressSlider.normalizedValue = progress;
        }

        /// <summary>
        /// 刷新关卡列表
        /// </summary>
        private void _refreshLevelShow()
        {
            _m_toggleShowLevel?.setSelected(_m_showLevel);
            if (_m_showLevel)
            {
                if (_m_levelContainer != null)
                {
                    _m_levelContainer.showWnd();
                    if (_m_chapter != null) _m_levelContainer.showItemList(_m_chapter);
                }
            }
            else
                _m_levelContainer?.hideWnd();

        }
    }
}
