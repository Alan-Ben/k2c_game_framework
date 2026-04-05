using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GTowerBattleStage : _AALBasicLoadObj
    {
        private Transform _m_stageRoot;
        private readonly NPGGoIndex _m_goIndex;
        private GTDTowerBattleStageMono _m_stageMono;
        private TowerChapterRefObj _m_towerChapterRefObj;
        
        
        private bool _m_isBattleEnd = false; // 战斗是否结束
        private long _m_serialize;


        private NPGGoIndex _m_playerGoIndex;
        private NPGGoIndex _m_bossGoIndex;
        private GTowerActorView _m_playerActorView;
        private GTowerActorView _m_bossActorView;
        
        public GTowerBattleStage(TowerChallengeResult _result, GTDTowerBattleSceneMono _sceneMono, NPGGoIndex _goIndex, TowerChapterRefObj _towerChapterRefObj)
        {
            if (_result == null || _towerChapterRefObj == null || _sceneMono == null)
                return;
            _m_stageRoot = _sceneMono.stageRoot == null ? _sceneMono.transform : _sceneMono.stageRoot;
            
            _m_goIndex = _goIndex;
            _m_towerChapterRefObj = _towerChapterRefObj;

            _m_playerGoIndex = NPPlayer.instance?.playerInfo?.curSkinRef?.td_show;
            _m_bossGoIndex = _m_towerChapterRefObj.boss_go_index;
            
            _m_playerActorView = new GTowerActorView(_m_playerGoIndex, _sceneMono.talkAniName);
            _m_playerActorView.load();
            _m_bossActorView = new GTowerActorView(_m_bossGoIndex, _sceneMono.talkAniName);
            _m_bossActorView.load();
        }

        protected override void _loadOp()
        {
            GGoResCore.instance.loadObj(_m_goIndex, _assetHandle =>
            {
                if (_assetHandle == null || _assetHandle.loadedInfo == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _setLoadDone();
                    return;
                }
                //获取资源对象
                GameObject assetGo = _assetHandle.loadedInfo.obj;
                if (null == assetGo)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _setLoadDone();
                    return;
                }
                GameObject go = GameObject.Instantiate(assetGo);

                if (null == go)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex实例化错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _setLoadDone();
                    return;
                }
                _m_stageMono = go.GetComponent<GTDTowerBattleStageMono>();
                if (_m_stageMono == null)
                {
                    ALLog.Error($"[**Tower**] 爬塔资源 {_m_goIndex} 不含有 GTDTowerBattleStageMono ");
                    ALUnityCommon.releaseGameObj(go.gameObject);
                    _setLoadDone();
                    return;
                }

                go.transform.parent = _m_stageRoot;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                
                if (_m_playerActorView != null) 
                    _m_playerActorView.setParent(_m_stageMono.playerParent);

                if(_m_bossActorView != null)
                    _m_bossActorView.setParent(_m_stageMono.bossParent);
                hideTalkSFX();
                _setLoadDone();
            });
        }

        protected override void _discard()
        {
            _m_serialize = ALSerializeOpMgr.next();
            if (_m_stageMono != null && null != _m_stageMono.gameObject)
            {
                ALUnityCommon.releaseGameObj(_m_stageMono.gameObject);
            }
            _m_stageMono = null;

            if (_m_playerActorView != null) 
                _m_playerActorView.discard();
            _m_playerActorView = null;
            
            if (_m_bossActorView != null) 
                _m_bossActorView.discard();
            _m_bossActorView = null;
        }
      


        public void endBattle()
        {
            _m_serialize = ALSerializeOpMgr.next();
        }
        
        public void showTalkSFX()
        {
            if (_m_stageMono != null)
            {
                if(AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes)
                    ALUGUICommon.setGameObjEnable(_m_stageMono.sfxList10x, true);
                else
                    ALUGUICommon.setGameObjEnable(_m_stageMono.sfxList, true);
            }
        }

        public void hideTalkSFX()
        {
            if (_m_stageMono != null)
            {
                ALUGUICommon.setGameObjEnable(_m_stageMono.sfxList10x, false);
                ALUGUICommon.setGameObjEnable(_m_stageMono.sfxList, false);
            }
        }
        
    }
}