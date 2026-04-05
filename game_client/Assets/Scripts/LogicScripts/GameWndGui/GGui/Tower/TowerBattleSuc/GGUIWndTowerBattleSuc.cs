using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerBattleSuc : _ATALBasicUIWnd<GGUIMonoTowerBattleSuc>
    {
        private static GGUIWndTowerBattleSuc _g_instance = new GGUIWndTowerBattleSuc();

        public static GGUIWndTowerBattleSuc instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerBattleSuc();
                return _g_instance;
            }
        }

        public GGUIWndTowerBattleSuc() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerBattleSuc.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerBattleSuc.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        private TowerChallengeResult _m_result;
        private NPGGUIWndCommonItemContainer _m_itemContainer;
        private Action _m_onClose;

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_itemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            // 奖励列表
            _m_itemContainer?.discard();
            _m_itemContainer = null;
            _m_result = null;
            _m_onClose = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // 奖励列表
            if (null != wnd.itemContainer)
                _m_itemContainer = new NPGGUIWndCommonItemContainer(wnd.itemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        private void _onBtnCloseClick(GameObject _obj)
        {
            _m_onClose?.Invoke();
        }

        public void setInfo(TowerChallengeResult _result, Action _onClose)
        {
            _m_result = _result;
            _m_onClose = _onClose;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_result)
                return;
            TowerChapterRefObj newChapter = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(NPPlayer.instance.towerComp.curChapterId);
            int newLevel = NPPlayer.instance.towerComp.curLevel;

            long canActiveBonus = 0;
            if (newChapter != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtLevelName, newChapter.getLevelName(newLevel));
            
                int upLevel = _calculateTowerLevelNumBetween(_m_result.oldChapterId, _m_result.oldLevel, newChapter.id, newLevel, out canActiveBonus);
                ALUGUICommon.setLabelTxt(wnd.txtUpLevelNum, TextTranslate.instance.getLanguage(TransKeyConst.tower_suc_up_level_num_desc, upLevel));
                
                ALUGUICommon.setLabelTxt(wnd.txtResearchUpDesc, TextTranslate.instance.getLanguage(TransKeyConst.tower_suc_research_bonus_up_desc, canActiveBonus/100f));
                ALUGUICommon.setGameObjEnable(wnd.hasAddResearchShowGos, canActiveBonus > 0);

                TowerResearchRefObj nextResearchRefObj = getNextResearchRefObj(newChapter, newLevel);
                if (nextResearchRefObj != null)
                {
                    TowerChapterRefObj nextResearchChapterRef = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(nextResearchRefObj.chapter_id);
                    if (nextResearchChapterRef != null)
                        ALUGUICommon.setLabelTxt(wnd.txtNextResearchDesc, 
                            TextTranslate.instance.getLanguage(TransKeyConst.tower_suc_next_research_pos_desc,
                                nextResearchChapterRef.getLevelName(nextResearchRefObj.level)));
                }
                if (upLevel > 0)
                {
                    //计算总赚速
                    NPPlayer.instance.buildingComp.recalAllBusinessBuilding();
                }
            }
            TowerChapterRefObj oldChapter = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(_m_result.oldChapterId);
            if (oldChapter != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtOldLevelName, oldChapter.getLevelName(_m_result.oldLevel));
            }


            if (_m_result.hasItem)
            {
                _m_itemContainer?.showWnd();
                _m_itemContainer?.showItemList(_m_result.costItems);
            }
            else
            {
                _m_itemContainer?.hideWnd();
            }
            ALUGUICommon.setGameObjEnable(wnd.hasRewardShowGos, _m_result.hasItem);
        }
    
        // 计算两个关卡之间的关卡数
        private int _calculateTowerLevelNumBetween(long _old_chapter, int _old_level, long _new_chapter, int _new_level, out long _canActiveBonus)
        {
            _canActiveBonus = 0;
            if (_old_chapter > _new_chapter || (_old_chapter == _new_chapter && _old_level > _new_level))
                return -1; // _new 在 _old 之前
        
            TowerChapterRefObj curChapterRef = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(_old_chapter);

            if (curChapterRef == null)
                return -1;
            if (_old_chapter == _new_chapter)
            {
                // 遍历当前章节的研究列表，计算可以激活的加成
                if (curChapterRef.research_list != null)
                    foreach (var researchRef in curChapterRef.research_list)
                    {
                        if(researchRef != null && researchRef.level > _old_level && researchRef.level <= _new_level)
                            _canActiveBonus += researchRef.building_profit_add_per;
                    }

                // 如果在同一个Chapter下，则通过章节Level来计算
                return _new_level - _old_level;
            }
            long chapter = _old_chapter;

            int totalNum = curChapterRef.level_count - _old_level;
            // 不在同一个章节，遍历当前章节关卡之后的研究列表，加上可以激活的加成
            if (curChapterRef.research_list != null)
                foreach (var researchRef in curChapterRef.research_list)
                {
                    if(researchRef != null && researchRef.level > _old_level)
                        _canActiveBonus += researchRef.building_profit_add_per;
                }
            while (chapter != _new_chapter)
            {
                curChapterRef = GRefdataCoreMgr.instance.getTowerNextChapterRefObj(chapter);
                if (curChapterRef == null)
                {
                    Debug.LogError($"爬塔：Chapter 数据有错：{chapter}");
                    break;
                }
                chapter = curChapterRef.id;
                // 如果当前章节不是目标章节，则加上章节的关卡数
                if (curChapterRef.id != _new_chapter)
                {
                    totalNum += curChapterRef.level_count;
                    if (curChapterRef.research_list != null)
                        foreach (var researchRef in curChapterRef.research_list)
                        {
                            if(researchRef != null)
                                _canActiveBonus += researchRef.building_profit_add_per;
                        }
                }
                else
                {
                    // 如果当前章节的等于目标章节，则可以直接计算
                    break;
                }
            }

            if (curChapterRef == null)
                return -1;
            // 遍历当前章节的研究列表，计算可以激活的加成
            if (curChapterRef.research_list != null)
                foreach (var researchRef in curChapterRef.research_list)
                {
                    if(researchRef != null && researchRef.level <= _new_level)
                        _canActiveBonus += researchRef.building_profit_add_per;
                }
            return totalNum + _new_level ;
        }

        private TowerResearchRefObj getNextResearchRefObj(TowerChapterRefObj _chapterRef, int _level)
        {
            if (null == _chapterRef || null == _chapterRef.research_list)
                return null;
            foreach (var research in _chapterRef.research_list)
            {
                if (research != null && research.level > _level)
                    return research;
            }

            // 如果当前章节没有更多的研究，则获取下一个章节的第一个研究
            TowerChapterRefObj nextChapterRefObj = GRefdataCoreMgr.instance.getTowerNextChapterRefObj(_chapterRef.id);
            if (nextChapterRefObj != null && nextChapterRefObj.research_list != null)
            {
                foreach (var research in nextChapterRefObj.research_list)
                {
                    if (research != null)
                        return research;
                }
            }

            return null;
        }
    }
}