using System;
using ALPackage;
using DG.Tweening;
using GOE;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hotfix
{
    /// <summary>
    /// 游戏棋子
    /// </summary>
    public class GGUIWndTileMatchChecker : _AHotfixBaseSubWnd<GGUIMonoTileMatchChecker>
    {
        private string _m_sWndName;
        
        private TileMatchBlockShowRefObj _m_rBlockShowRefObj;
        private TileMatchBlockRefObj _m_rBlockRefObj; 

        private GGUIWndTileMatchGamePlay _m_gameShow;
        
        private Vector2Int _m_checkerLogicPos;//棋子的逻辑坐标

        private long _m_lShowSerialize;
        private TweenContainer _m_tweenContainer;

        #region 表现子窗口

        // 普通格子子窗口
        private GGUIWndTileMatchCheckerSubNormal _m_wNormalChecker;
        // 炸弹格子子窗口
        private GGUIWndTileMatchCheckerSubBoom _m_wBoomChecker;
        // 火箭格子子窗口
        private GGUIWndTileMatchCheckerSubRocket _m_wRocketChecker;
        // 彩虹格子子窗口
        private GGUIWndTileMatchCheckerSubRainbow _m_wRainbowChecker;

        /// <summary>
        /// 当前展示的是哪个子窗口
        /// </summary>
        private _IGGUIWndTileMatchCheckerSub _m_nowCheckerSub;
        #endregion
        
        public GGUIWndTileMatchChecker(GGUIHotfixCommonMono _wnd, TileMatchBlockShowRefObj _blockShowRefObj) : base(_wnd)
        {
            _m_rBlockShowRefObj = _blockShowRefObj;
            _m_rBlockRefObj = HotfixRefdataCoreMgr.instance.tileMatchBlockRefCore.getRef(_m_rBlockShowRefObj?.block_id ?? 0);
            
            initWnd();
        }

        public TileMatchBlockShowRefObj blockShowRefObj { get { return _m_rBlockShowRefObj; } }
        public TileMatchBlockRefObj blockRefObj { get { return _m_rBlockRefObj; } }
        public Vector2Int checkerLogicPos { get { return _m_checkerLogicPos; } }
        
        public event Action<GGUIWndTileMatchChecker> onPress;
        public event Action<GGUIWndTileMatchChecker, ETileMatchDirection> onTriggerDragMove;
        
        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null || wnd == null)
                return;

            _m_sWndName = wnd.name;
            
            ALUGUICommon.combineBtnPress(hotfixWnd.btnOp, _onPress);
            ALUGUICommon.combineBeginDrag(hotfixWnd.btnOp, _onBeginDrag);
            ALUGUICommon.combineDrag(hotfixWnd.btnOp, _onDrag);
            
            _m_tweenContainer = new TweenContainer();

            if (_m_rBlockRefObj != null && hotfixWnd.showSubWnd != null)
            {
                switch (_m_rBlockRefObj.type)
                {
                    case TileMatchEnum.ETileMatch_BlockType.NONE:
                        // _m_wNormalChecker = new GGUIWndTileMatchCheckerSubNormal(_m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        // _m_nowCheckerSub = _m_wNormalChecker;
                        _m_nowCheckerSub = new GGUIWndTileMatchCheckerSubNormal(this, _m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        break;
                    
                    case TileMatchEnum.ETileMatch_BlockType.BOOM:
                        // _m_wBoomChecker = new GGUIWndTileMatchCheckerSubBoom(_m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        // _m_nowCheckerSub = _m_wBoomChecker;
                        _m_nowCheckerSub = new GGUIWndTileMatchCheckerSubBoom(this, _m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        break;
                    
                    case TileMatchEnum.ETileMatch_BlockType.ROCKET:
                        // _m_wRocketChecker = new GGUIWndTileMatchCheckerSubRocket(_m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        // _m_nowCheckerSub = _m_wRocketChecker;
                        _m_nowCheckerSub = new GGUIWndTileMatchCheckerSubRocket(this, _m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        break;
                    
                    case TileMatchEnum.ETileMatch_BlockType.RAINBOW:
                        // _m_wRainbowChecker = new GGUIWndTileMatchCheckerSubRainbow(_m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        // _m_nowCheckerSub = _m_wRainbowChecker;
                        _m_nowCheckerSub = new GGUIWndTileMatchCheckerSubRainbow(this, _m_rBlockRefObj, _m_rBlockShowRefObj, hotfixWnd.showSubWnd);
                        break;
                }
            }
        }
        
        protected override void _onDiscard()
        {
            onPress = null;
            onTriggerDragMove = null;

            _m_tweenContainer?.killAllDoTween();
            _m_tweenContainer = null;
            
            if (hotfixWnd != null)
            {
                ALUGUICommon.uncombineBtnPress(hotfixWnd.btnOp, _onPress);
                ALUGUICommon.uncombineBeginDrag(hotfixWnd.btnOp, _onBeginDrag);
                ALUGUICommon.uncombineDrag(hotfixWnd.btnOp, _onDrag);
            }

            _m_nowCheckerSub?.getGUIHotfixBasicSimpleSubWnd?.discard();
            _m_nowCheckerSub = null;
            
            // _m_wNormalChecker?.discard();
            // _m_wNormalChecker = null;
            // _m_wBoomChecker?.discard();
            // _m_wBoomChecker = null;
            // _m_wRocketChecker?.discard();
            // _m_wRocketChecker = null;
            // _m_wRainbowChecker?.discard();
            // _m_wRainbowChecker = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            _m_nowCheckerSub?.getGUIHotfixBasicSimpleSubWnd?.showWnd();

            // _m_wNormalChecker?.showWnd();
            // _m_wBoomChecker?.showWnd();
            // _m_wRocketChecker?.showWnd();
            // _m_wRainbowChecker?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            _m_tweenContainer?.killAllDoTween();
            
            _m_nowCheckerSub?.getGUIHotfixBasicSimpleSubWnd?.hideWnd();

            // _m_wNormalChecker?.hideWnd();
            // _m_wBoomChecker?.hideWnd();
            // _m_wRocketChecker?.hideWnd();
            // _m_wRainbowChecker?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_checkerLogicPos = TileMatchUtil.inVaildLogicPos;
            
            _m_nowCheckerSub?.getGUIHotfixBasicSimpleSubWnd?.resetWnd();

            // _m_wNormalChecker?.resetWnd();
            // _m_wBoomChecker?.resetWnd();
            // _m_wRocketChecker?.resetWnd();
            // _m_wRainbowChecker?.resetWnd();
        }

        public void setGameShow(GGUIWndTileMatchGamePlay _gameShow)
        {
            _m_gameShow = _gameShow;
            
            _m_nowCheckerSub?.setGameShow(_gameShow);
        }
        
        /// <summary>
        /// 设置棋子逻辑坐标
        /// </summary>
        /// <param name="_logicPos"></param>
        public void setCheckerLogicPos(Vector2Int _logicPos)
        {
            _m_checkerLogicPos = _logicPos;

#if UNITY_EDITOR
            if (wnd != null)
                wnd.name = _m_sWndName + $"_{_logicPos}_{TileMatchUtil.vector2ToIndex(_logicPos)}";
#endif
        }
        
        /// <summary>
        /// 设置棋子的UI局部坐标
        /// </summary>
        /// <param name="_pos"></param>
        public void setUILocalPos(Vector2 _pos)
        {
            if(wnd == null || wnd.transform == null)
                return;
            wnd.transform.localPosition = _pos;
        }

        /// <summary>
        /// 交换移动到某位置
        /// </summary>
        /// <param name="_targetUiPos"></param>
        /// <param name="_onComplete"></param>
        public void exChangeMoveTo(Vector2 _targetUiPos, Action _onComplete)
        {
            if (wnd == null || wnd.transform == null || hotfixWnd == null || _m_tweenContainer == null)
            {
                _onComplete?.Invoke();
                return;
            }

            _m_tweenContainer.killAllDoTween();

            long showSerialize = _m_lShowSerialize;
            
            _m_tweenContainer.regDoTween
            (wnd.transform.DOLocalMove(_targetUiPos, hotfixWnd.exChangeMoveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                if(_m_lShowSerialize != showSerialize)
                    return;
                
                _onComplete?.Invoke();
            }));
        }

        /// <summary>
        /// 下落到某位置
        /// </summary>
        public void fallTo(Vector2 _startUiPos, Vector2 _targetUiPos, Action _onComplete)
        {
            if (wnd == null || wnd.transform == null || hotfixWnd == null || _m_tweenContainer == null)
            {
                _onComplete?.Invoke();
                return;
            }
            
            setUILocalPos(_startUiPos);

            _m_tweenContainer.killAllDoTween();
            long showSerialize = _m_lShowSerialize;
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (_m_lShowSerialize != showSerialize)
                    return;
                
                _onComplete?.Invoke();
            });
            
            _m_tweenContainer.regDoTween
            (wnd.transform.DOLocalMove(_targetUiPos, hotfixWnd.fallTime).SetEase(Ease.OutQuad).OnComplete(
                () =>
                {
                    if(_m_lShowSerialize != showSerialize)
                        return;
                    
                    setUILocalPos(_targetUiPos);
                    stepCounter.addDoneStepCount();
                }));
            
            if (_m_nowCheckerSub != null)
                _m_nowCheckerSub.playFallAnimation(stepCounter.addDoneStepCount);
            else
                stepCounter.addDoneStepCount();
        }
        
        /// <summary>
        /// 下落到某位置
        /// </summary>
        public void fallTo(Vector2 _targetUiPos, Action _onComplete)
        {
            if (wnd == null || wnd.transform == null || hotfixWnd == null || _m_tweenContainer == null)
            {
                _onComplete?.Invoke();
                return;
            }
            
            _m_tweenContainer.killAllDoTween();

            long showSerialize = _m_lShowSerialize;
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (_m_lShowSerialize != showSerialize)
                    return;
                
                _onComplete?.Invoke();
            });
            
            _m_tweenContainer.regDoTween
            (wnd.transform.DOLocalMove(_targetUiPos, hotfixWnd.fallTime).SetEase(Ease.OutQuad).OnComplete(
                () =>
                {
                    if (_m_lShowSerialize != showSerialize)
                        return;
                    
                    setUILocalPos(_targetUiPos);
                    stepCounter.addDoneStepCount();
                }));

            if (_m_nowCheckerSub != null)
                _m_nowCheckerSub.playFallAnimation(stepCounter.addDoneStepCount);
            else
                stepCounter.addDoneStepCount();
        }

        #region 子窗口表现

        public T getCheckerSubWnd<T>() where T : _IGGUIWndTileMatchCheckerSub
        {
            if (_m_nowCheckerSub is T subWnd)
                return subWnd;

            return default;
        }
        
        /// <summary>
        /// 播放生成格子动画
        /// </summary>
        /// <param name="_complete"></param>
        public void playCreateAnimation(Action _complete)
        {
            if (_m_rBlockRefObj == null || _m_nowCheckerSub == null)
            {
                _complete?.Invoke();
                return;
            }

            _m_nowCheckerSub.playCreateAnimation(_complete);
        }
        
        /// <summary>
        /// 播放被消除动画
        /// </summary>
        /// <param name="_complete"></param>
        public void playByClearAnimation(Action _complete)
        {
            if (_m_rBlockRefObj == null || _m_nowCheckerSub == null)
            {
                _complete?.Invoke();
                return;
            }

            _m_nowCheckerSub.playByClearAnimation(_complete);
        }
        
        /// <summary>
        /// 播放idle动画
        /// </summary>
        public void playIdleAnimation()
        {
            if (_m_rBlockRefObj == null || _m_nowCheckerSub == null)
                return;

            _m_nowCheckerSub.playIdleAnimation();
        }

        /// <summary>
        /// 主动联合普通格子触发表现
        /// </summary>
        /// <param name="_complete"></param>
        public void proactiveUniteNormalTriggerShow(Action _complete)
        {
            if (_m_nowCheckerSub == null)
            {
                _complete?.Invoke();
                return;
            }
            
            _m_nowCheckerSub.proactiveUniteNormalTriggerShow(_complete);
        }

        /// <summary>
        /// 联合炸弹格子触发表现
        /// </summary>
        /// <param name="_complete"></param>
        public void proactiveUniteBoomTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            if (_m_nowCheckerSub == null)
            {
                _complete?.Invoke();
                return;
            }
            
            _m_nowCheckerSub.proactiveUniteBoomTriggerShow(_uniteBlockLogicPos, _complete);
        }

        /// <summary>
        /// 主动联合火箭格子触发表现
        /// </summary>
        /// <param name="_complete"></param>
        public void proactiveUniteRocketTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            if (_m_nowCheckerSub == null)
            {
                _complete?.Invoke();
                return;
            }
            
            _m_nowCheckerSub.proactiveUniteRocketTriggerShow(_uniteBlockLogicPos, _complete);
        }

        #endregion
        
        #region UI事件监听

        /// <summary>
        /// 鼠标点下事件
        /// </summary>
        /// <param name="_isPress"></param>
        /// <param name="_data"></param>
        private void _onPress(bool _isPress, PointerEventData _data)
        {
            if (_isPress)
                onPress?.Invoke(this);
        }

        private Vector3 _m_beginDragPosition;//开始拖动距离
        private bool _m_bIsDragTriggerMove = false;//拖动是否触发过了移动
        /// <summary>
        /// 开始拖动
        /// </summary>
        private void _onBeginDrag(PointerEventData _eventData)
        {
            if(_eventData == null)
                return;

            _m_beginDragPosition = _eventData.position;
            _m_bIsDragTriggerMove = false;
        }

        private void _onDrag(PointerEventData _eventData)
        {
            if(_eventData == null)
                return;

            if (!_m_bIsDragTriggerMove)
            {
                var dragPurpose = _getDragPurPose(_m_beginDragPosition, _eventData.position);
                if (dragPurpose != ETileMatchDirection.None)
                {
                    _m_bIsDragTriggerMove = true;
                    onTriggerDragMove?.Invoke(this, dragPurpose);
                }
            }
        }
        
        //获取拖拽方向
        private ETileMatchDirection _getDragPurPose(Vector3 _beginPos, Vector3 _dragPos)
        {
            if (hotfixWnd == null)
                return ETileMatchDirection.None;
            
            //  Debug.LogError($"_beg:{_beginPos} => _end{_dragPos}");
            float offset_x = _dragPos.x - _beginPos.x;
            float offset_y = _dragPos.y - _beginPos.y;

            if ((Mathf.Abs(offset_x) > Mathf.Abs(offset_y)) && Mathf.Abs(offset_x) > hotfixWnd.triggerMoveDragDis)
            {
                if (offset_x > 0)
                    return ETileMatchDirection.Right;
                else if (offset_x < 0)
                    return ETileMatchDirection.Left;
            }
            else if ((Mathf.Abs(offset_x) < Mathf.Abs(offset_y))&& Mathf.Abs(offset_y) > hotfixWnd.triggerMoveDragDis)
            {
                if (offset_y > 0)
                    return ETileMatchDirection.Up;
                else if (offset_y < 0)
                    return ETileMatchDirection.Down;
            }
            return ETileMatchDirection.None;
        }
        
        #endregion
    }

    /// <summary>
    /// 三消棋子窗口cache
    /// </summary>
    public class GGUIWndTileMatchCheckerCache : _AHotfixCacheController<GGUIWndTileMatchChecker, GGUIHotfixCommonMono>
    {
        private TileMatchBlockShowRefObj _m_rBlockShowRefObj;
        private Transform _m_cacheRoot;

        public GGUIWndTileMatchCheckerCache(TileMatchBlockShowRefObj _blockShowRefObj, Transform _cacheRoot, int _minCount = 1, int _maxCount = 80) : base(_minCount, _maxCount)
        {
            _m_rBlockShowRefObj = _blockShowRefObj;
            _m_cacheRoot = _cacheRoot;
        }

        protected override string _warningTxt { get { return "GGUIWndTileMatchCheckerCache"; } }

        protected override GGUIWndTileMatchChecker _createItem(GGUIHotfixCommonMono _template)
        {
            if (_template == null)
                return null;
            
            // 这里不使用在调用Instantiate时就直接设置父节点的方式，因为这样 GGUIHotfixCommonMono调用Awake会受父节点_m_cacheRoot是否显示的影响
            // 若在调用Instantiate时父节点_m_cacheRoot不处于Active状态, 那么就算GGUIHotfixCommonMono实例化完成后也不会调用自己的Awake方法, 会导致在GGUIHotfixCommonMono上获取MonoSkin时获取不到正确的值
            // GGUIHotfixCommonMono mono = UnityEngine.Object.Instantiate(_template, _m_cacheRoot, false); 
            GGUIHotfixCommonMono mono = UnityEngine.Object.Instantiate(_template);
            if (mono == null || mono.transform == null)
                return null;

            mono.transform.SetParent(_m_cacheRoot);
            mono.transform.localPosition = Vector3.zero;
            
            ALUGUICommon.setGameObjEnable(mono, true);//确保mono在进行GGUIWndTileMatchChecker初始化前被激活, 否则会获取不到GGUIHotfixCommonMono上的MonoSkin
            GGUIWndTileMatchChecker itemWnd = new GGUIWndTileMatchChecker(mono, _m_rBlockShowRefObj);
            ALUGUICommon.setGameObjEnable(mono, false);//初始化完成后再取消激活
            
            return itemWnd;
        }

        protected override void _discardItem(GGUIWndTileMatchChecker _item)
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
        
        protected override void _resetItem(GGUIWndTileMatchChecker _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            _item.resetWnd();
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
        }
    }

    /// <summary>
    /// 三消棋子cache管理器
    /// </summary>
    public class GGUIWndTileMatchCheckerCacheMgr : _ATHotfixCacheControllerMgr<GGUIWndTileMatchChecker, GGUIHotfixCommonMono, TileMatchBlockShowRefObj, GGUIWndTileMatchCheckerCache>
    {
        //最小缓存数
        private readonly int _m_iMinCacheCount;
        //最大缓存数
        private readonly int _m_iMaxCacheCount;
        
        public GGUIWndTileMatchCheckerCacheMgr(GameObject _cacheParent, int _miniCacheCount, int _maxCacheCount) : base(_cacheParent)
        {
            _m_iMinCacheCount = _miniCacheCount;
            _m_iMaxCacheCount = _maxCacheCount;
        }

        protected override void LoadResource(TileMatchBlockShowRefObj _blockShowRefObj, Action<GGUIHotfixCommonMono> _onLoaded)
        {
            if (_blockShowRefObj == null || _blockShowRefObj.prefab_asset_path == null || !_blockShowRefObj.prefab_asset_path.enable)
            {
                _onLoaded?.Invoke(null);
                return;
            }
            HotfixAssetLoader<GameObject> assetLoader = new HotfixAssetLoader<GameObject>(GameResCore.instance, _blockShowRefObj.prefab_asset_path.asset_path, _blockShowRefObj.prefab_asset_path.obj_name);
            assetLoader.loadAsset((itemAsset) =>
            {
                if (null == itemAsset)
                {
                    if (null != _onLoaded)
                        _onLoaded(null);
                    return;
                }
                GGUIHotfixCommonMono itemMono = itemAsset.GetComponent<GGUIHotfixCommonMono>();
                if (null != _onLoaded)
                    _onLoaded(itemMono);
            });
        }

        protected override GGUIWndTileMatchCheckerCache CreateCache(TileMatchBlockShowRefObj _blockShowRefObj)
        {
            return new GGUIWndTileMatchCheckerCache(_blockShowRefObj, _m_RootGo.transform, _m_iMinCacheCount, _m_iMaxCacheCount);
        }

        /// <summary>
        /// 以同步的方式获取一个GGUIWndTileMatchChecker
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public GGUIWndTileMatchChecker popItem(TileMatchBlockShowRefObj _blockShowRefObj)
        {
            if (_blockShowRefObj == null || _blockShowRefObj.prefab_asset_path == null || !_blockShowRefObj.prefab_asset_path.enable)
                return null;

            if (_m_CacheMap.TryGetValue(_blockShowRefObj, out GGUIWndTileMatchCheckerCache _cache) && _cache != null)
                return _cache.popItem();

            return null;
        }
    }
}