using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// boss Actor
    /// </summary>
    public class GGUIWndEveningDungeonBossActor : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoEveningDungeonBossActor>
    {
        private string _m_sAssetPath;
        private string _m_sObjName;
        
        public GGUIWndEveningDungeonBossActor(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        public void setBossState(EEveningDungeonBossState _state)
        {
            if(wnd == null || wnd.multiStateShow == null)
                return;
            
            wnd.multiStateShow.setShowData(_state);
        }
        
        // /// <summary>
        // /// 播放动画
        // /// </summary>
        // public void playActorAnimation(EEveningDungeonBossState _bossState, Action _onPlayDone = null)
        // {
        //     if (wnd == null || wnd.skeletonGraphic == null)
        //     {
        //         _onPlayDone?.Invoke();
        //         return;
        //     }
        //
        //     EveningDungeonBossStateActorShowConfig showConfig = wnd.getActorShowConfig(_bossState);
        //     if (showConfig == null || showConfig.animationConfig == null)
        //     {
        //         _onPlayDone?.Invoke();
        //         return;
        //     }
        //     
        //     showConfig.animationConfig.playAnimation(wnd.skeletonGraphic, _onPlayDone);
        // }
    }
}