using System;
using ALPackage;
using DG.Tweening;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消任务飞行item
    /// </summary>
    public class GGUIWndTileMatchTaskBlockFlyItem : _AHotfixBaseSubWnd<GGUIMonoTileMatchTaskBlockFlyItem>
    {
        private TileMatchBlockShowRefObj _m_rBlockShowRefObj;//方块展示引用对象
        private int _m_iBelongTaskSerialId;//所属任务序列号
        private Action _m_aFlyCompleteAction;
        
        private GGuiWndSprite _m_wIcon;//图标

        private TweenContainer _m_tweenContainer;

        private long _m_lShowSerialize;

        private float _m_fStartFlyTime;
        
        public GGUIWndTileMatchTaskBlockFlyItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long blockId { get { return _m_rBlockShowRefObj?.block_id ?? 0; } }

        /// <summary>
        /// 所属任务序列号
        /// </summary>
        public int belongTaskSerialId { get { return _m_iBelongTaskSerialId; } }
        
        /// <summary>
        /// 剩余飞行时间
        /// </summary>
        public float leftFlyTime {
            get
            {
                if (hotfixWnd == null)
                    return 0;

                return hotfixWnd.flyTimeS - Time.realtimeSinceStartup + _m_fStartFlyTime;
            }
        }

        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;
            
            if (hotfixWnd.icon != null)
                _m_wIcon = new GGuiWndSprite(hotfixWnd.icon);

            _m_tweenContainer = new TweenContainer();
        }
        
        protected override void _onDiscard()
        {
            _m_tweenContainer?.killAllDoTween();
            _m_tweenContainer = null;
            
            _m_wIcon?.discard();
            _m_wIcon = null;

            _m_aFlyCompleteAction = null;
            _m_rBlockShowRefObj = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_aFlyCompleteAction = null;
            _m_rBlockShowRefObj = null;
            
            _m_tweenContainer?.killAllDoTween();

            _m_wIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_tweenContainer?.killAllDoTween();

            _m_wIcon?.discardTexture();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void setData(TileMatchBlockShowRefObj _blockShowRefObj, int _belongTaskSerialId, Vector3 _flyStartWorldPos, Vector3 _flyEndWorldPos, Action _flyComplete)
        {
            _m_rBlockShowRefObj = _blockShowRefObj;
            _m_iBelongTaskSerialId = _belongTaskSerialId;
            _m_aFlyCompleteAction = _flyComplete;
            
            if (wnd == null || wnd.transform == null || hotfixWnd == null || _m_tweenContainer == null)
            {
                _flyComplete?.Invoke();
                return;
            }
            
            _m_tweenContainer?.killAllDoTween();
            
            if (_m_wIcon != null && _blockShowRefObj != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_blockShowRefObj.icon);
            }
            wnd.transform.position = _flyStartWorldPos;

            // 播放飞行动画
            _playAnimation(hotfixWnd.aniFlyAnimation, null);
            
            long showSerialize = _m_lShowSerialize;
            _m_fStartFlyTime = Time.realtimeSinceStartup;
            _m_tweenContainer.regDoTween
            (wnd.transform.DOMove(_flyEndWorldPos, hotfixWnd.flyTimeS).SetEase(Ease.OutQuad).OnComplete(
                () =>
                {
                    _onItemFlyComplete(showSerialize);
                }));
        }

        /// <summary>
        /// 切换飞行目标
        /// </summary>
        public void chgFlyTarget(Vector3 _flyEndWorldPos)
        {
            _m_tweenContainer?.killAllDoTween();
            
            if (wnd == null || wnd.transform == null || hotfixWnd == null)
            {
                _onItemFlyComplete(_m_lShowSerialize);
                return;
            }
            
            float leftTime = leftFlyTime;
            if (leftTime <= 0f)
            {
                _onItemFlyComplete(_m_lShowSerialize);
                return;
            }
            
            long showSerialize = _m_lShowSerialize;
            _m_tweenContainer.regDoTween
            (wnd.transform.DOMove(_flyEndWorldPos, leftTime).SetEase(Ease.OutQuad).OnComplete(
                () =>
                {
                    _onItemFlyComplete(showSerialize);
                }));
        }
        
        private void _onItemFlyComplete(long _serialize)
        {
            if(_m_lShowSerialize != _serialize)
                return;
            
            _m_aFlyCompleteAction?.Invoke();
            _m_aFlyCompleteAction = null;
        }
        
        #region 动画

        /// <summary>
        /// 播放动画
        /// </summary>
        private void _playAnimation(string _aniName, Action _playDone)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                _playDone?.Invoke();
                return;
            }

            hotfixWnd.ani.Play(_aniName, _playDone);
        }

        private void _sample(string _aniName, float _normalizedTime)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                return;
            }
            
            hotfixWnd.ani.Sample(_aniName, _normalizedTime);
        }

        #endregion
    }

    public class GGUIWndTileMatchTaskFlyItemCache : _AHotfixCacheController<GGUIWndTileMatchTaskBlockFlyItem, GGUIHotfixCommonMono>
    {
        private Transform _m_cacheRoot;

        public GGUIWndTileMatchTaskFlyItemCache(Transform _cacheRoot, int _minCount, int _maxCount) : base(_minCount, _maxCount)
        {
            _m_cacheRoot = _cacheRoot;
        }

        public GGUIWndTileMatchTaskFlyItemCache(Transform _cacheRoot, int _minCount, int _maxCount, int _addUnit) : base(_minCount, _maxCount, _addUnit)
        {
            _m_cacheRoot = _cacheRoot;
        }

        protected override string _warningTxt { get { return "GGUIWndTileMatchTaskFlyItemCache"; } }

        protected override GGUIWndTileMatchTaskBlockFlyItem _createItem(GGUIHotfixCommonMono _template)
        {
            if (_template == null)
                return null;

            GGUIHotfixCommonMono mono = UnityEngine.Object.Instantiate(_template);
            if (mono == null || mono.transform == null)
                return null;

            mono.transform.SetParent(_m_cacheRoot);
            mono.transform.localPosition = Vector3.zero;
            mono.transform.localScale = Vector3.one;
            
            return new GGUIWndTileMatchTaskBlockFlyItem(mono);
        }

        protected override void _discardItem(GGUIWndTileMatchTaskBlockFlyItem _item)
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
        
        protected override void _resetItem(GGUIWndTileMatchTaskBlockFlyItem _item)
        {
            if (_item == null || _item.wnd == null)
                return;

            _item.resetWnd();
            _item.wnd.transform.SetParent(_m_cacheRoot, false);
        }
    }
}