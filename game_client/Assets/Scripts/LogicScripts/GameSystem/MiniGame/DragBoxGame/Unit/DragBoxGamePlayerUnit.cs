using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    /// <summary>
    /// 推箱子游戏玩家单元
    /// </summary>
    public class DragBoxGamePlayerUnit : _ADragBoxGameUnit
    {
        private GTDMonoDragBoxGamePlayerCuteActor _m_playerCuteActorMono;
        
        private GTDPlayerCuteActor _m_playerCuteActor;//玩家Q版形象
        [NotNull] private AnimatorControlRegInfo _m_playerAnimatorControlRegInfo = new AnimatorControlRegInfo();//玩家动画控制注册信息

        private NPGGoIndex _m_playerFollowerGoIndex;//玩家跟随物体GoIndex
        private GameObject _m_playerFollowerGo;//玩家跟随物体
        [NotNull] private AnimatorControlRegInfo _m_playerFollowerAnimatorControlRegInfo = new AnimatorControlRegInfo();//玩家跟随物体动画控制注册信息

        [NotNull] private List<long> _m_lAudioClipIdList = new List<long>();//音效实例id列表
        private long _m_lRunFootstepsAudioClipId = AudioObject.g_iInvalidAudioObjectID;//跑步时的脚步音效实例id
        
        public DragBoxGamePlayerUnit([NotNull] DragBoxGameLogic _gameLogic, [NotNull] DragBoxGameController _gameController, GTDMonoDragBoxGamePlayerCuteActor _playerCuteActorMono) : base(_gameLogic, _gameController)
        {
            _m_playerCuteActorMono = _playerCuteActorMono;
        }

        public override void init()
        {
            if (_m_playerCuteActorMono == null)
            {
                Debug.LogError("[DragBoxGamePlayerUnit init] _m_playerCuteActorMono is null");
                return;
            }
            
            _initPlayerCuteActor();
            _initPlayerFollower();
            
            _m_lRunFootstepsAudioClipId = AudioObject.g_iInvalidAudioObjectID;
        }

        public override void discard()
        {
            _stopAllAudio();

            _discardPlayerCuteActor();
            _discardPlayerFollower();
            
            _m_playerCuteActorMono = null;
        }
        
        /// <summary>
        /// 初始化玩家
        /// </summary>
        private void _initPlayerCuteActor()
        {
            if (_m_playerCuteActorMono == null)
            {
                Debug.LogError("[DragBoxGamePlayerUnit _initPlayerCuteActor] _m_playerCuteActorMono is null");
                return;
            }

            if (_m_playerCuteActor != null)
            {
                _discardPlayerCuteActor();
            }
            
            long loadSerializeId = _m_gameLogic.gameSerializeId;
            
            GTDPlayerCuteActor.createPlayerCuteActor(NPPlayer.instance.playerInfo.curCuteActorShowInfo, EPlayerCuteActorTagType.COMMON, _m_playerCuteActorMono.playerCuteActorParent,
                (_playerCuteActor) =>
                {
                    if (_playerCuteActor == null)
                        return;

                    if (loadSerializeId != _m_gameLogic.gameSerializeId || _m_playerCuteActorMono == null)
                    {
                        _playerCuteActor.discard();
                        return;
                    }
                    
                    _m_playerCuteActor = _playerCuteActor;
                    _m_playerCuteActor.setPosition(Vector3.zero, false);
                    _m_playerCuteActor.setRotation(Quaternion.identity, false);
                    _m_playerCuteActor.setScale(Vector3.one);
                    
                    _m_playerCuteActor.onRunFootsteps += _onRunFootsteps;
                    
                    // 注册动画控制
                    _m_playerAnimatorControlRegInfo.controlName = _m_playerCuteActorMono.playerAnimatorControlRegName;
                    _m_playerAnimatorControlRegInfo.animator?.Clear();
                    if (_m_playerAnimatorControlRegInfo.animator == null)
                        _m_playerAnimatorControlRegInfo.animator = new List<Animator>();
                    if (_m_playerCuteActor.actorAnimator != null)
                    {
                        _m_playerAnimatorControlRegInfo.animator.Add(_m_playerCuteActor.actorAnimator);
                    }
                    else if(_m_playerCuteActor.gameObject != null)
                    {
                        _m_playerCuteActor.gameObject.GetComponents<Animator>(_m_playerAnimatorControlRegInfo.animator);
                    }
                    
                    AnimatorControllerMgr.instance.regMono(_m_playerAnimatorControlRegInfo);
                });
        }
        
        /// <summary>
        /// 销毁玩家形象
        /// </summary>
        private void _discardPlayerCuteActor()
        {
            AnimatorControllerMgr.instance.unregMono(_m_playerAnimatorControlRegInfo);
            _m_playerAnimatorControlRegInfo.animator?.Clear();

            if (_m_playerCuteActor != null)
            {
                _m_playerCuteActor.onRunFootsteps -= _onRunFootsteps;
                _m_playerCuteActor.discard();
                _m_playerCuteActor = null;
            }
        }
        
        /// <summary>
        /// 初始化玩家跟随物体
        /// </summary>
        private void _initPlayerFollower()
        {
            if (_m_playerCuteActorMono == null)
            {
                Debug.LogError("[DragBoxGamePlayerUnit _initPlayerFollower] _m_playerCuteActorMono is null");
                return;
            }
            
            if (_m_playerFollowerGo != null)
            {
                _discardPlayerFollower();
            }

            NPGGoIndex goIndex = _m_playerFollowerGoIndex = _m_playerCuteActorMono.playerFollowerGo;
            long loadSerializeId = _m_gameLogic.gameSerializeId;
            GGoIndexCacheMgr.instance.popItem(goIndex, (_go) =>
            {
                if (_go == null)
                {
                    Debug.LogError($"[DragBoxGamePlayerUnit _initPlayerFollower] load :{goIndex} fail 加载出的_go == null");
                    return;
                }

                if (loadSerializeId != _m_gameLogic.gameSerializeId || _m_playerCuteActorMono == null)
                {
                    GGoIndexCacheMgr.instance.pushbackItem(goIndex, _go);
                    return;
                }

                _m_playerFollowerGo = _go;
                _m_playerFollowerGo.transform.SetParent(_m_playerCuteActorMono.playerFollowerGoParent);
                _m_playerFollowerGo.transform.localPosition = Vector3.zero;
                _m_playerFollowerGo.transform.localRotation = Quaternion.identity;
                _m_playerFollowerGo.transform.localScale = Vector3.one;
                
                // 注册动画控制
                _m_playerFollowerAnimatorControlRegInfo.controlName = _m_playerCuteActorMono.playerFollowerAnimatorControlRegName;
                _m_playerFollowerAnimatorControlRegInfo.animator?.Clear();
                if (_m_playerFollowerAnimatorControlRegInfo.animator == null)
                    _m_playerFollowerAnimatorControlRegInfo.animator = new List<Animator>();
                // GTDMonoChapterGameFollowPlayerUnit followPlayerUnit = _m_playerFollowerGo.GetComponent<GTDMonoChapterGameFollowPlayerUnit>();
                // if (followPlayerUnit != null)
                // {
                //     _m_playerFollowerAnimatorControlRegInfo.animator.Add(followPlayerUnit.animator);
                // }
                // else
                // {
                //     _m_playerFollowerGo.GetComponents<Animator>(_m_playerFollowerAnimatorControlRegInfo.animator);
                // }
                
                AnimatorControllerMgr.instance.regMono(_m_playerFollowerAnimatorControlRegInfo);
            });
        }
        
        private void _discardPlayerFollower()
        {
            AnimatorControllerMgr.instance.unregMono(_m_playerFollowerAnimatorControlRegInfo);
            _m_playerFollowerAnimatorControlRegInfo.animator?.Clear();
            
            if (_m_playerFollowerGo != null)
            {
                GGoIndexCacheMgr.instance.pushbackItem(_m_playerFollowerGoIndex, _m_playerFollowerGo);
                _m_playerFollowerGoIndex = null;
                _m_playerFollowerGo = null;
            }
        }
        
        private void _onRunFootsteps()
        {
            _playRunFootstepsAudio();
        }
        
        #region 音效

        /// <summary>
        /// 停止所有音效
        /// </summary>
        private void _stopAllAudio()
        {
            foreach (long audioClipId in _m_lAudioClipIdList)
            {
                PlayAudioMgr.instance.stopClip(audioClipId);
            }
            _m_lAudioClipIdList.Clear();
            _m_lRunFootstepsAudioClipId = AudioObject.g_iInvalidAudioObjectID;
        }

        private void _stopAudio(long _audioClipId)
        {
            _m_lAudioClipIdList.Remove(_audioClipId);
            PlayAudioMgr.instance.stopClip(_audioClipId);
        }
        
        /// <summary>
        /// 播放跑步时的脚步音效
        /// </summary>
        private void _playRunFootstepsAudio()
        {
            if (_m_playerCuteActorMono == null || _m_playerCuteActorMono.runFootstepsAudioIdList == null || _m_playerCuteActorMono.runFootstepsAudioIdList.Count <= 0)
                return;

            _stopAudio(_m_lRunFootstepsAudioClipId);
            
            long runFootstepsAudioId = _m_playerCuteActorMono.runFootstepsAudioIdList.GetRandomItem();
            if(runFootstepsAudioId <= 0)
                return;
            
            _m_lRunFootstepsAudioClipId = PlayAudioMgr.instance.playClip(runFootstepsAudioId);
            _m_lAudioClipIdList.Add(_m_lRunFootstepsAudioClipId);
        }

        #endregion
    }
}