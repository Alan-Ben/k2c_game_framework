using System;
using ALPackage;
using DG.Tweening;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 客人窗口，管理单个客人的状态表现和移动
    /// </summary>
    public class GGUIWndTileMatchGuest : _AHotfixBaseSubWnd<GGUIMonoTileMatchGuest>
    {
        private NPGGoIndex _m_iResGoIndex;
        
        /// <summary>
        /// 移动动画容器
        /// </summary>
        private TweenContainer _m_tweenContainer;
        
        private int _m_iShowSerialId;

        public GGUIWndTileMatchGuest(NPGGoIndex _goIndex, GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            _m_iResGoIndex = _goIndex;
            initWnd();
        }

        public NPGGoIndex resGoIndex { get { return _m_iResGoIndex; } }
        
        protected override void _onWndInitDoneHotfix()
        {
            _m_tweenContainer = new TweenContainer();
        }
        
        protected override void _onDiscard()
        {
            _m_tweenContainer?.killAllDoTween();
            _m_tweenContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshSkin();
            
            // 默认进入IDLE状态
            _setState(ETileMatchGuestState.IDLE);
        }

        protected override void _onHideWnd()
        {
            _m_iShowSerialId = ALSerializeOpMgr.next();
            
            _m_tweenContainer?.killAllDoTween();
        }

        protected override void _onReset()
        {
            _m_tweenContainer?.killAllDoTween();
        }

        /// <summary>
        /// 设置客人状态
        /// </summary>
        /// <param name="_state">目标状态</param>
        /// <param name="_onPlayDone">动画播放完成回调</param>
        public void setState(ETileMatchGuestState _state, Action _onPlayDone = null)
        {
            _setState(_state, _onPlayDone);
        }

        public void setPosition(Vector3 _worldPos)
        {
            if (wnd == null || wnd.transform == null)
                return;

            wnd.transform.position = _worldPos;
        }
        
        /// <summary>
        /// 移动客人到目标位置
        /// </summary>
        /// <param name="_targetPos">目标世界坐标</param>
        /// <param name="_moveTime">移动时间</param>
        /// <param name="_onComplete">移动完成回调</param>
        public void moveTo(Vector3 _targetPos, float _moveTime, Action _onComplete = null)
        {
            if (wnd == null || wnd.transform == null)
            {
                _onComplete?.Invoke();
                return;
            }

            _setState(ETileMatchGuestState.WALK);
            _m_tweenContainer?.killAllDoTween();
            _m_tweenContainer?.regDoTween(
                wnd.transform.DOMove(_targetPos, _moveTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        _setState(ETileMatchGuestState.IDLE);
                        _onComplete?.Invoke();
                    })
            );
        }

        /// <summary>
        /// 左右翻转客人（通过X轴缩放翻转）
        /// </summary>
        public void flip()
        {
            if (wnd == null || wnd.transform == null)
                return;

            Vector3 localScale = wnd.transform.localScale;
            localScale.x = -localScale.x;
            wnd.transform.localScale = localScale;
        }

        /// <summary>
        /// 进行成功表现
        /// </summary>
        public void showSucc(Action _onComplete = null)
        {
            int showSerialId = _m_iShowSerialId;
            _setState(ETileMatchGuestState.SUCCESS, () =>
            {
                if(showSerialId != _m_iShowSerialId)
                    return;
                
                _onComplete?.Invoke();
            });
        }

        /// <summary>
        /// 进行失败表现
        /// </summary>
        /// <param name="_onComplete"></param>
        public void showFail(Action _onComplete = null)
        {
            int showSerialId = _m_iShowSerialId;
            _setState(ETileMatchGuestState.FAIL, () =>
            {
                if(showSerialId != _m_iShowSerialId)
                    return;
                
                _onComplete?.Invoke();
            });
        }

        /// <summary>
        /// 内部设置状态
        /// </summary>
        private void _setState(ETileMatchGuestState _state, Action _onPlayDone = null)
        {
            if (hotfixWnd == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            switch (_state)
            {
                case ETileMatchGuestState.None:
                    _onPlayDone?.Invoke();
                    break;
                
                case ETileMatchGuestState.IDLE:
                    if(hotfixWnd.spineAni != null && hotfixWnd.idleShowAniConfig != null)
                        hotfixWnd.idleShowAniConfig.playAnimation(hotfixWnd.spineAni, _onPlayDone);
                    else
                        _onPlayDone?.Invoke();
                    break;
                
                case ETileMatchGuestState.WALK:
                    if(hotfixWnd.spineAni != null && hotfixWnd.walkShowAniConfig != null)
                        hotfixWnd.walkShowAniConfig.playAnimation(hotfixWnd.spineAni, _onPlayDone);
                    else
                        _onPlayDone?.Invoke();
                    break;
                
                case ETileMatchGuestState.SUCCESS:
                    if(hotfixWnd.spineAni != null && hotfixWnd.successShowAniConfig != null)
                        hotfixWnd.successShowAniConfig.playAnimation(hotfixWnd.spineAni, _onPlayDone);
                    else
                        _onPlayDone?.Invoke();
                    break;
                
                case ETileMatchGuestState.FAIL:
                    if(hotfixWnd.spineAni != null && hotfixWnd.failShowAniConfig != null)
                        hotfixWnd.failShowAniConfig.playAnimation(hotfixWnd.spineAni, _onPlayDone);
                    else
                        _onPlayDone?.Invoke();
                    break;
            }
        }

        private void _refreshSkin()
        {
            if(hotfixWnd == null || !isShow || hotfixWnd.spineAni == null)
                return;
            
            string skinName = string.Empty;
            skinName = hotfixWnd.skinNameList.GetRandomItem();
            Spine.Skeleton skeleton = hotfixWnd.spineAni.Skeleton;
            if(string.IsNullOrEmpty(skinName) || skeleton == null || skeleton.Data == null)
                return;
            
            Spine.Skin newSKin = skeleton.Data.FindSkin(skinName);
            if (newSKin != null)
            {
                skeleton.SetSkin(newSKin);
                skeleton.SetSlotsToSetupPose();
                hotfixWnd.spineAni.UpdateMesh();
            }
        }
    }

    /// <summary>
    /// 客人窗口缓存池
    /// </summary>
    public class GGUIWndTileMatchGuestCache : _AHotfixCacheController<GGUIWndTileMatchGuest, GGUIHotfixCommonMono>
    {
        private NPGGoIndex _m_goIndex;
        private GameObject _m_gRootGo;

        public GGUIWndTileMatchGuestCache(NPGGoIndex _index, GameObject _parent, int _minCount = 1, int _maxCount = 10) : base(_minCount, _maxCount)
        {
            _m_goIndex = _index;
            _m_gRootGo = _parent;
        }

        protected override string _warningTxt { get { return $"GGUIWndTileMatchGuestCache _m_goIndex:{_m_goIndex}"; } }

        protected override GGUIWndTileMatchGuest _createItem(GGUIHotfixCommonMono _template)
        {
            if (_template == null || null == _m_gRootGo)
                return null;

            // 不在Instantiate时直接设置父节点，避免父节点未激活时Awake不触发导致MonoSkin获取失败
            GGUIHotfixCommonMono mono = UnityEngine.Object.Instantiate(_template);
            if (mono == null || mono.transform == null)
                return null;

            mono.transform.SetParent(_m_gRootGo.transform);
            mono.transform.localPosition = Vector3.zero;
            mono.transform.localScale = Vector3.one;

            // 确保mono在初始化前被激活，否则获取不到MonoSkin
            ALUGUICommon.setGameObjEnable(mono, true);
            GGUIWndTileMatchGuest guestWnd = new GGUIWndTileMatchGuest(_m_goIndex, mono);
            // initWnd完成后禁用对象，保持缓存状态一致（pool中对象应为disabled）
            ALUGUICommon.setGameObjEnable(mono, false);
            return guestWnd;
        }

        protected override void _discardItem(GGUIWndTileMatchGuest _item)
        {
            GGUIHotfixCommonMono mono = null;
            if (null != _item)
            {
                mono = _item.wnd;
                _item.discard();
            }

            ALUnityCommon.releaseGameObj(mono);
        }

        protected override void _onInit(GGUIHotfixCommonMono _template)
        {
        }

        protected override void _resetItem(GGUIWndTileMatchGuest _item)
        {
            if(null == _item || null == _item.rectTransform || null == _m_gRootGo)
                return;
            
            _item.resetWnd();
            // 先Disable再修改parent，防止reparent到active父节点时对象提前可见
            ALUGUICommon.setGameObjDisable(_item.go);
            //设置为子节点
            _item.rectTransform.SetParent(_m_gRootGo.transform);
            _item.rectTransform.localPosition = Vector3.zero;
        }
    }

    /// <summary>
    /// 客人窗口缓存池管理器，根据NPGGoIndex加载和管理客人预制
    /// </summary>
    public class GGUIWndTileMatchGuestCacheMgr : _AHotfixCacheControllerMgr<GGUIWndTileMatchGuest, GGUIHotfixCommonMono, GGUIWndTileMatchGuestCache>
    {
        public GGUIWndTileMatchGuestCacheMgr(string _rootFolderName) : base(_rootFolderName)
        {
        }

        public GGUIWndTileMatchGuestCacheMgr(GameObject _cacheParent) : base(_cacheParent)
        {
        }

        protected override void LoadResource(BasicResIndexInfo _index, Action<GGUIHotfixCommonMono> _onLoaded)
        {
            NPGGoIndex goIndex = _index as NPGGoIndex;
            if(goIndex == null)
            {
                Debug.LogError($"{_index} is not a NPGGoIndex");
                _onLoaded?.Invoke(null);
                return;
            }

            HotfixGoIndexLoader.loadGameObject(goIndex, (assetGo) =>
            {
                if(null == assetGo)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError($"NPGGoIndex加载错误: mainId={goIndex.mainId} subId={goIndex.subId}");
#endif
                    _onLoaded?.Invoke(null);
                    return;
                }

                GGUIHotfixCommonMono mono = assetGo.GetComponent<GGUIHotfixCommonMono>();
                if (mono == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError($"NPGGoIndex:{goIndex.mainId}-{goIndex.subId} 对象上没有 GGUIHotfixCommonMono 组件");
#endif
                    ALUnityCommon.releaseGameObj(assetGo);
                    _onLoaded?.Invoke(null);
                    return;
                }

                _onLoaded?.Invoke(mono);
            });
        }

        protected override GGUIWndTileMatchGuestCache CreateCache(BasicResIndexInfo _index)
        {
            NPGGoIndex goIndex = _index as NPGGoIndex;
            if(goIndex == null)
            {
                Debug.LogError($"{_index} is not a NPGGoIndex");
                return null;
            }
            
            return new GGUIWndTileMatchGuestCache(goIndex, _m_RootGo);
        }
    }
}