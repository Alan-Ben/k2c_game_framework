using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    //火星探索3d主场景
    public class MainAdditionMarsExploreTDScene : _ABasicAdditionMainTDScene
    {
        [NotNull] public static MainAdditionMarsExploreTDScene instance { get { return _g_instance ??= new MainAdditionMarsExploreTDScene(); } }
        private static MainAdditionMarsExploreTDScene _g_instance;
        
        
        private GTDMonoMarsExploreScene _m_sceneMono;
        

        public MainAdditionMarsExploreTDScene()
        {
        }
        

        public override bool needDiscardOnSwitch { get { return true; } }
        public override SceneInfoRefObj sceneRefObj
        {
            get
            {
                long sceneId = NPPlayer.instance.marsComp.exploreSubComponent.levelRef.map_scene_id;
                SceneInfoRefObj sceneInfoRefObj = GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(sceneId);
                return sceneInfoRefObj;
            }
        }
        

        public bool checkNeedReload()
        {
            if (!isInited)
                return false;

            NPGSceneIndex needToLoadIndex = sceneRefObj?.td_scene_idx;
            BasicResIndexInfo curIndex = sceneIndex;
            if (needToLoadIndex == null || curIndex == null)
                return false;

            bool result = needToLoadIndex.mainId != curIndex.mainId || needToLoadIndex.subId != curIndex.subId;
            return result;
        }
        
        protected override void _onSceneInited()
        {
        }
        protected override void _onQuitTDScene()
        {
        }
        public override void onSwitchHideScene()
        {
        }
        protected override void _onRootGOLoaded(GameObject _go)
        {
            if (_m_sceneMono != null || _go == null)
                return;

            _m_sceneMono = _go.GetComponent<GTDMonoMarsExploreScene>();
        }
        protected override _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj)
        {
            return new CommonInputDefaultDealer(_sceneRefObj);
        }
        protected override void _dealShowSceneNP(Action _delegate)
        {
            CameraController.instance.refreshByCameraSetting(sceneRefObj.camera_setting, sceneRefObj.corner_pos);
            _delegate?.Invoke();
        }
        protected override void _dealHideSceneNP(Action _delegate)
        {
            _delegate?.Invoke();
        }

        public void createUnit<T>(NPGGoIndex _resIndex, Vector3 _position, Action<T> _complete)
            where T : MonoBehaviour
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error($"{_resIndex} load failed, scene mono is null.");
                _complete?.Invoke(null);
                return;
            }

            GGoIndexCacheMgr.instance.popItem(_resIndex, _go =>
            {
                if (_go == null)
                {
                    ALLog.Error($"{_resIndex} load failed, cannot find the resource.");
                    _complete?.Invoke(null);
                    return;
                }

                T mono = _go.GetComponent<T>();
                if (mono == null || _m_sceneMono == null)
                {
                    GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _go);
                    ALLog.Error($"{_resIndex} load failed, the resource doesn't have the component {typeof(T).Name}.");
                    _complete?.Invoke(null);
                    return;
                }

                Transform monoTrans = mono.transform;
                monoTrans.SetParent(_m_sceneMono.unitRoot);
                monoTrans.position = _position;
                _complete?.Invoke(mono);
            });
        }
        public void discardUnit<T>(NPGGoIndex _resIndex, T _mono)
            where T : MonoBehaviour
        {
            if (_mono == null)
                return;

            GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _mono.gameObject);
        }

        public GTDMonoMarsExploreHomeBase getHomeBase()
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getHomeBase failed, scene mono is null.");
                return null;
            }

            if (_m_sceneMono.monoHomeBase == null)
            {
                ALLog.Error("getHomeBase failed, monoHomeBase is null.");
                return null;
            }

            ALUGUICommon.setGameObjEnable(_m_sceneMono.monoHomeBase, true);
            return _m_sceneMono.monoHomeBase;
        }

        public GTDMonoMarsExploreEventPos getEventPos(long _posId)
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("getEventPos failed, scene mono is null.");
                return null;
            }

            if (_m_sceneMono.eventPosList == null)
            {
                ALLog.Error("getEventPos failed, eventPosList is null.");
                return null;
            }

            foreach (GTDMonoMarsExploreEventPos eventPos in _m_sceneMono.eventPosList)
            {
                if (eventPos != null && eventPos.posId == _posId)
                    return eventPos;
            }

            ALLog.Error($"getEventPos failed, eventPos with posId {_posId} not found.");
            return null;
        }
        
        public void showEventLoadedEffect(Vector3 _position)
        {
            if (_m_sceneMono == null)
            {
                ALLog.Error("showEventLoadedEffect failed, scene mono is null.");
                return;
            }
            
            PlaySfxMgr.instance.playSfxByPos(_m_sceneMono.eventShowSfxId, _position);
        }
    }
}