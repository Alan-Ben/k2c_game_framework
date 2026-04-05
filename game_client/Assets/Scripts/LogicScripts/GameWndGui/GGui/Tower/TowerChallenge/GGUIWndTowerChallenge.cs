using System.Collections.Generic;
using ALPackage;
using Common.TowerObj;
using GOE;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndTowerChallenge : _ATALBasicUIWnd<GGUIMonoTowerChallenge>
    {
        private static GGUIWndTowerChallenge _g_instance = new GGUIWndTowerChallenge();

        public static GGUIWndTowerChallenge instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndTowerChallenge();
                return _g_instance;
            }
        }

        
        public GGUIWndTowerChallenge() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoTowerChallenge.assetPath; }
        protected override string _monoObjName { get => GGUIMonoTowerChallenge.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        private NPGGUIWndPlayerIcon _m_playerInfo; //玩家信息
        private GGUIWndTowerChallengeItemGrid _m_levelGrid;
        private NPGGUIWndCommonToggleEx _m_toggleSkipBattle;

        private List<TowerLevelInfo> _m_itemDataList = new List<TowerLevelInfo>();

        private bool _m_bMoveToMiniLevel;

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_TOWER_CHALLENGE_INDEX, _onSimulateClickTowerChallengeByIndex);
            WinMsg.RegisterMsg(WinMsgType.ON_TOWER_CHG, _onTowerChange);
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_TOWER_CHALLENGE_INDEX, _onSimulateClickTowerChallengeByIndex);
            WinMsg.UnregisterMsg(WinMsgType.ON_TOWER_CHG, _onTowerChange);
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_playerInfo?.discard();
            _m_playerInfo = null;
            
            _m_levelGrid?.discard();
            _m_levelGrid = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnResearch, _onBtnResearchClick);
            }
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);

            if (null != wnd.towerChallengeItemGrid)
            {
                _m_levelGrid = new GGUIWndTowerChallengeItemGrid(wnd.towerChallengeItemGrid);
            }
            if(null != wnd.toggleSkipBattle)
            {
                _m_toggleSkipBattle = new NPGGUIWndCommonToggleEx(wnd.toggleSkipBattle);
                _m_toggleSkipBattle.clickDelegate += _onToggleClick;
                _m_toggleSkipBattle.setSelected(false);
            } 
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnResearch, _onBtnResearchClick);

        }

        public void setInfo(bool _moveToMiniLevel)
        {
            _m_bMoveToMiniLevel = _moveToMiniLevel;
        }
        
        /// <summary>
        /// 获取爬塔最大可战胜的关卡数量
        /// </summary>
        /// <param name="_curPower"></param>
        /// <param name="_curTowerLevel"></param>
        /// <param name="_showCount"></param>
        /// <returns></returns>
        private int getTowerChallengeMaxShow(long _curPower, long _curLevel_chapter, int _curLevel_level, int _showCount)
        {
            int maxShowLevel = _showCount;
            GRefdataCoreMgr.instance.towerChapterStageShowRefCore.dealAllRef(_obj =>
            {
                if (_obj != null)
                {
                    TowerLevelInfo towerLevelInfo = getNextLevelInfo(_curLevel_chapter, _curLevel_level, _obj.show_lvl);
                    if (towerLevelInfo != null && towerLevelInfo.power < _curPower && _obj.show_lvl > maxShowLevel)
                        maxShowLevel = _obj.show_lvl;
                }
            });
            return maxShowLevel;
        }

        /// <summary>
        /// 获取后面第_step关
        /// </summary>
        /// <param name="_curLevel"></param>
        /// <param name="_step"></param>
        /// <returns></returns>
        private TowerLevelInfo getNextLevelInfo(long _curLevel_chapter, int _curLevel_level, int _step)
        {
            TowerChapterRefObj curChapterRef = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_curLevel_chapter);

            if (curChapterRef == null)
            {
                Debug.LogError($"爬塔：Chapter数据有错：{_curLevel_chapter},");
                return null;
            }
            // 如果目标关卡在当前章节内，则可以直接给level
            if (_step <= curChapterRef.level_count - _curLevel_level)
                return new TowerLevelInfo(_curLevel_chapter, _curLevel_level + _step);

            // 如果目标关卡不在当前章节内，则需要便利后续章节找到所在的章节并算出stage和level
            long chapter = _curLevel_chapter;

            int curStep = 0;
            curStep += curChapterRef.level_count - _curLevel_level;
            
            while (curStep < _step)
            {
                curChapterRef = GRefdataCoreMgr.instance.getTowerNextChapterRefObj(chapter);
                if (curChapterRef == null)
                {
                    Debug.LogError($"爬塔：Chapter 数据有错：{chapter}");
                    break;
                }
                chapter = curChapterRef.id;
                // 如果当前章节的关卡数量小于目标关卡数，则继续下一章
                if (curChapterRef.level_count + curStep < _step)
                {
                    curStep += curChapterRef.level_count;
                }
                else
                {
                    // 如果当前章节的关卡数量大于目标关卡数，则找到目标关卡所在的stage和level
                    break;
                }
            }

            if (curChapterRef == null)
            {
                Debug.LogError($"爬塔：Chapter数据有错：");
                return null;
            }
            
            return new TowerLevelInfo(curChapterRef.id, _step - curStep);
        }
        
     
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            
            if (null != _m_playerInfo)
            {
                _m_playerInfo.showWnd();
                _m_playerInfo.setSelfInfo();
            }
            TowerChapterRefObj newChapter = GRefdataCoreMgr.instance.towerChapterRefCore?.getRef(NPPlayer.instance.towerComp.curChapterId);
            int newLevel = NPPlayer.instance.towerComp.curLevel;
            if (newChapter != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtEarnBonus, TextTranslate.instance.getLanguage(TransKeyConst.tower_challengeEarningAddPer_num, NPPlayer.instance.towerComp.towerEarningAddPer/100f));
                ALUGUICommon.setLabelTxt(wnd.txtLevelName, newChapter.getLevelName( newLevel));
                ALUGUICommon.setLabelTxt(wnd.txtDailyCoins, TextTranslate.instance.getLanguage(TransKeyConst.tower_daily_coin_get_num, newChapter.getTowerCoinCount(newLevel)));
            }

            _m_toggleSkipBattle?.setSelected(AccountSettingMgr.instance.accountSetting.towerSkipBattleShow);

            bool canSkip = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_skip_battle_simple_unlock_id);
            ALUGUICommon.setGameObjEnable(wnd.goSkipShowList, canSkip);
            ALUGUICommon.setGameObjEnable(wnd.goSkipHideList, !canSkip);

           
            _refreshGridData();
        }
        private void _refreshGridData()
        {
            
            if(null == wnd)
                return;
            // 离线版爬塔挑战列表获取方法
            long curPower = NPPlayer.instance.heroComponent.totalPower;
            TowerChapterRefObj curChapterRef = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(NPPlayer.instance.towerComp.curChapterId);
            
            TowerChapterRefObj nextChapterRef = GRefdataCoreMgr.instance.getTowerNextChapterRefObj(NPPlayer.instance.towerComp.curChapterId);
            
            int maxShow = 0;
            foreach (var stageShowRef in GRefdataCoreMgr.instance.towerChapterStageShowRefCore.refList)
            {
                if(stageShowRef != null && stageShowRef.show_lvl > maxShow)
                    maxShow = stageShowRef.show_lvl;
            }
            bool needReqChallengeList = false;
            if (curChapterRef == null || curChapterRef.if_pve_chapter)
            {
                if (curChapterRef != null && nextChapterRef != null && !nextChapterRef.if_pve_chapter)
                {
                    // 如果当前章节是pve章节，并且下个章节是pvp章节，如果当前章节的关卡数加上最大展示关卡数大于当前章节的总关卡数，则需要向服务器请求挑战列表
                    if(NPPlayer.instance.towerComp.curLevel + maxShow > curChapterRef.level_count)
                        needReqChallengeList = true;
                }
            }
            else // 如果当前章节是pvp章节
                needReqChallengeList = true;
            
            // 是否要向服务端请求挑战列表
            if (needReqChallengeList)
            {
                NPPlayer.instance.towerComp.reqTowerChallengeList(_list =>
                {
                    if (_list != null)
                    {
                        _m_itemDataList.Clear();
                        foreach (Tower_OpponentInfo item in _list)
                        {
                            if (item == null) continue;
                            Tower_PosInfo posInfo = item.getPosInfo();
                            TowerLevelInfo levelInfo = new TowerLevelInfo(posInfo.getChapterId(), posInfo.getLevel());
                            levelInfo.updatePlayerInfo(item.getPlayerId());
                            _m_itemDataList.Add(levelInfo);
                        }
                        _m_itemDataList.Sort(TowerLevelInfo.Sort);
                        _refreshGrid();
                    }

                });
            }
            else
            {
                int maxShowLevel = getTowerChallengeMaxShow(curPower, NPPlayer.instance.towerComp.curChapterId, NPPlayer.instance.towerComp.curLevel, wnd.showCount);
                if (maxShowLevel % wnd.showCount != 0)
                {
                    Debug.LogError($"爬塔关卡显示数量配置错误，当前显示数量为{maxShowLevel}，请配置为{maxShowLevel / wnd.showCount}的整数倍");
                    return;
                }

                int step = maxShowLevel / wnd.showCount;
                _m_itemDataList.Clear();
                for (int i = wnd.showCount; i > 0; i--)
                {
                    TowerLevelInfo towerLevelInfo = getNextLevelInfo(NPPlayer.instance.towerComp.curChapterId, NPPlayer.instance.towerComp.curLevel, step * i );
                    if(null != towerLevelInfo)
                        _m_itemDataList.Add(towerLevelInfo);
                }
                _m_itemDataList.Sort(TowerLevelInfo.Sort);
                _refreshGrid();
            }
           
        }

        private void _refreshGrid()
        {
            _m_levelGrid?.showWnd();
            _m_levelGrid?.showItemList(_m_itemDataList); 
            
            if (_m_bMoveToMiniLevel)
            {
                _m_levelGrid?.moveToTopNextFrame();
                _m_bMoveToMiniLevel = false;
            }
            else
            {
                _m_levelGrid?.moveToBottomNextFrame();
            }
        }
        
        
        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TOWER_CHALLENGE);
        }
        
        private void _onToggleClick(NPGGUIWndCommonToggleEx obj)
        {
            if (null == _m_toggleSkipBattle || null == obj)
                return;
            if(!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_skip_battle_simple_unlock_id, true))
                return;
            _m_toggleSkipBattle.setSelected(!obj.isOn);
            AccountSettingMgr.instance.accountSetting.setTowerSkipBattleShow(_m_toggleSkipBattle.isOn);
        }

        private void _onBtnResearchClick(GameObject _obj)
        {
            QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTowerResearch.instance, UINodeTagConst.C_TOWER_RESEARCH);
        }
        
        private void _onTowerChange(params object[] _objects)
        {
            _refreshWnd();
        }

        //模拟点击挑战当前列表第几个boss的效果(序列号0为当前界面最底下一层)
        private void _onSimulateClickTowerChallengeByIndex(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long index = (long)_objects[0];
            _m_levelGrid?.setClickChallengeByIndex((int)index);
        }
    }
}