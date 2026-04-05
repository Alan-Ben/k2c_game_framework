using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class MainAdditionChildTDScene : _ABasicAdditionMainTDScene
    {
        [NotNull] public static MainAdditionChildTDScene instance { get { return _g_instance ??= new MainAdditionChildTDScene(); } }
        private static MainAdditionChildTDScene _g_instance;


        private GTDMonoChildScene _m_sceneMono;
        
        
        public override bool needDiscardOnSwitch { get { return true; } }
        public override SceneInfoRefObj sceneRefObj { get { return GRefdataCoreMgr.instance.sceneInfoRefCore.getRef(GRefdataCoreMgr.instance.npGeneral.child_scene_id); } }


        protected override void _onSceneInited()
        {
        }
        public override void onSwitchHideScene()
        {
        }
        protected override void _onQuitTDScene()
        {
            _m_sceneMono = null;
        }
        protected override void _onRootGOLoaded(GameObject _go)
        {
            if (_m_sceneMono != null || _go == null)
                return;

            _m_sceneMono = _go.GetComponent<GTDMonoChildScene>();
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
        
        
        public void createClassroom(Action<GTDMonoChildClassroom> _complete)
        {
            if (_m_sceneMono == null)
            {
                _complete?.Invoke(null);
                return;
            }
            
            GGoIndexCacheMgr.instance.popItem(GRefdataCoreMgr.instance.npGeneral.child_classroom_res_index, _go =>
            {
                if (_go == null)
                {
                    _complete?.Invoke(null);
                    return;
                }

                GTDMonoChildClassroom mono = _go.GetComponent<GTDMonoChildClassroom>();
                if (mono == null)
                {
                    _complete?.Invoke(null);
                    return;
                }

                Transform monoTrans = mono.transform;
                monoTrans.SetParent(_m_sceneMono.unitParent);
                monoTrans.position = _m_sceneMono.classroomPoint == null ? Vector3.zero : _m_sceneMono.classroomPoint.position;
                _complete?.Invoke(mono);
            });
        }
        public void discardClassroom(GTDMonoChildClassroom _classroom)
        {
            if (_classroom == null)
                return;

            GGoIndexCacheMgr.instance.pushbackItem(GRefdataCoreMgr.instance.npGeneral.child_classroom_res_index, _classroom.gameObject);
        }
        public void discardUnit<T>(NPGGoIndex _resIndex, T _building)
            where T : MonoBehaviour
        {
            if (_building == null)
                return;
            
            GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _building.gameObject);
        }
        
        
        private void _createUnit<T>(NPGGoIndex _resIndex, Action<T> _complete)
            where T : MonoBehaviour
        {
            if (_m_sceneMono == null)
            {
                _complete?.Invoke(null);
                return;
            }
            
            GGoIndexCacheMgr.instance.popItem(_resIndex, _go =>
            {
                if (_go == null)
                {
                    _complete?.Invoke(null);
                    return;
                }

                T mono = _go.GetComponent<T>();
                if (mono == null || _m_sceneMono == null)
                {
                    GGoIndexCacheMgr.instance.pushbackItem(_resIndex, _go);
                    _complete?.Invoke(null);
                    return;
                }

                Transform monoTrans = mono.transform;
                monoTrans.SetParent(_m_sceneMono.unitParent);
                _complete?.Invoke(mono);
            });
        }
    }
}