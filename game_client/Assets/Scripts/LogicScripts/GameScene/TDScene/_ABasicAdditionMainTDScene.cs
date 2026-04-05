using ALPackage;
using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace GOE
{
    public abstract class _ABasicAdditionMainTDScene : _ABasicAdditionTDScene
    {

        private int _m_iOpSerialize;//操作序列号
        [NotNull]private readonly ALStepCounter _m_scShowSceneCounter = new ALStepCounter();//场景配置SO加载记录对象
        
        private bool _m_bIsShowRender = false;//是否显示了场景渲染效果
        private _IGameInputDealer _m_inputDealer;

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override BasicResIndexInfo _getDefaultSceneIndex()
        {
            return sceneRefObj?.td_scene_idx;
        }

        public sealed override void _dealShowScene(Action _delegate)
        {
            //加载场景SO，加载完成之后显示场景渲染效果
            int serialize = _m_iOpSerialize = ALSerializeOpMgr.next();

            //创建步骤统计
            _m_scShowSceneCounter.resetAll();
            _m_scShowSceneCounter.chgTotalStepCount(3);
            _m_scShowSceneCounter.regAllDoneDelegate(_delegate);
            
            // 开启灯光环境配置，这里暂时使用主灯光
            if(sceneRefObj != null)
                LightsMgr.instance.openMainLight(sceneRefObj.dir_lights_so_index, _m_scShowSceneCounter.addDoneStepCount);
            
            //加载数据并进行设置操作
            _loadAllSceneSO(serialize, sceneRefObj, () =>
            {
                //不是本次操作序列号不处理
                if (serialize != _m_iOpSerialize)
                {
                    _m_scShowSceneCounter.addDoneStepCount();
                    return;
                }

                //显示场景渲染效果
                _showRender();
                _m_scShowSceneCounter.addDoneStepCount();
            });

            //设置输入控制
            InputListener.instance.setInputObj(GameInputListener.instance);
            //显示场景
            _dealShowSceneNP(() =>
            {
                //设置输入对象的默认处理对象
                setSceneInputDealer(_getSceneInputerDealer(sceneRefObj));
                CameraController.instance.refreshFaceCameraObjs();
                _m_scShowSceneCounter.addDoneStepCount();
            });
        }

        public sealed override void _dealHideScene(Action _delegate)
        {
            // 由于使用的是主灯光配置，这里暂时不关闭灯光环境配置
            //LightsMgr.instance.closeLight();
            
            _hideRender();
            _dealHideSceneNP(_delegate);

            //序列号增加，防止隐藏Scene后调用显示场景渲染效果方法
            _m_iOpSerialize = ALSerializeOpMgr.next();
            GameInputListener.instance.quitDefaultInputDealer(_m_inputDealer);
            _m_inputDealer = null;
        }
        
        public void setSceneInputDealer(_IGameInputDealer _inputDealer)
        {
            // 如果没有在展示不响应
            if (!isShow)
                return;
            
            GameInputListener.instance.setDefaultInputDealer(_m_inputDealer = _inputDealer);
        }


        #region 场景效果相关

        /// <summary>
        /// 开启渲染效果
        /// </summary>
        protected virtual void _showRender()
        {
            if (_m_bIsShowRender)
                return;

            _m_bIsShowRender = true;
            
            //设置相机的Render索引，用于不同场景配置不同的渲染效果
            if (Game.instance.mainCamera.mainCameraData != null && null != sceneRefObj)
                Game.instance.mainCamera.mainCameraData.SetRenderer(sceneRefObj.urp_render_index);
        }

        /// <summary>
        /// 关闭渲染效果
        /// </summary>
        protected virtual void _hideRender()
        {
            if (!_m_bIsShowRender)
                return;

            _m_bIsShowRender = false;
        }

        /// <summary>
        /// 加载所有场景相关配置SO
        /// </summary>
        /// <param name="_sceneRef"></param>
        /// <param name="_stepCounter"></param>
        /// <param name="_onLoaded"></param>
        protected virtual void _loadAllSceneSO(int _opSerialize, SceneInfoRefObj _sceneRef, Action _onLoaded)
        {
            if (_sceneRef == null)
            {
                _onLoaded?.Invoke();
                return;
            }

            //暂时没有环境so
            _onLoaded?.Invoke();
        }

        /// <summary>
        /// 加载场景相关配置SO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_soIndex"></param>
        /// <param name="_onLoaded"></param>
        protected void _loadSceneSO<T>(BasicResIndexInfo _soIndex, Action<T> _onLoaded) where T : ScriptableObject
        {
            //下标无效，直接返回
            if (_soIndex == null || !_soIndex.isValid())
            {
                _onLoaded?.Invoke(default(T));
                return;
            }

            //加载SO
            _loadSceneSO(_soIndex.assetPath, _soIndex.objName, _onLoaded);
        }

        /// <summary>
        /// 加载场景相关配置SO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_assetPath"></param>
        /// <param name="_objName"></param>
        /// <param name="_onLoaded"></param>
        protected void _loadSceneSO<T>(string _assetPath, string _objName, Action<T> _onLoaded) where T : ScriptableObject
        {
            ALAssetLoader<T> assetLoader = new ALAssetLoader<T>(GameResCore.instance, _assetPath, _objName
#if UNITY_EDITOR
                , ".asset"
                , "t:scriptableobject"
#endif
                );

            assetLoader.loadAsset((_so) =>
            {
                if (_so == null)
                {
                    Debug.LogError($"【_ANPBasicAdditionMainTDScene Error】找不到{typeof(T)}文件，assetPath：{_assetPath}，objName：{_objName}");
                    _onLoaded?.Invoke(_so);
                    return;
                }

                _onLoaded?.Invoke(_so);
            });
        }

        #endregion


        /// <summary> 场景配置信息 </summary>
        public abstract SceneInfoRefObj sceneRefObj { get; }

        /// <summary>
        /// 把相机移动聚焦到目标位置
        /// </summary>
        public virtual void focusToTarget(Vector3 _targetPos, float _duration, Action _complete = null)
        {
            if (sceneRefObj == null)
            {
                _complete?.Invoke();
                return;
            }

            if (!isEntered)
            {
                _complete?.Invoke();
                return;
            }
            
            CameraController.instance.setCameraMoveController(new CameraMoveToFocusPointEaseController(_targetPos, sceneRefObj.move_type, _duration, _complete));
            CameraController.instance.frameCheck();
        }
        /// <summary>
        /// 把相机移动到目标位置
        /// </summary>
        public virtual void moveCameraTo(Vector3 _targetPos, float _duration, Action _complete = null)
        {
            if (sceneRefObj != null)
            {
                switch (sceneRefObj.move_type)
                {
                    case ESceneMoveType.XY:
                        _targetPos.z = CameraController.instance.cameraPos.z;
                        break;
                    case ESceneMoveType.XZ:
                        _targetPos.y = CameraController.instance.cameraPos.y;
                        break;
                    case ESceneMoveType.YZ:
                        _targetPos.x = CameraController.instance.cameraPos.x;
                        break;
                }
            }
            
            CameraController.instance.setCameraMoveController(new CameraMoveEaseController(_targetPos, _duration, _complete));
        }

        /// <summary>
        /// 获取场景输入处理对象
        /// </summary>
        /// <param name="_sceneRefObj"></param>
        /// <returns></returns>
        protected abstract _IGameInputDealer _getSceneInputerDealer(SceneInfoRefObj _sceneRefObj);

        /// <summary>
        /// 显示本scene对象的操作，显示完成则调用回调
        /// </summary>
        /// <param name="_delegate"></param>
        protected abstract void _dealShowSceneNP(Action _delegate);

        /// <summary>
        /// 隐藏本scene对象的操作
        /// </summary>
        /// <param name="_delegate"></param>
        protected abstract void _dealHideSceneNP(Action _delegate);
    }
}
