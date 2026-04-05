using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsExplorePvPDetail : _ANPGGUIBasicWnd<GGUIMonoMarsExplorePvPDetail>
    {
        private static GGUIWndMarsExplorePvPDetail _g_instance;
        [NotNull] public static GGUIWndMarsExplorePvPDetail instance { get { return _g_instance ??= new GGUIWndMarsExplorePvPDetail(); } }

        private NPGGuiWndTexture _m_myAvatarWnd;
        private NPGGuiWndTexture _m_enemyAvatarWnd;
        private NPGGUIWndPlayerIcon _m_myPlayerIconWnd;
        private NPGGUIWndPlayerIcon _m_enemyPlayerIconWnd;
        
        private GGUIWndMarsExplorePvPLog._ALogData _m_logData;
        private List<GGUIWndMarsExplorePvPLog._ALogData> _m_logDataList;
        private GGUIWndMarsExplorePvPLog._ALogData _m_prevLogData;
        private GGUIWndMarsExplorePvPLog._ALogData _m_nextLogData;

        private int _m_showSerialize; 


        protected override string _monoAssetPath { get { return GGUIMonoMarsExplorePvPDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsExplorePvPDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        public GGUIWndMarsExplorePvPDetail() 
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override void _onShowWnd()
        {
            _m_myAvatarWnd?.showWnd();
            _m_enemyAvatarWnd?.showWnd();
            _m_myPlayerIconWnd?.showWnd();
            _m_enemyPlayerIconWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_myAvatarWnd?.hideWnd();
            _m_enemyAvatarWnd?.hideWnd();
            _m_myPlayerIconWnd?.hideWnd();
            _m_enemyPlayerIconWnd?.hideWnd();

            _m_showSerialize = ALSerializeOpMgr.next();
        }
        protected override void _onReset()
        {
            _m_myAvatarWnd?.discardTexture();
            _m_enemyAvatarWnd?.discardTexture();
            _m_myPlayerIconWnd?.resetWnd();
            _m_enemyPlayerIconWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_myAvatarWnd?.discard();
            _m_myAvatarWnd = null;
            _m_enemyAvatarWnd?.discard();
            _m_enemyAvatarWnd = null;
            _m_myPlayerIconWnd?.discard();
            _m_myPlayerIconWnd = null;
            _m_enemyPlayerIconWnd?.discard();
            _m_enemyPlayerIconWnd = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevLog, _onClickPrevLog);
            ALUGUICommon.uncombineBtnClick(wnd.btnNextLog, _onClickNextLog);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgMyAvatar != null)
                _m_myAvatarWnd = new NPGGuiWndTexture(wnd.imgMyAvatar);
            if (wnd.imgEnemyAvatar != null)
                _m_enemyAvatarWnd = new NPGGuiWndTexture(wnd.imgEnemyAvatar);
            if (wnd.monoMyPlayerInfo != null)
                _m_myPlayerIconWnd = new NPGGUIWndPlayerIcon(wnd.monoMyPlayerInfo);
            if (wnd.monoEnemyPlayerInfo != null)
                _m_enemyPlayerIconWnd = new NPGGUIWndPlayerIcon(wnd.monoEnemyPlayerInfo);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);           
            ALUGUICommon.combineBtnClick(wnd.btnPrevLog, _onClickPrevLog);
            ALUGUICommon.combineBtnClick(wnd.btnNextLog, _onClickNextLog);
        }
        
        
        public void refreshWnd(GGUIWndMarsExplorePvPLog._ALogData _logData, List<GGUIWndMarsExplorePvPLog._ALogData> _logDataList)
        {
            _m_logData = _logData;
            _m_logDataList = _logDataList;
            _m_prevLogData = null;
            _m_nextLogData = null;
            if (_m_logDataList != null && _m_logData != null)
            {
                int curIndex = _m_logDataList.IndexOf(_m_logData);
                if (curIndex >= 0)
                {
                    if (curIndex > 0)
                        _m_prevLogData = _m_logDataList[curIndex - 1];
                    if (curIndex < _m_logDataList.Count - 1)
                        _m_nextLogData = _m_logDataList[curIndex + 1];
                }
            }
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_logData == null)
                return;

            int serialize = _m_showSerialize;
            wnd.setIsWin(_m_logData.isSuccess);
            wnd.setHasPrevLog(_m_prevLogData != null);
            wnd.setHasNextLog(_m_nextLogData != null);
            
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(_m_logData.getCreatedAt())));
            wnd.setLoadingState(true);
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (serialize != _m_showSerialize)
                    return;
                
                wnd.setLoadingState(false);
            });
            
            _m_logData.getMySideData((_myData) =>
            {
                if (serialize != _m_showSerialize)
                    return;
                
                ALUGUICommon.setLabelTxt(wnd.txtMyName, _myData.name);
                _m_myAvatarWnd?.setTexture(_myData.avatar);
                _m_myPlayerIconWnd?.setPlayerInfo(_myData.playerInfo);

                long myTotalTroops = _myData.oriSoldierNum;
                long myLossTroops = _myData.hurtSoldierNum;
                long mySurvivorTroops = myTotalTroops - myLossTroops;
                long myPowerLoss = myLossTroops * _myData.singleSoldierPower;

                ALUGUICommon.setLabelTxt(wnd.txtMyTotalTroops, myTotalTroops);
                ALUGUICommon.setLabelTxt(wnd.txtMyLossTroops, myLossTroops);
                ALUGUICommon.setLabelTxt(wnd.txtMySurvivorTroops, mySurvivorTroops);
                ALUGUICommon.setLabelTxt(wnd.txtMyPowerLoss, TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, myPowerLoss));
                stepCounter.addDoneStepCount();
            });
            _m_logData.getEnemySideData((_enemyData) =>
            {
                if (serialize != _m_showSerialize)
                    return;
                
                ALUGUICommon.setLabelTxt(wnd.txtEnemyName, _enemyData.name);
                _m_enemyAvatarWnd?.setTexture(_enemyData.avatar);
                _m_enemyPlayerIconWnd?.setPlayerInfo(_enemyData.playerInfo);
                wnd.setEnemyIsPlayer(_enemyData.playerInfo != null);

                long enemyTotalTroops = _enemyData.oriSoldierNum;
                long enemyLossTroops = _enemyData.hurtSoldierNum;
                long enemySurvivorTroops = enemyTotalTroops - enemyLossTroops;
                long enemyPowerLoss = enemyLossTroops * _enemyData.singleSoldierPower;

                ALUGUICommon.setLabelTxt(wnd.txtEnemyTotalTroops, enemyTotalTroops);
                ALUGUICommon.setLabelTxt(wnd.txtEnemyLossTroops, enemyLossTroops);
                ALUGUICommon.setLabelTxt(wnd.txtEnemySurvivorTroops, enemySurvivorTroops);
                ALUGUICommon.setLabelTxt(wnd.txtEnemyPowerLoss, TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, enemyPowerLoss));
                stepCounter.addDoneStepCount();
            });
        }


        private void _onClickPrevLog(GameObject _go)
        {
            refreshWnd(_m_prevLogData, _m_logDataList);
        }
        private void _onClickNextLog(GameObject _go)
        {
            refreshWnd(_m_nextLogData, _m_logDataList);
        }
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_PVP_DETAIL);
        }
    }
}
