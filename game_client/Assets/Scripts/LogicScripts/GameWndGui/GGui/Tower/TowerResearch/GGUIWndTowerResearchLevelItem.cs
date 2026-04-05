using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndTowerResearchLevelItem : _ATALBasicUISubWnd<GGUIMonoTowerResearchLevelItem>
    {
        private TowerChapterRefObj _m_towerChapterRef;
        private TowerResearchRefObj _m_towerResearchRef;
        private ETowerResearchActiveType _m_activeType;
        public GGUIWndTowerResearchLevelItem(GGUIMonoTowerResearchLevelItem _wnd) : base(_wnd)
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
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnActivate, _onBtnActivate);
        }

        public void setInfo(TowerChapterRefObj _chapterRef, TowerResearchRefObj _data)
        {
            _m_towerChapterRef = _chapterRef;
            _m_towerResearchRef = _data;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd || null == _m_towerResearchRef || null == _m_towerChapterRef)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtLevelName, TextTranslate.instance.getLanguage(TransKeyConst.tower_research_level_name, _m_towerChapterRef.getTotalLevel(_m_towerResearchRef.level)));
            
            _m_activeType = NPPlayer.instance.towerComp.getLevelActiveState(_m_towerChapterRef.id, _m_towerResearchRef.level);
            
            _freshDesc();

            switch (_m_activeType)
            {
                case ETowerResearchActiveType.NOT_ACTIVATE:
                    if (wnd.activateAnim != null) 
                        wnd.activateAnim.Sample(wnd.activateAnimName, 0);
                    ALUGUICommon.setGameObjEnable(wnd.canActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.hasActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.notActivateGoList, true);
                    break;
                case ETowerResearchActiveType.CAN_ACTIVATE:
                    if (wnd.activateAnim != null)
                        wnd.activateAnim.Sample(wnd.activateAnimName, 0);
                    ALUGUICommon.setGameObjEnable(wnd.notActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.hasActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.canActivateGoList, true);
                    break;
                case ETowerResearchActiveType.HAS_ACTIVATE:
                    if (wnd.activateAnim != null)
                        wnd.activateAnim.Sample(wnd.activateAnimName, 1);
                    ALUGUICommon.setGameObjEnable(wnd.canActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.notActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.hasActivateGoList, true);
                    break;
            }
        }
        
        private void _onBtnActivate(GameObject _obj)
        {
            if (_m_towerResearchRef == null) return;

            TowerPosInfo activePos = NPPlayer.instance.towerComp.activatePosInfo;
            if (activePos == null)
                return;
            // 如果当前激活位置的章节ID小于已经激活的章节ID，或者章节ID相同但当前研究的等级小于等于激活位置的等级，则不需要处理直接返回
            if(_m_towerResearchRef.chapter_id < activePos.chapterId || (_m_towerResearchRef.chapter_id == activePos.chapterId && _m_towerResearchRef.level <= activePos.level) )
            {
                return;
            }
            // 如果当前激活位置的章节ID大于已经激活的章节ID，则需要判断当前激活位置的上一个章节是否已经全部激活，如果不是则需要提示先激活上一个章节
            if (_m_towerResearchRef.chapter_id > activePos.chapterId)
            {
                long preChapterId = GRefdataCoreMgr.instance.getTowerPreChapterId(_m_towerResearchRef.chapter_id);
                if (preChapterId > 0)
                {
                    if (NPPlayer.instance.towerComp.getChapterActiveState(preChapterId) !=
                        ETowerResearchActiveType.HAS_ACTIVATE)
                    {
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.tower_research_active_need_pre_chapter_tip));
                        return;
                    }
                }
            }

            NPPlayer.instance.towerComp.reqTowerResearchActive(_m_towerResearchRef.chapter_id, _m_towerResearchRef.level);
        }

        private void _freshDesc()
        {
            long bonus = _m_towerResearchRef.building_profit_add_per;
            TowerChapterRefObj preChapterRef = GRefdataCoreMgr.instance.getTowerPreChapterRef(_m_towerChapterRef.id);
            if (preChapterRef != null)
                bonus -= preChapterRef.research_list.GetLast().building_profit_add_per;
            
            
            ALUGUICommon.setLabelTxt(wnd.txtEffectDesc,
                TextTranslate.instance.getLanguage(_m_activeType == ETowerResearchActiveType.HAS_ACTIVATE ? TransKeyConst.tower_research_active_desc : TransKeyConst.tower_research_notactive_desc, 
                    TextTranslate.instance.getLanguage(_m_towerChapterRef.name),
                     bonus / 100));
        }
        
        /// <summary>
        /// 当激活位置发生变化时调用
        /// </summary>
        private void _onActivePosChg()
        {
            ETowerResearchActiveType oldActiveType = _m_activeType;

            if (_m_towerResearchRef != null)
                _m_activeType = NPPlayer.instance.towerComp.getLevelActiveState(_m_towerResearchRef.chapter_id, _m_towerResearchRef.level);

            _freshDesc();
            
            if(oldActiveType == _m_activeType)
                return;
            switch (_m_activeType)
            {
                case ETowerResearchActiveType.NOT_ACTIVATE:
                    ALUGUICommon.setGameObjEnable(wnd.canActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.hasActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.notActivateGoList, true);
                    break;
                case ETowerResearchActiveType.CAN_ACTIVATE:
                    ALUGUICommon.setGameObjEnable(wnd.notActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.hasActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.canActivateGoList, true);
                    break;
                case ETowerResearchActiveType.HAS_ACTIVATE:
                    ALUGUICommon.setGameObjEnable(wnd.canActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.notActivateGoList, false);
                    ALUGUICommon.setGameObjEnable(wnd.hasActivateGoList, true);
                    break;
            }
            if (oldActiveType == ETowerResearchActiveType.CAN_ACTIVATE &&
                _m_activeType == ETowerResearchActiveType.HAS_ACTIVATE)
            {
                if (wnd != null && wnd.activateAnim != null)
                {
                    wnd.activateAnim.Sample(wnd.activateAnimName, 0);
                    wnd.activateAnim.Play(wnd.activateAnimName);
                }
            }
        }
    }
}
