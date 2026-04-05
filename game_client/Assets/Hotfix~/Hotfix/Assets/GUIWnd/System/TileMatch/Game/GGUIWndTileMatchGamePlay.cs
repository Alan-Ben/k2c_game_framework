using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using GOE;
using Hotfix.TileMatchEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 游戏玩法子窗口
    /// </summary>
    public class GGUIWndTileMatchGamePlay : _AHotfixBaseSubWnd<GGUIMonoTileMatchGamePlay>
    {
        private long _m_lShowSerialize;
        private long _m_lGamelogicStartSerialize;
        
        private TileMatchGamePlayUIConfig _m_gamePlayUIConfig;
        
        private bool _m_bHasPrepareCheckerCache;
        private GGUIWndTileMatchCheckerCacheMgr _m_CheckerCacheMgr;//格子的缓存

        private CommonOpMaskLogic _m_opMaskLogic;//操作遮罩
        
        private GGUIWndTileMatchModeTab _m_wAdvancedModeTab;//高级模式tab
        private GGUIWndTileMatchModeTab _m_wExtremeModeTab;//极限模式tab
        private List<TileMatchGameModelConfig> _m_lGameModelConfigList;//游戏模式配置列表

        private GGUISubWndTileMatchTask _m_wTileMatchTask;//三消任务窗口
        
        [NotNull] private List<CommonUISfxObj> _m_lCheckerboardSfxList = new List<CommonUISfxObj>();//棋盘特效列表
        [NotNull] private TweenContainer _m_tweenContainer = new TweenContainer();
        
        private TileMatchGameLogic _m_gameLogic;
        
        public GGUIWndTileMatchGamePlay(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        public TileMatchGamePlayUIConfig gameUIConfig { get { return _m_gamePlayUIConfig; } }

        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            _m_gamePlayUIConfig = new TileMatchGamePlayUIConfig();
            _m_gamePlayUIConfig.init(hotfixWnd.gameUiConfig);
            
            _m_CheckerCacheMgr?.discardAll();
            _m_bHasPrepareCheckerCache = false;
            _m_CheckerCacheMgr = new GGUIWndTileMatchCheckerCacheMgr(hotfixWnd.checkerCacheParent, 1, TileMatchUtil.column * TileMatchUtil.row * 2);
            
            _m_opMaskLogic?.forceCloseOpMask();
            if (_m_opMaskLogic == null)
                _m_opMaskLogic = new CommonOpMaskLogic(_dealOpenOpMask, _dealCloseOpMask);

            if (hotfixWnd.advancedModeTab != null)
            {
                _m_wAdvancedModeTab = new GGUIWndTileMatchModeTab(ETileMatch_ModeType.ADVANCED, hotfixWnd.advancedModeTab);
                _m_wAdvancedModeTab.onClickButton += _onTabClick;
            }
            if (hotfixWnd.extremeModeTab != null)
            {
                _m_wExtremeModeTab = new GGUIWndTileMatchModeTab(ETileMatch_ModeType.EXTREME, hotfixWnd.extremeModeTab);
                _m_wExtremeModeTab.onClickButton += _onTabClick;
            }

            _m_lGameModelConfigList = new List<TileMatchGameModelConfig>();
            if (hotfixWnd.gameModelConfigList != null)
            {
                TileMatchGameModelConfig gameModelConfig = null;
                MonoSkin gameModelConfigMono = null;
                for (int i = 0, count = hotfixWnd.gameModelConfigList.Count; i < count; i++)
                {
                    gameModelConfigMono = hotfixWnd.gameModelConfigList[i];
                    if (gameModelConfigMono != null)
                    {
                        gameModelConfig = new TileMatchGameModelConfig();
                        gameModelConfig.init(gameModelConfigMono);
                        _m_lGameModelConfigList.Add(gameModelConfig);
                    }
                }
            }
            
            if (hotfixWnd.monoTileMatchTask != null)
                _m_wTileMatchTask = new GGUISubWndTileMatchTask(hotfixWnd.monoTileMatchTask);
        }
        
        protected override void _onDiscard()
        {
            _m_CheckerCacheMgr?.discardAll();
            _m_CheckerCacheMgr = null;
            _m_bHasPrepareCheckerCache = false;
            
            _m_opMaskLogic?.forceCloseOpMask();
            _m_opMaskLogic = null;
            
            _discardAllCheckerboardSfx();
            _m_tweenContainer.killAllDoTween();

            if (_m_wAdvancedModeTab != null)
            {
                _m_wAdvancedModeTab.onClickButton -= _onTabClick;
                _m_wAdvancedModeTab.discard();
            }
            _m_wAdvancedModeTab = null;

            if (_m_wExtremeModeTab != null)
            {
                _m_wExtremeModeTab.onClickButton -= _onTabClick;
                _m_wExtremeModeTab.discard();
            }
            _m_wExtremeModeTab = null;
            
            _m_lGameModelConfigList?.Clear();
            _m_lGameModelConfigList = null;
            
            _m_wTileMatchTask?.discard();
            _m_wTileMatchTask = null;
        }
        
        protected override void _onShowWnd()
        {
            long showSerialize = _m_lShowSerialize = ALSerializeOpMgr.next();

            // 刷新当前游戏模式
            TileMatchModeRefObj tileMatchModeRefObj =
                HotfixRefdataCoreMgr.instance.tileMatchModeRefCore.getRef((long) HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType());
            // 若存储器中记录的游戏模式未解锁或未满足解锁条件，切换游戏模式
            if (tileMatchModeRefObj == null || !tileMatchModeRefObj.gameModeIsUnlock())
            {
                int modeCount = HotfixRefdataCoreMgr.instance.tileMatchModeRefCore.refList.Count;
                for (int i = 0; i < modeCount; i++)
                {
                    tileMatchModeRefObj = HotfixRefdataCoreMgr.instance.tileMatchModeRefCore.refList[i];
                    if (tileMatchModeRefObj == null || !tileMatchModeRefObj.gameModeIsUnlock())
                    {
                        tileMatchModeRefObj = null;
                    }
                    else
                    {
                        HotfixAccountSettingMgr.instance.hotfixAccountSetting.setTileMatchModelType(tileMatchModeRefObj.type);
                        break;
                    }
                }
            }
            if (tileMatchModeRefObj == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(HotfixTransKeyConst.tilematch_diedTipDesc_none);
                Debug.LogError($"[TileMatchGamePlay] 从tilematch_mode找不到当前可玩的游戏模式, 检查配表是否配置错误");
                return;
            }
            
            _refreshTab();
            _m_wTileMatchTask?.showWnd();

            _startGameLogic();
            
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
            ALMsgSys.RegisterMsgAct(HotfixMsgType.ON_TILEMATCH_ACTIVITY_TOTAL_SCORE_CHG, _refreshTabUnlock);
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _onCustomReload);
            ALMsgSys.UnregisterMsgAct(HotfixMsgType.ON_TILEMATCH_ACTIVITY_TOTAL_SCORE_CHG, _refreshTabUnlock);

            // 停止游戏逻辑
            _stopGameLogic();
            
            _m_opMaskLogic?.forceCloseOpMask();
            
            _discardAllCheckerboardSfx();
            _m_tweenContainer.killAllDoTween();
            
            _m_wAdvancedModeTab?.hideWnd();
            _m_wExtremeModeTab?.hideWnd();
            
            _m_wTileMatchTask?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_opMaskLogic?.forceCloseOpMask();
            
            _discardAllCheckerboardSfx();
            _m_tweenContainer.killAllDoTween();
            
            _m_wAdvancedModeTab?.resetWnd();
            _m_wExtremeModeTab?.resetWnd();
            
            _m_wTileMatchTask?.resetWnd();
        }

        #region 棋子格子生成销毁

        /// <summary>
        /// 预热棋子缓存(这样可以使后面使用cache时不需要通过回调)
        /// </summary>
        private void _prepareCheckerCache(Action _complete)
        {
            if (_m_CheckerCacheMgr == null || _m_bHasPrepareCheckerCache)
            {
                _complete?.Invoke();
                return;
            }
            
            long showSerialize = _m_lShowSerialize;
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(()=>
            {
                if(showSerialize != _m_lShowSerialize)
                    return;
                
                _m_bHasPrepareCheckerCache = true;
                _complete?.Invoke();
            });
            
            foreach (var tileMatchBlockShowRefObj in HotfixRefdataCoreMgr.instance.tileMatchBlockShowRefCore.refList)
            {
                if(tileMatchBlockShowRefObj == null || tileMatchBlockShowRefObj.prefab_asset_path == null || !tileMatchBlockShowRefObj.prefab_asset_path.enable)
                    continue;
                
                stepCounter.chgTotalStepCount(1);
                _m_CheckerCacheMgr.loadCache(tileMatchBlockShowRefObj, (_cache) =>
                {
                    if(showSerialize != _m_lShowSerialize)
                        return;
                    
                    stepCounter.addDoneStepCount();
                });
            }
            
            stepCounter.addDoneStepCount();
        }

        public GGUIWndTileMatchChecker popTileBlockShow(TileMatchBlockShowRefObj _tileMatchBlockShowRefObj)
        {
            if (_m_CheckerCacheMgr == null || _tileMatchBlockShowRefObj == null)
                return null;
            
            if (!_m_bHasPrepareCheckerCache)
            {
                Debug.LogError($"[GGUIWndTileMatchGamePlay getTileBlockShow] cache未预加载");
                return null;
            }

            GGUIWndTileMatchChecker checker = _m_CheckerCacheMgr.popItem(_tileMatchBlockShowRefObj);
            if (checker != null && checker.wnd != null && checker.wnd.transform != null && hotfixWnd != null)
            {
                checker.setGameShow(this);
                
                Transform trans = checker.wnd.transform;
                trans.SetParent(hotfixWnd.checkerboardArea);
                trans.localScale = Vector3.one;
            }

            return checker;
        }

        public void pushBackTileBlockShow(GGUIWndTileMatchChecker _checker)
        {
            if(_checker == null)
                return;
            
            _checker.setGameShow(null);

            if (_m_CheckerCacheMgr == null || _checker.blockShowRefObj == null)
            {
                GGUIHotfixCommonMono mono = _checker.wnd;
                _checker.discard();
                ALUnityCommon.releaseGameObj(mono);
                _checker = null;
                mono = null;
                return;
            }

            _m_CheckerCacheMgr.pushBackItem(_checker.blockShowRefObj, _checker);
        }

        #endregion
        
        /// <summary>
        /// 获取棋盘中心的局部坐标位置(相对于checkerboardArea的)
        /// </summary>
        /// <returns></returns>
        public Vector2 getCheckerboardCenterLocalPosition()
        {
            if (hotfixWnd == null || hotfixWnd.checkerboardArea == null)
                return Vector2.zero;

            Rect checkerboardAreaRect = hotfixWnd.checkerboardArea.rect;
            Vector2 pivotLocalPos = new Vector2(checkerboardAreaRect.width * hotfixWnd.checkerboardArea.pivot.x, checkerboardAreaRect.height * hotfixWnd.checkerboardArea.pivot.y);
            return new Vector2(checkerboardAreaRect.width / 2, checkerboardAreaRect.height / 2) - pivotLocalPos;
        }
        
        /// <summary>
        /// 获取格子的UI局部坐标位置(相对于checkerboardArea的)
        /// </summary>
        /// <param name="_logicPos"></param>
        /// <returns></returns>
        public Vector2 getBlockUiLocalPosition(Vector2Int _logicPos)
        {
            if (hotfixWnd == null || hotfixWnd.checkerboardArea == null)
                return Vector2.zero;

            Vector2 pivot = hotfixWnd.checkerboardArea.pivot;
            float x = -hotfixWnd.checkerboardArea.rect.width * pivot.x + hotfixWnd.cubeWidth * 0.5f +
                      _logicPos.x * hotfixWnd.cubeWidth + hotfixWnd.cubeSpacingX * (_logicPos.x);

            float y = -hotfixWnd.checkerboardArea.rect.height * pivot.y + hotfixWnd.cubeHeight * 0.5f +
                      _logicPos.y * hotfixWnd.cubeHeight + hotfixWnd.cubeSpacingY * (_logicPos.y + 1);

            return new Vector2(x, y);
        }
        
        public Vector3 getBlockUiWorldPosition(Vector2Int _logicPos)
        {
            if (hotfixWnd == null || hotfixWnd.checkerboardArea == null)
                return Vector3.zero;

            Vector3 localPos = getBlockUiLocalPosition(_logicPos);
            
            return hotfixWnd.checkerboardArea.TransformPoint(localPos);
        }

        #region 窗口消息

        private void _onCustomReload()
        {
            _refreshTabUnlock();
        }
        
        #endregion

        #region 游戏逻辑

        /// <summary>
        /// 开始游戏逻辑
        /// </summary>
        private void _startGameLogic()
        {
            _stopGameLogic();

            long gameLogicStartSerialize = _m_lGamelogicStartSerialize;
            // 开始游戏逻辑前先保证cache已经预加载完成
            _prepareCheckerCache(() =>
            {
                if(gameLogicStartSerialize != _m_lGamelogicStartSerialize)
                    return;
                
                _m_gameLogic = new TileMatchGameLogic(this);
                _m_gameLogic.start(()=>
                {
                    if(gameLogicStartSerialize != _m_lGamelogicStartSerialize)
                        return;
                    _onGameLogicStartComplete();
                }, ()=>
                {
                    if(gameLogicStartSerialize != _m_lGamelogicStartSerialize)
                        return;
                    _onGameLogicStartFail();
                });
            });
        }
        
        /// <summary>
        /// 游戏逻辑开始完成调用方法
        /// </summary>
        private void _onGameLogicStartComplete()
        {
            _m_wTileMatchTask?.showWnd();
            _m_wTileMatchTask?.setGameLogic(_m_gameLogic);
        }
        
        /// <summary>
        /// 游戏逻辑开始失败调用方法
        /// </summary>
        private void _onGameLogicStartFail()
        {
        }
        
        /// <summary>
        /// 停止游戏逻辑
        /// </summary>
        private void _stopGameLogic()
        {
            _m_lGamelogicStartSerialize = ALSerializeOpMgr.next();
            if (_m_gameLogic != null)
            {
                _m_gameLogic.stop();
            }

            _m_gameLogic = null;
        }

        #endregion
        
        #region 操作遮罩

        /// <summary>
        /// 打开操作遮罩(传入CommonOpMaskLogic的回调)
        /// </summary>
        private void _dealOpenOpMask()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.setGameObjEnable(hotfixWnd.opMask, true);
        }
        
        /// <summary>
        /// 关闭操作遮罩(传入CommonOpMaskLogic的回调)
        /// </summary>
        private void _dealCloseOpMask()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.setGameObjEnable(hotfixWnd.opMask, false);
        }
        
        /// <summary>
        /// 打开操作遮罩(外部调用, 对_m_opMaskLogic进行操作)
        /// </summary>
        /// <returns></returns>
        public int openOpMask()
        {
            if (_m_opMaskLogic == null)
                return 0;

            return _m_opMaskLogic.openOpMask();
        }

        /// <summary>
        /// 关闭操作遮罩(外部调用, 对_m_opMaskLogic进行操作))
        /// </summary>
        /// <param name="_serialize"></param>
        public void closeOpMask(int _serialize)
        {
            if (_m_opMaskLogic == null)
                return;

            _m_opMaskLogic.closeOpMask(_serialize);
        }
        
        #endregion

        #region 具体表现

        /// <summary>
        /// 弹出死局提示窗体
        /// </summary>
        /// <param name="_sureAction"></param>
        public void showDiedTipWnd(Action _sureAction)
        {
            NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(HotfixTransKeyConst.tilematch_diedTipDesc_none),
                TextTranslate.instance.getLanguage(TransKeyConst.ok), _sureAction,true, HotfixTransKeyConst.tilematch_diedTipTitle_none);
        }
        
        #endregion
        
        #region 操作提示

        /// <summary>
        /// 显示操作提示
        /// </summary>
        /// <param name="_logicPos">提示显示的逻辑位置</param>
        /// <param name="_tipDic">提示显示的方向</param>
        public void showOpTip(Vector2Int _logicPos, ETileMatchDirection _tipDic)
        {
            if(hotfixWnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(hotfixWnd.opTipGameObject, true);
            if(hotfixWnd.opTipGameObject)
                hotfixWnd.opTipGameObject.transform.position = getBlockUiWorldPosition(_logicPos);

            switch (_tipDic)
            {
                case ETileMatchDirection.Left:
                    _playAnimation(hotfixWnd.opTipAnimation, hotfixWnd.moveLeftOpTipAniName, null);
                    break;
                case ETileMatchDirection.Right:
                    _playAnimation(hotfixWnd.opTipAnimation, hotfixWnd.moveRightOpTipAniName, null);
                    break;
                case ETileMatchDirection.Down:
                    _playAnimation(hotfixWnd.opTipAnimation, hotfixWnd.moveDownOpTipAniName, null);
                    break;
                case ETileMatchDirection.Up:
                    _playAnimation(hotfixWnd.opTipAnimation, hotfixWnd.moveUpOpTipAniName, null);
                    break;
            }
        }

        /// <summary>
        /// 隐藏操作提示
        /// </summary>
        public void hideOpTip()
        {
            if(hotfixWnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(hotfixWnd.opTipGameObject, false);
        }
        
        #endregion

        #region 通用动画

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_ani"></param>
        /// <param name="_aniName"></param>
        private void _playAnimation(Animation _ani, string _aniName, Action _complete)
        {
            if (_ani == null || string.IsNullOrEmpty(_aniName))
            {
                _complete?.Invoke();
                return;
            }

            _ani.Play(_aniName, _complete);
        }
        
        private void _sampleAnimation(Animation _ani, string _aniName, float _normalizeTime)
        {
            if (_ani == null || string.IsNullOrEmpty(_aniName))
                return;

            _ani.Sample(_aniName, _normalizeTime);
        }

        #endregion

        #region 棋盘特效

        /// <summary>
        /// 播放棋盘特效
        /// </summary>
        /// <param name="_sfxId"></param>
        /// <param name="_localPos">特效位置</param>
        /// <param name="_rotationAngleZ">特效旋转角度</param>
        public long playCheckerboardSfx(long _sfxId, Vector2 _localPos, float _rotationAngleZ = 0, Action _onComplete = null)
        {
            if (hotfixWnd == null || hotfixWnd.checkerboardSfxParent == null || _sfxId <= 0)
            {
                _onComplete?.Invoke();
                return 0;
            }

            CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(_sfxId, hotfixWnd.checkerboardSfxParent);
            if (sfxObj == null)
            {
                _onComplete?.Invoke();
                return 0;
            }

            long showSerialize = _m_lShowSerialize;
            _m_lCheckerboardSfxList.Add(sfxObj);
            sfxObj.regLoadDoneDelegate(() =>
            {
                if(_m_lShowSerialize != showSerialize)
                    return;
                
                sfxObj.setLocalPos(_localPos);
                sfxObj.setLocalRotation(Quaternion.Euler(0, 0, _rotationAngleZ));
            });
            sfxObj.regPlayCompleteDelegate(() =>
            {
                if(_m_lShowSerialize != showSerialize)
                    return;
                
                _m_lCheckerboardSfxList.Remove(sfxObj);
                _onComplete?.Invoke();
            });
            return sfxObj.sfxSerialize;
        }
        
        public long playCheckerboardSfx(long _sfxId, Vector2Int logicPos, Action _onComplete = null)
        {
            return playCheckerboardSfx(_sfxId, getBlockUiLocalPosition(logicPos), 0, _onComplete);
        }

        /// <summary>
        /// 在棋盘上播放飞行特效
        /// </summary>
        public void playCheckerboardFlySfx(long _sfxId, Vector2 _startPos, Vector2 _endPos, float _flyTime, Action _onFlyDone = null)
        {
            if (hotfixWnd == null || hotfixWnd.checkerboardSfxParent == null || _sfxId <= 0)
            {
                _onFlyDone?.Invoke();
                return;
            }
            
            CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(_sfxId, hotfixWnd.checkerboardSfxParent);
            if (sfxObj == null)
            {
                _onFlyDone?.Invoke();
                return;
            }
            
            long showSerialize = _m_lShowSerialize;
            _m_lCheckerboardSfxList.Add(sfxObj);
            sfxObj.regLoadDoneDelegate(() =>
            {
                if(_m_lShowSerialize != showSerialize)
                    return;

                if (sfxObj.sfxTrans == null)
                {
                    _onFlyDone?.Invoke();
                    return;
                }
                
                sfxObj.setLocalPos(_startPos);
                _m_tweenContainer.regDoTween(sfxObj.sfxTrans.DOLocalMove(_endPos, _flyTime)
                    .SetEase(Ease.Linear).OnComplete(
                        () =>
                        {
                            if(_m_lShowSerialize != showSerialize)
                                return;

                            _m_lCheckerboardSfxList.Remove(sfxObj);
                            _onFlyDone?.Invoke();
                        }));
            });
        }

        public void playCheckerboardFlySfx(long _sfxId, Vector2Int _startLogicPos, Vector2Int _endLogicPos, float _flyTime, Action _onFlyDone = null)
        {
            playCheckerboardFlySfx(_sfxId, getBlockUiLocalPosition(_startLogicPos), getBlockUiLocalPosition(_endLogicPos), _flyTime, _onFlyDone);
        }
        
        public void discardCheckerboardSfx(long _sfxSerialize)
        {
            int sfxCount = _m_lCheckerboardSfxList.Count;
            CommonUISfxObj sfxObj = null;
            for (int i = 0; i < sfxCount; i++)
            {
                sfxObj = _m_lCheckerboardSfxList[i];
                if (sfxObj != null && sfxObj.sfxSerialize == _sfxSerialize)
                {
                    sfxObj.forceDiscard();
                    _m_lCheckerboardSfxList.RemoveAt(i);
                    break;
                }
            }
        }
        
        /// <summary>
        /// 销毁所有棋盘特效
        /// </summary>
        private void _discardAllCheckerboardSfx()
        {
            int sfxCount = _m_lCheckerboardSfxList.Count;
            CommonUISfxObj sfxObj = null;
            for (int i = 0; i < sfxCount; i++)
            {
                sfxObj = _m_lCheckerboardSfxList[i];
                if(sfxObj != null)
                    sfxObj.forceDiscard();
            }
            _m_lCheckerboardSfxList.Clear();
        }

        #endregion

        #region 模式tab

        /// <summary>
        /// 刷新tab的可用(已解锁)状态
        /// </summary>
        private void _refreshTabUnlock()
        {
            _m_wAdvancedModeTab?.refreshTabUnlock();
            _m_wExtremeModeTab?.refreshTabUnlock();
        }

        /// <summary>
        /// 刷新tab
        /// </summary>
        private void _refreshTab()
        {
            // 显示tab
            _m_wAdvancedModeTab?.showWnd();
            _m_wExtremeModeTab?.showWnd();

            // 当前游戏模式
            ETileMatch_ModeType gameMode = HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType();
            
            _refreshTabUnlock();//刷新tab的已解锁状态
            
            if(_m_wAdvancedModeTab != null)
                _m_wAdvancedModeTab.setSelected(_m_wAdvancedModeTab.modeType == gameMode);
            
            if(_m_wExtremeModeTab != null)
                _m_wExtremeModeTab.setSelected(_m_wExtremeModeTab.modeType == gameMode);
        }

        /// <summary>
        /// 当模式tab被点击
        /// </summary>
        /// <param name="_modeTab"></param>
        private void _onTabClick(GGUIWndTileMatchModeTab _modeTab)
        {
            if(_modeTab == null || !isShow || !_modeTab.isShow)
                return;

            // 若当前该tab为选中, 表明它要变成未选中
            if (_modeTab.isOn)
            {
                TileMatchModeRefObj normalModeRefObj = HotfixRefdataCoreMgr.instance.tileMatchModeRefCore.getRef((long) ETileMatch_ModeType.NORMAL);
                if(normalModeRefObj == null || !normalModeRefObj.gameModeIsUnlock())
                    return;
                else
                {
                    HotfixAccountSettingMgr.instance.hotfixAccountSetting.setTileMatchModelType(ETileMatch_ModeType.NORMAL);
                    _modeTab.setSelected(false);

                    // 播放切换模式的动画
                    _playChgModelAnimation(ETileMatch_ModeType.NORMAL);
                }
            }
            // 若当前该tab为未选中, 表明它要变成选中
            else
            {
                TileMatchModeRefObj modeRefObj = _m_gameLogic?.nowGameModeRefObj;
                if(modeRefObj == null || !modeRefObj.gameModeIsUnlock())
                    return;
                else
                {
                    HotfixAccountSettingMgr.instance.hotfixAccountSetting.setTileMatchModelType(_modeTab.modeType);
                    
                    _m_wAdvancedModeTab?.setSelected(false);
                    _m_wExtremeModeTab?.setSelected(false);
                    
                    _modeTab.setSelected(true);
                    
                    // 播放切换模式的动画
                    _playChgModelAnimation(_modeTab.modeType);
                }
            }

            // 先隐藏任务窗口
            _m_wTileMatchTask?.hideWnd();

            // 游戏模式没变的情况都会在上面直接返回, 到这一步时, 游戏模式一定发生了变化, 重新开始游戏逻辑
            _startGameLogic();
        }

        /// <summary>
        /// 播放切换模式的动画
        /// </summary>
        private void _playChgModelAnimation(ETileMatch_ModeType _modeType)
        {
            if(hotfixWnd == null || hotfixWnd.chgModelAnimation == null)
                return;
            
            TileMatchGameModelConfig modelConfig = _getGameModelConfig(_modeType);
            if(modelConfig == null || string.IsNullOrEmpty(modelConfig.changeToModelShowAniName))
                return;
            
            _playAnimation(hotfixWnd.chgModelAnimation, modelConfig.changeToModelShowAniName, null);
        }
        
        /// <summary>
        /// 获取游戏模式配置
        /// </summary>
        /// <param name="_gameModelType"></param>
        private TileMatchGameModelConfig _getGameModelConfig(ETileMatch_ModeType _gameModelType)
        {
            if(_m_lGameModelConfigList == null)
                return null;

            TileMatchGameModelConfig selectGameModelConfig = null;
            for (int i = 0, count = _m_lGameModelConfigList.Count; i < count; i++)
            {
                TileMatchGameModelConfig gameModelConfig = _m_lGameModelConfigList[i];
                if(gameModelConfig == null)
                    continue;

                if (gameModelConfig.gameModelTypeStr == _gameModelType.ToString())
                    return gameModelConfig;
            }

            return null;
        }
        
        #endregion
    }
}