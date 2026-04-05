using System;
using ALPackage;
using Common.ArenaObj;
using System.Collections.Generic;
using UnityEngine;
using Common.HeroObj;
using GS2GC.p023_ArenaOp;
using Common.ArenaEnum;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗主界面
    /// </summary>
    public class GGUIWndArenaBattle : _ANPGGUIBasicResBarWnd<GGUIMonoArenaBattle>
    {
        private static GGUIWndArenaBattle _g_instance;
        public static GGUIWndArenaBattle instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaBattle();
                return _g_instance;
            }
        }

        //竞技场战斗信息
        private ArenaBattleInfo _m_arenaBattleInfo;
        //自己伙伴信息附加窗口
        private GGUIWndArenaBattleSubSelfHeroInfo _m_wSelfHeroInfo;
        //战场上自己伙伴item附加窗口
        private GGUIWndArenaBattleSubHeroInfoItem _m_wSelfHeroItem;
        //战场上对手伙伴item附加窗口列表
        private List<GGUIWndArenaBattleSubHeroInfoItem> _m_lOpponentHeroInfoList;
        //对手信息附加窗口
        private GGUIWndArenaBattleSubOpponentInfo _m_wOpponentInfo;
        //对手名称
        private string _m_sOpponentName;
        //战斗表现步骤管理对象
        private ALProcess _m_pProcessObj;

        public GGUIWndArenaBattle() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattle.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattle.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_ARENA_SELECT_OPPONENT_HERO_BY_INDEX, _simulateSelectOpponentHero);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ARENA_BATTLE_INFO_CHG, _onBattleInfoChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_ARENA_ONE_KEY_BATTLE_SETTING, _onSimulateClickOpenOneKeySetting);
            //设置信息
            _m_arenaBattleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;
            //刷新窗口
            _refreshWnd();
            //检查是否购买buff
            _checkBuyBuff();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_ARENA_SELECT_OPPONENT_HERO_BY_INDEX, _simulateSelectOpponentHero);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ARENA_BATTLE_INFO_CHG, _onBattleInfoChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_ARENA_ONE_KEY_BATTLE_SETTING, _onSimulateClickOpenOneKeySetting);
            _m_wSelfHeroInfo?.hideWnd();
            _m_wOpponentInfo?.hideWnd();
            _m_wSelfHeroItem?.hideWnd();
            _m_sOpponentName = null;

            if (_m_lOpponentHeroInfoList != null)
            {
                foreach (var _opponentHeroInfo in _m_lOpponentHeroInfoList)
                {
                    _opponentHeroInfo?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_wSelfHeroInfo?.resetWnd();
            _m_wOpponentInfo?.resetWnd();
            _m_wSelfHeroItem?.resetWnd();

            if (_m_lOpponentHeroInfoList != null)
            {
                foreach (var _opponentHeroInfo in _m_lOpponentHeroInfoList)
                {
                    _opponentHeroInfo?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_wSelfHeroInfo?.discard();
            _m_wSelfHeroInfo = null;
            _m_wOpponentInfo?.discard();
            _m_wOpponentInfo = null;
            _m_wSelfHeroItem?.discard();
            _m_wSelfHeroItem = null;

            if (_m_lOpponentHeroInfoList != null)
            {
                foreach (var _opponentHeroInfo in _m_lOpponentHeroInfoList)
                {
                    _opponentHeroInfo?.discard();
                }
                _m_lOpponentHeroInfoList.Clear();
            }
            _m_lOpponentHeroInfoList = null;

            if (_m_pProcessObj != null)
                _m_pProcessObj.discard();
            _m_pProcessObj = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onClickRank);//点击排行榜
            ALUGUICommon.uncombineBtnClick(wnd.btnConvenientSetting, _onClickConvenientSetting);//点击便捷设置
            ALUGUICommon.uncombineBtnClick(wnd.btnSelectBuff, _onClickSelectBuff);//点击选择增益
            ALUGUICommon.uncombineBtnClick(wnd.btnOneKeySetting, _onClickOneKeySetting);//点击一键谈判
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoSubOpponentInfo != null)
                _m_wOpponentInfo = new GGUIWndArenaBattleSubOpponentInfo(wnd.monoSubOpponentInfo);

            if (wnd.monoSubSelfHeroInfo != null)
                _m_wSelfHeroInfo = new GGUIWndArenaBattleSubSelfHeroInfo(wnd.monoSubSelfHeroInfo);

            if (wnd.monoSelfHeroItem != null)
                _m_wSelfHeroItem = new GGUIWndArenaBattleSubHeroInfoItem(wnd.monoSelfHeroItem);

            if (wnd.monoSubOpponentHeroItemList != null)
            {
                _m_lOpponentHeroInfoList = new List<GGUIWndArenaBattleSubHeroInfoItem>();
                for (int i = 0; i < wnd.monoSubOpponentHeroItemList.Count; i++)
                {
                    if(wnd.monoSubOpponentHeroItemList[i] == null)
                        continue;

                    GGUIWndArenaBattleSubHeroInfoItem opponentHeroInfoWnd = new GGUIWndArenaBattleSubHeroInfoItem(wnd.monoSubOpponentHeroItemList[i].monoHeroItem, wnd.monoSubOpponentHeroItemList[i].aniSelect);
                    opponentHeroInfoWnd.onClickFight += _onClickSelectOpponentHeroFight;
                    _m_lOpponentHeroInfoList.Add(opponentHeroInfoWnd);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭按钮
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onClickRank);//点击排行榜
            ALUGUICommon.combineBtnClick(wnd.btnConvenientSetting, _onClickConvenientSetting);//点击便捷设置
            ALUGUICommon.combineBtnClick(wnd.btnSelectBuff, _onClickSelectBuff);//点击选择增益
            ALUGUICommon.combineBtnClick(wnd.btnOneKeySetting, _onClickOneKeySetting);//点击一键谈判
        }

        #region 刷新窗口

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshSelfHeroInfo();
            _refreshOpponentHeroInfo();
            _refreshOpponentInfo();
        }

        //刷新自己伙伴信息
        private void _refreshSelfHeroInfo()
        {
            if (wnd == null || _m_arenaBattleInfo == null)
                return;

            //设置连胜数量
            if (_m_arenaBattleInfo.hadDefeatNum == 0)
                ALUGUICommon.setGameObjEnable(wnd.goNoWinCountHideList, false);
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goNoWinCountHideList, true);
                ALUGUICommon.setLabelTxt(wnd.txtWinCount, TextTranslate.instance.getLanguage(TransKeyConst.common_multiple_num, _m_arenaBattleInfo.hadDefeatNum));
            }

            //自己伙伴详细信息
            _m_wSelfHeroInfo?.showWnd();
            _m_wSelfHeroInfo?.setInfo();

            //自己伙伴item信息
            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_arenaBattleInfo.heroId);
            if (heroInfo == null)
                return;

            _m_wSelfHeroItem?.showWnd();
            _m_wSelfHeroItem?.setInfo(new Hero_ArenaShowInfo(heroInfo.id, heroInfo.curSkinId, (int)heroInfo.level, 0));

            //一键战斗按钮显示隐藏
            bool isOneKeyUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.arena_one_key_attack_simple_unlock_id, false);
            ALUGUICommon.setGameObjEnable(wnd.goOneKeyLockShowList, !isOneKeyUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goOneKeyLockHideList, isOneKeyUnlock);

        }

        //刷新对手伙伴信息
        private void _refreshOpponentHeroInfo()
        {
            if (_m_arenaBattleInfo == null || _m_arenaBattleInfo.canAttackHeroList == null || _m_lOpponentHeroInfoList == null)
                return;

            for (int i = 0; i < _m_lOpponentHeroInfoList.Count; i++)
            {
                if(_m_lOpponentHeroInfoList[i] == null)
                    continue;

                //重置一下动画
                _m_lOpponentHeroInfoList[i].resetSelectAni();

                //有数据则显示，无数据则隐藏
                if (_m_arenaBattleInfo.canAttackHeroList.Count > i)
                {
                    _m_lOpponentHeroInfoList[i].showWnd();
                    _m_lOpponentHeroInfoList[i].setInfo(_m_arenaBattleInfo.canAttackHeroList[i]);
                }
                else
                    _m_lOpponentHeroInfoList[i].hideWnd();
            }
        }

        //刷新对手信息
        private void _refreshOpponentInfo()
        {
            ArenaBattleInfo arenaBattleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;
            if (arenaBattleInfo == null)
                return;

            _m_wOpponentInfo?.showWnd();
            _m_wOpponentInfo?.setCid(arenaBattleInfo.opponentCid, (_info)=>
            {
                _recordOpponentName(_info);
                _m_wOpponentInfo?.setPower(arenaBattleInfo.opponentPower);
            });
            _m_wOpponentInfo?.setHeroCount(arenaBattleInfo.opponentLeftHeroNum, arenaBattleInfo.opponentHeroNum);
        }

        //记录对手名称
        private void _recordOpponentName(NPCommonSimplePlayerInfo _info)
        {
            if (_info == null || wnd == null || !isShow)
                return;

            _m_sOpponentName = _info.name;
        }

        #endregion

        #region 处理战斗流程

        //处理普通战斗流程
        private void _dealBattleProcess(GS2GC_023_005_RetRoundAttack _result, Hero_ArenaShowInfo _selectHeroShowInfo)
        {
            if (_result == null)
                return;

            _m_pProcessObj = ALProcess.CreateProcess("arena_battle");
            _m_pProcessObj
                .addDelegateProcess(_onDone => { _process_showFight(_selectHeroShowInfo, _result.getRoundResult().getIsDefeat(), _onDone); })//展示战斗动画界面
                .addProcess(_refreshWnd)//刷新界面
                .addDelegateProcess(_onDone => { _process_showRoundResult(_result.getRoundResult(), _onDone); })//展示回合结果
                .addDelegateProcess(_onDone => { _process_showRoundReward(_result.getRoundResult(), _result.getRoundReward(), _onDone); })//展示回合特殊奖励
                .addDelegateProcess(_onDone => { _process_showFinalResult(_result.getBattleResult(), _onDone); })//展示最终结果
                .addDelegateProcess(_onDone => { _process_showHeroAddPower(_result.getBattleResult(), _onDone); })//展示伙伴增加实力
                .addProcess(_checkBuyBuff)//检查新一回合是否需要买buff
                .deal();
        }

        //处理一键战斗流程
        private void _dealOneKeyBattleProcess(GS2GC_023_013_RetArenaAKeyAttack _result)
        {
            if (_result == null)
                return;

            _m_pProcessObj = ALProcess.CreateProcess("arena_one_key_battle");
            _m_pProcessObj
                .addProcess(_refreshWnd)//刷新界面
                .addDelegateProcess(_onDone => { _process_showFinalResult(_result.getBattleResult(), _onDone); })//展示最终结果
                .addDelegateProcess(_onDone => { _process_showHeroAddPower(_result.getBattleResult(), _onDone); })//展示伙伴增加实力
                .deal();
        }

        //显示战斗过程
        private void _process_showFight(Hero_ArenaShowInfo _selectHeroShowInfo, bool _isWin, Action _onDone)
        {
            //如果设置跳过单场战斗动画，则直接完成
            if (AccountSettingMgr.instance.accountSetting.arenaSetSkipBattle)
            {
                _onDone?.Invoke();
                return;
            }

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleShow.instance, () =>
            {
                GGUIWndArenaBattleShow.instance.showWnd();
                GGUIWndArenaBattleShow.instance.setInfo(_m_arenaBattleInfo, _selectHeroShowInfo, _isWin, _onDone);
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_ARENA_BATTLE_SHOW, false, false);
        }

        //展示回合结果
        private void _process_showRoundResult(Arena_RoundResult _roundResult, Action _onDone)
        {
            if (_roundResult == null)
            {
                _onDone?.Invoke();
                return;
            }

            //是否击败对手伙伴
            if(_roundResult.getIsDefeat())
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleRoundWin.instance, () =>
                {
                    GGUIWndArenaBattleRoundWin.instance.showWnd();
                    GGUIWndArenaBattleRoundWin.instance.setInfo(_roundResult, _onDone);
                }, UINodeTagConst.C_ARENA_BATTLE_ROUND_WIN);
            }
            else
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleRoundLost.instance, () =>
                {
                    GGUIWndArenaBattleRoundLost.instance.showWnd();
                    GGUIWndArenaBattleRoundLost.instance.setInfo(_m_arenaBattleInfo != null ? _m_arenaBattleInfo.heroId : 0, _onDone);
                }, UINodeTagConst.C_ARENA_BATTLE_ROUND_LOST);
            }
        }

        //展示回合特殊奖励
        private void _process_showRoundReward(Arena_RoundResult _roundResult, Arena_RoundReward _roundReward, Action _onDone)
        {
            if (_roundResult == null || _roundReward == null || _roundReward.getGainItem() == null || _roundReward.getGainItem().Count == 0)
            {
                _onDone?.Invoke();
                return;
            }

            ArenaRoundRewardRefObj arenaRoundRef = GRefdataCoreMgr.instance.arenaRoundRewardRefCore.getRef(_roundResult.getRound());

            //没有连胜奖励，不需要展示
            if (arenaRoundRef == null)
            {
                _onDone?.Invoke();
                return;
            }

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleRoundReward.instance, () =>
            {
                GGUIWndArenaBattleRoundReward.instance.showWnd();
                GGUIWndArenaBattleRoundReward.instance.setInfo(_roundReward, _onDone);
            }, UINodeTagConst.C_ARENA_BATTLE_ROUND_REWARD);
        }

        //展示最终结果
        private void _process_showFinalResult(Arena_BattleResult _finalResult, Action _onDone)
        {
            if (_finalResult == null || !_finalResult.getIsSettle())
            {
                _onDone?.Invoke();
                return;
            }

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleFinalResult.instance, () =>
            {
                GGUIWndArenaBattleFinalResult.instance.showWnd();
                GGUIWndArenaBattleFinalResult.instance.setInfo(_m_arenaBattleInfo, _finalResult, _m_sOpponentName, ()=>
                {
                    _onDone?.Invoke();
                });
            }, UINodeTagConst.C_ARENA_BATTLE_FINAL_RESULT);
        }

        //展示伙伴增加实力
        private void _process_showHeroAddPower(Arena_BattleResult _finalResult, Action _onDone)
        {
            if (_finalResult == null || _finalResult.getAddPower() <= 0)
            {
                _onDone?.Invoke();
                if(_finalResult.getIsSettle())
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE);
                return;
            }

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleFinishHeroAddPower.instance, () =>
            {
                GGUIWndArenaBattleFinishHeroAddPower.instance.showWnd();
                GGUIWndArenaBattleFinishHeroAddPower.instance.setInfo(_m_arenaBattleInfo, _finalResult, ()=>
                {
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE);
                    _onDone?.Invoke();
                });
            }, UINodeTagConst.C_ARENA_BATTLE_FINAL_HERO_ADD_POWER);
        }

        //检查是否需要买buff
        private void _checkBuyBuff()
        {
            if (_m_arenaBattleInfo == null || 
                _m_arenaBattleInfo.hadBuyBuff || 
                _m_arenaBattleInfo.canAttackHeroList == null || 
                _m_arenaBattleInfo.canAttackHeroList.Count <= 0 ||
                _m_arenaBattleInfo.getCurLeftPower() <= 0 ||
                _m_arenaBattleInfo.getIsFirstBattleRound())//第一轮已在外面购买了初始增益，这里不再处理
                return;

            List<long> buffIdList = null;
            buffIdList = GRefdataCoreMgr.instance.npGeneral.arena_choose_buff_list;
            Dictionary<EArenaBuffType, ArenaBuffRefObj> buffTypeDic = new Dictionary<EArenaBuffType, ArenaBuffRefObj>();
            foreach (long buffId in buffIdList)
            {
                ArenaBuffRefObj buffRef = GRefdataCoreMgr.instance.arenaBuffRefCore.getRef(buffId);
                if (buffRef == null)
                    continue;
                buffTypeDic[buffRef.type] = buffRef;
            }

            ArenaBuffRefObj targetItem = null;
            //是否自动购买水晶临时增益，不足时购买2硬币加成
            if (AccountSettingMgr.instance.accountSetting.arenaSetAutoBuyBuffByCrystal)
            {
                targetItem = buffTypeDic[EArenaBuffType.CRYSTAL];
                if (!_reqAutoBuyBuff(targetItem))
                {
                    targetItem = buffTypeDic[EArenaBuffType.TWO_COIN];
                    _reqAutoBuyBuff(targetItem);
                }
            }//购买2硬币增益，不足时购买1硬币增益
            else if (AccountSettingMgr.instance.accountSetting.arenaSetAutoBuyBuffByTwoCoin)
            {
                targetItem = buffTypeDic[EArenaBuffType.TWO_COIN];
                if (!_reqAutoBuyBuff(targetItem))
                {
                    targetItem = buffTypeDic[EArenaBuffType.ONE_COIN];
                    _reqAutoBuyBuff(targetItem);
                }
            }//购买1硬币增益
            else if (AccountSettingMgr.instance.accountSetting.arenaSetAutoBuyBuffByOneCoin)
            {
                targetItem = buffTypeDic[EArenaBuffType.ONE_COIN];
                _reqAutoBuyBuff(targetItem);
            }//不是勾选不购买增益，弹出增益选择界面
            else if (!AccountSettingMgr.instance.accountSetting.arenaSetNotToBuyBuff)
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleSelectBuff.instance, GGUIWndArenaBattleSelectBuff.instance.showWnd, UINodeTagConst.C_ARENA_BATTLE_SELECT_BUFF);
        }

        //请求自动购买buff
        private bool _reqAutoBuyBuff(ArenaBuffRefObj _buffRef)
        {
            if (_buffRef != null && GCommon.isItemEnough(_buffRef.cost, false))
            {
                NPPlayer.instance.arenaComp.reqChooseBuff(_buffRef.id);
                return true;
            }
            return false;
        }

        #endregion

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            //如果在战斗中，强制关闭主界面
            if (NPPlayer.instance.arenaComp.isInBattle())
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_MAIN);

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE);
        }


        //点击排行榜
        private void _onClickRank(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaRank.instance, GGUIWndArenaRank.instance.showWnd, UINodeTagConst.C_ARENA_RANK);
        }

        //点击设置
        private void _onClickConvenientSetting(GameObject _go)
        {
            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.arena_convenient_setting_simple_unlock_id, true))
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaConvenientSetting.instance, GGUIWndArenaConvenientSetting.instance.showWnd, UINodeTagConst.C_ARENA_CONVENTENT_SETTING);
        }

        //点击选择增益
        private void _onClickSelectBuff(GameObject _go)
        {
            if (_m_arenaBattleInfo == null)
                return;

            if (_m_arenaBattleInfo.hadBuyBuff)
            {
                //本回合已购买临时增益，去跟对手谈判吧
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_battleHadBuyBuffTip_none);
                return;
            }

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleSelectBuff.instance, GGUIWndArenaBattleSelectBuff.instance.showWnd, UINodeTagConst.C_ARENA_BATTLE_SELECT_BUFF);
        }

        //点击一键谈判
        private void _onClickOneKeySetting(GameObject _go)
        {
            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.arena_one_key_attack_simple_unlock_id, true))
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaBattleOneKey.instance, () =>
            {
                GGUIWndArenaBattleOneKey.instance.showWnd();
                GGUIWndArenaBattleOneKey.instance.setInfo(_dealOneKeyBattleProcess);
            }, UINodeTagConst.C_ARENA_BATTLE_ONE_KEY);
        }

        //点击选择对手伙伴谈判
        private void _onClickSelectOpponentHeroFight(GGUIWndArenaBattleSubHeroInfoItem _wndOpponentHero)
        {
            if (_m_arenaBattleInfo == null || _wndOpponentHero == null || _wndOpponentHero.arenaHeroInfo == null)
                return;

            //处理谈判请求
            Hero_ArenaShowInfo selectHeroShowInfo = _wndOpponentHero.arenaHeroInfo;
            //先屏蔽输入
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            //请求谈判
            NPPlayer.instance.arenaComp.reqRoundAttack(_wndOpponentHero.arenaHeroInfo.getHeroId(), (_isSuc, _msg) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);

                //发生错误不处理
                if (!_isSuc)
                    return;

                _m_arenaBattleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;
                //开始展示战斗结算界面
                if (_wndOpponentHero.selectAni != null)
                {
                    //先播放选中动画
                    int aniMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                    _wndOpponentHero.selectAni.forcePlay(() =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(aniMaskSerialize);
                        _dealBattleProcess(_msg, selectHeroShowInfo);
                    });
                }
                else
                {
                    _dealBattleProcess(_msg, selectHeroShowInfo);
                }
            });
        }

        #endregion

        #region 消息事件

        //模拟选择对手伙伴
        private void _simulateSelectOpponentHero(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long index = (long)_objects[0];
            if (_m_lOpponentHeroInfoList == null || index < 0 || index >= _m_lOpponentHeroInfoList.Count)
                return;

            _onClickSelectOpponentHeroFight(_m_lOpponentHeroInfoList[(int)index]);
        }

        //竞技场战斗信息变化
        private void _onBattleInfoChg()
        {
            _refreshSelfHeroInfo();
        }

        //模拟点击打开竞技场一键战斗设置窗口
        private void _onSimulateClickOpenOneKeySetting()
        {
            _onClickOneKeySetting(null);
        }

        #endregion
    }
}