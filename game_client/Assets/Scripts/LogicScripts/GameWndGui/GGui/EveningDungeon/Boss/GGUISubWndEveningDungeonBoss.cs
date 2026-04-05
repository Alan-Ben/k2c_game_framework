using System;
using System.Collections.Generic;
using ALPackage;
using Common.DungeonObj;
using GOE.EveningDungeon;

namespace GOE
{
    /// <summary>
    /// 晚间活动boss子窗口
    /// </summary>
    public class GGUISubWndEveningDungeonBoss : _ANPGGUIBasicSubWnd<GGUISubMonoEveningDungeonBoss>
    {
        private EveningDungeonGameController _m_gameController;// 晚间活动游戏控制器

        private List<GGUIWndEveningDungeonBossActor> _m_lBossActorList = new List<GGUIWndEveningDungeonBossActor>();// boss形象列表
        private GGUIWndEveningDungeonBossActor _m_curShowBossActor = null;// 当前显示的boss形象
        private NPGGUIWndProgress _m_wHp;//血条
        private GGUIWndEveningDungeonLossBloodTip _m_wLossBloodTip;//掉血tip显示窗口
        private NPGGUIWndPlayerIcon _m_wFinalAttackPlayerIcon;//最后一击玩家头像

        private NPCenterTipsRefObj _m_rDefaultLossHpCenterTipsRefObj;//掉血tip配表数据
        
        private long _m_lShowSerial;// 显示序列号

        public GGUISubWndEveningDungeonBoss(GGUISubMonoEveningDungeonBoss _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.bossActorGoIndexList != null)
            {
                if (_m_lBossActorList == null)
                    _m_lBossActorList = new List<GGUIWndEveningDungeonBossActor>();

                NPGGoIndex bossActorGoIndex = null;
                GGUIWndEveningDungeonBossActor bossActorWnd = null;
                for (int i = 0, count = wnd.bossActorGoIndexList.Count; i < count; i++)
                {
                    bossActorGoIndex = wnd.bossActorGoIndexList[i];
                    if (bossActorGoIndex == null || !bossActorGoIndex.isValid())
                    {
                        Debug.LogError_EditorOnly($"GGUISubMonoEveningDungeonBoss bossActorGoIndexList[{i}] 配置错误, GoIndex:{bossActorGoIndex} 无效", wnd);
                        continue;
                    }
                    
                    bossActorWnd = new GGUIWndEveningDungeonBossActor(bossActorGoIndex.assetPath, bossActorGoIndex.objName, wnd.actorParent);
                    _m_lBossActorList.Add(bossActorWnd);
                }
            }
            
            if (wnd.finalAttackPlayerIcon != null)
                _m_wFinalAttackPlayerIcon = new NPGGUIWndPlayerIcon(wnd.finalAttackPlayerIcon);

            if (wnd.monoHpProgress != null)
                _m_wHp = new NPGGUIWndProgress(wnd.monoHpProgress);
            
            if(wnd.monoLossBloodTip != null)
                _m_wLossBloodTip = new GGUIWndEveningDungeonLossBloodTip(wnd.monoLossBloodTip);
        }
        
        protected override void _onDiscard()
        {
            _m_curShowBossActor = null;
            if (_m_lBossActorList != null)
            {
                foreach (var actor in _m_lBossActorList)
                {
                    actor?.discard();
                }
                _m_lBossActorList.Clear();
                _m_lBossActorList = null;
            }
            
            _m_wFinalAttackPlayerIcon?.discard();
            _m_wFinalAttackPlayerIcon = null;
            
            _m_wHp?.discard();
            _m_wHp = null;
            
            _m_wLossBloodTip?.discard();
            _m_wLossBloodTip = null;
            
            _m_rDefaultLossHpCenterTipsRefObj = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerial = ALSerializeOpMgr.next();
            
            _m_wHp?.showWnd();
            _m_wLossBloodTip?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerial = ALSerializeOpMgr.next();

            if (_m_lBossActorList != null)
            {
                foreach (var actor in _m_lBossActorList)
                {
                    actor?.hideWnd();
                }
            }
            
            _m_wHp?.hideWnd();
            _m_wLossBloodTip?.hideWnd();
            _m_wFinalAttackPlayerIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_lBossActorList != null)
            {
                foreach (var actor in _m_lBossActorList)
                {
                    actor?.resetWnd();
                }
            }
            
