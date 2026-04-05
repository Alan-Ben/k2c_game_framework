using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 爬塔战斗管理器
    /// </summary>
    public class GTowerBattleSceneMgr
    {
        private static GTowerBattleSceneMgr _m_instance = new GTowerBattleSceneMgr();

        public static GTowerBattleSceneMgr instance
        {
            get
            {
                if (null == _m_instance)
                    _m_instance = new GTowerBattleSceneMgr();
                return _m_instance;
            }
        }
        
        private bool _m_isInit = false;
        private GTDTowerBattleSceneMono _m_sceneMono;
        
        private TowerChallengeResult _m_result;

        GTowerBattleStage _m_towerBattleStage;
        TowerChapterRefObj _m_towerChapterRefObj;

        private bool _m_isBattleEnd = false; // 战斗是否结束

        public GTowerBattleSceneMgr()
        {
        }
        
        public void initSceneMono(GTDTowerBattleSceneMono _mono)
        {
            if (_m_isInit)
                return;
           
            _m_sceneMono = _mono;
            if(_m_sceneMono == null)
                return;

            _m_isInit = true;
        }

        /// <summary>
        /// 初始化舞台，战斗信息
        /// </summary>
        /// <param name="_result"></param>
        /// <param name="_chapterRef"></param>
        /// <param name="_onComplete"></param>
        public void initStage(TowerChallengeResult _result, Action _onComplete = null)
        {
            if (_result == null || _m_sceneMono == null)
            {
                _onComplete?.Invoke();
                return;
            }

            _m_towerChapterRefObj = GRefdataCoreMgr.instance.towerChapterRefCore.getRef(_result.targetChapterId);
            if (_m_towerChapterRefObj == null)
            {
                _onComplete?.Invoke();
                return;
            }
            
            _m_result = _result;
            if(_m_towerBattleStage != null)
                _m_towerBattleStage.discard();
            
            _m_towerBattleStage = new GTowerBattleStage(_result, _m_sceneMono, _m_towerChapterRefObj.chapter_scene_go_index, _m_towerChapterRefObj);
            _m_towerBattleStage.load(_onComplete);
        }

        public void showTalkSFX()
        {
            if (_m_towerBattleStage != null)
                _m_towerBattleStage.regLoadDoneDelegate(_m_towerBattleStage.showTalkSFX);
        }

        public void hideTalkSFX()
        {
            if (_m_towerBattleStage != null)
                _m_towerBattleStage.regLoadDoneDelegate(_m_towerBattleStage.hideTalkSFX);
        }
        
        public void discard()
        {
            _m_sceneMono = null;

            _m_isInit = false;
            
            if (_m_towerBattleStage != null) 
                _m_towerBattleStage.discard();
            _m_towerBattleStage = null;
        }
    }
}