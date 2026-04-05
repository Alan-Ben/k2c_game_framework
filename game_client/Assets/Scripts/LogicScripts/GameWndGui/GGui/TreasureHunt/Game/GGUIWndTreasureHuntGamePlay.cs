using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 太空寻宝游戏游玩界面
    /// </summary>
    public class GGUIWndTreasureHuntGamePlay : _ATALBasicUIWnd<GGUIMonoTreasureHuntGamePlay>
    {
        [NotNull] public static GGUIWndTreasureHuntGamePlay instance { get { return _g_instance ??= new GGUIWndTreasureHuntGamePlay(); } }
        private static GGUIWndTreasureHuntGamePlay _g_instance;
        
        
        private TreasureHuntGameController _m_gameController;
        private ALCommonEnableTaskController _m_touchMoveTask;
        private bool _m_bIsPlayingAudio;//是否正在播放移动音效
        private long _m_lShowSerialize;//窗口显示序列号
        private Vector2 _m_lastTouchPos;//上次触摸位置


        public GGUIWndTreasureHuntGamePlay() 
            : base(EALUIWndLayer.NORMAL)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntGamePlay.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntGamePlay.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsPlayingAudio = false;
            _m_lastTouchPos = Vector2.zero;
            _m_touchMoveTask.setDisable();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnPause, _onBtnPauseClick);
            ALUGUICommon.uncombineBtnPress(wnd.btnTouchMove, _onBtnTouchMove);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnPause, _onBtnPauseClick);
            ALUGUICommon.combineBtnPress(wnd.btnTouchMove, _onBtnTouchMove);
        }


        public void setGameController(TreasureHuntGameController _gameController)
        {
            _m_gameController = _gameController;
        }
        public void refreshDistance(float _currentDistance, float _targetDistance)
        {
            if (wnd == null || !_m_bIsShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtDistance, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_gameDistance_value, (int)_currentDistance));
            ALUGUICommon.setLabelTxt(wnd.txtDistanceProgressCur, (int)_currentDistance);
            ALUGUICommon.setLabelTxt(wnd.txtDistanceProgressTarget, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_distanceProgressTarget_value, (int)_targetDistance));
        }
        public void refreshBoostDistance(float _currentDistance, float _boostDistance)
        {
            if (wnd == null || !_m_bIsShow)
                return;
                
            ALUGUICommon.setLabelTxt(wnd.txtRunUp, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, (int)_currentDistance, (int)_boostDistance));
        }
        public void refreshHealthCount(int _healthCount, int _maxHealthCount)
        {
            if (wnd == null || !_m_bIsShow)
                return;
                
            // 这边 UI 上是保护次数，不是血量，所以统一减一，但是不能小于 0，
            _healthCount = Mathf.Max(0, _healthCount - 1);
            _maxHealthCount = Mathf.Max(0, _maxHealthCount - 1);
            ALUGUICommon.setLabelTxt(wnd.txtHealth, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _healthCount, _maxHealthCount));
        }
        public void refreshRewardGain(int _gainCount, int _maxCount)
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtRewardGain, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _gainCount, _maxCount));
        }
        public void refreshRewardTipDistance(float _nextRewardDistance)
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            wnd.setHasRewardTipShow(_nextRewardDistance > 0 && _nextRewardDistance <= wnd.showRewardTipDistance);
            ALUGUICommon.setLabelTxt(wnd.txtRewardTip, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_gameRewardTip_distance, (int)_nextRewardDistance));
        }
        public void refreshArea(TreasureHuntAreaRefObj _areaRefObj)
        {
            if (wnd == null || !_m_bIsShow || _areaRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtAreaName, TextTranslate.instance.getLanguage(_areaRefObj.name));
        }
        public void setRunUpShow()
        {
            if (wnd == null || !_m_bIsShow)
                return;
                
            wnd.setRunUpShow();
        }
        public void setGamingShow()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            wnd.setGamingShow();
        }

        //播放移动音效
        private void _playMoveAudio()
        {
            if (_m_bIsPlayingAudio || wnd == null || wnd.moveAudioId <= 0)
                return;

            //播放音效
            PlayAudioMgr.instance.playClip(wnd.moveAudioId);

            //设置正在播放状态
            _m_bIsPlayingAudio = true;

            //延时设置下次可播放
            long serialize = _m_lShowSerialize;
            ALCommonActionMonoTask.addMonoTask((() =>
            {
                if (_m_lShowSerialize != serialize)
                    return;

                _m_bIsPlayingAudio = false;
            }), wnd.dragMoveAudioDuration);
        }
        
        private void _onBtnPauseClick(GameObject _obj)
        {
            if (wnd == null || _m_gameController == null)
                return;

            if (AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.TREASURE_HUNT_GAMEPLAY_QUIT))
                _m_gameController.pauseGame();
            else
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_GAME_MAIN);
                GNodeTreasureHuntGameMain.addNode();
            }
        }
        private void _onBtnTouchMove()
        {
            if (_m_gameController == null)
                return;
                
            Vector2 screenPos = ALInputControl.instance.getBtnUnityPos(EALGUIOpButtonType.OP_BTN);
            _m_gameController.setMoveTarget(screenPos);
            if (_m_lastTouchPos != screenPos)
            {
                _m_lastTouchPos = screenPos;
                //播放移动音效
                _playMoveAudio();
            }
        }
        private void _onBtnTouchMove(bool _isPress, PointerEventData _)
        {
            if (_isPress)
            {
                _m_touchMoveTask.setDisable();
                _m_touchMoveTask = ALCommonEnableTickActionMonoTask.addMonoTask(_onBtnTouchMove);
            }
            else
            {
                _m_bIsPlayingAudio = false;
                _m_touchMoveTask.setDisable();
            }

        }
    }
}