            _m_wHp?.resetWnd();
            _m_wLossBloodTip?.resetWnd();
            _m_wFinalAttackPlayerIcon?.resetWnd();
        }
        
        /// <summary>
        /// 设置游戏控制器
        /// </summary>
        /// <param name="_gameController"></param>
        public void setGameController(EveningDungeonGameController _gameController)
        {
            _m_gameController = _gameController;
        }

        /// <summary>
        /// 设置boss复活次数
        /// </summary>
        /// <param name="_reviveCount"></param>
        public void setBossReviveCount(int _reviveCount)
        {
            if (wnd != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtRevivedCount, TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_bossRevivedCount_num, _reviveCount));
                ALUGUICommon.setGameObjEnable(wnd.hasRevivedShow, _reviveCount > 0);
            }

            // 更新boss形象
            updateBossActor(_reviveCount);
        }

        /// <summary>
        /// 设置最后一击玩家
        /// </summary>
        /// <param name="_cid"></param>
        public void setFinalAttackPlayer(long _cid)
        {
            if (_m_wFinalAttackPlayerIcon != null)
            {
                if (_cid > 0)
                {
                    _m_wFinalAttackPlayerIcon.showWnd();
                    _m_wFinalAttackPlayerIcon.setPlayer(_cid);
                }
                else
                {
                    _m_wFinalAttackPlayerIcon.hideWnd();
                }
            }
        }

        /// <summary>
        /// 设置boss血量
        /// </summary>
        /// <param name="_deductedHp">已扣除的血量</param>
        /// <param name="_totalHp">总血量</param>
        public void setBossHp(long _deductedHp, long _totalHp)
        {
            _m_wHp?.setProgress(_totalHp - _deductedHp, _totalHp, EValueFormatType.NORMAL);
        }

        /// <summary>
        /// 刷新复活倒计时
        /// </summary>
        public void setReviveCountDown(long _countDown)
        {
            if(wnd == null)
                return;

            string countDownStr = TextTranslate.instance.getLanguage(TransKeyConst.time_lessOneMin_num,
                _countDown <= 0 ? 0 : (_countDown / 1000f).ToCeilingLongValue());
            
            ALUGUICommon.setLabelTxt(wnd.txtReviveCountDown, countDownStr);

            if(wnd.tmpReviveCountDown != null)
                wnd.tmpReviveCountDown.text = countDownStr;
        }
        
        /// <summary>
        /// 设置boss状态
        /// </summary>
        public void setBossStateShow(EEveningDungeonBossState _bossState)
        {
            if(wnd == null)
                return;
            
            if(wnd.multiStateShow != null)
                wnd.multiStateShow.setShowData(_bossState);

            if(_m_curShowBossActor != null)
                _m_curShowBossActor.setBossState(_bossState);
        }

        /// <summary>
        /// boss被攻击表现
        /// </summary>
        public void showBossByAttack(Action _showDone)
        {
            if(wnd == null)
                return;
            
            long serial = _m_lShowSerial;
            setBossStateShow(EEveningDungeonBossState.BY_ATTACK);
            ALCommonTaskController.CommonActionAddMonoTask(()=>
            {
                if(serial != _m_lShowSerial)
                    return;
                
                _showDone?.Invoke();
            }, wnd.bossByAttackShowTimes);
        }

        /// <summary>
        /// boss死亡表现
        /// </summary>
        public void showBossDead(Action _showDone)
        {
            if(wnd == null)
                return;
            
            long serial = _m_lShowSerial;
            setBossStateShow(EEveningDungeonBossState.DEAD);
            ALCommonTaskController.CommonActionAddMonoTask(()=>
            {
                if(serial != _m_lShowSerial)
                    return;
                
                _showDone?.Invoke();
            }, wnd.bossDeadShowTimes);
        }
        
        /// <summary>
        /// 显示掉血tip
        /// </summary>
        public void showLossBloodTip(long _lossHp, Action _onPop = null)
        {
            if (_m_wLossBloodTip == null)
            {
                _onPop?.Invoke();
                return;
            }
            
            _m_wLossBloodTip.showLossBloodTip(_lossHp, _onPop);
        }

        /// <summary>
        /// 更新boss形象
        /// </summary>
        /// <param name="_reviveCount"></param>
        public void updateBossActor(int _reviveCount)
        {
            GGUIWndEveningDungeonBossActor needShowBossActor = _getBossActor(_reviveCount);
            if(needShowBossActor == null)
                return;

            if (needShowBossActor != _m_curShowBossActor)
            {
                _m_curShowBossActor?.hideWnd();
                
                _m_curShowBossActor = needShowBossActor;
            }

            //如未加载在此加载，释放由_m_lBossActorList队列释放控制
            if (!_m_curShowBossActor.isLoaded)
                _m_curShowBossActor.load();
            _m_curShowBossActor.regLoadDoneDelegate(() =>
            {
                _m_curShowBossActor.showWnd();
            });
        }

        private GGUIWndEveningDungeonBossActor _getBossActor(int _reviveCount)
        {
            if (_m_lBossActorList == null)
                return null;
            
            int bossActorCount = _m_lBossActorList.Count;
            if (bossActorCount <= 0)
                return null;
            
            if(_reviveCount < 0)
                _reviveCount = 0;
            
            int order = _reviveCount % bossActorCount;
            return _m_lBossActorList[order];
        }
    }
}