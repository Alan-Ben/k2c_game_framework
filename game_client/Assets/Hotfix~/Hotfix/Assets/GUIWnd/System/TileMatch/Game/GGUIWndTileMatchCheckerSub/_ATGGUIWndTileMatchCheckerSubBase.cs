using System;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public abstract class _ATGGUIWndTileMatchCheckerSubBase<T> : _AHotfixBaseSimpleSubWnd<T>, _IGGUIWndTileMatchCheckerSub
        where T : GGUIMonoTileMatchCheckerSubBase, new()
    {
        private GGuiWndSprite _m_wBlockIcon;
        
        protected long _m_lShowSerialize;

        protected TileMatchBlockRefObj _m_rBlockRefObj; 
        protected TileMatchBlockShowRefObj _m_rBlockShowRefObj;

        [NotNull] protected GGUIWndTileMatchChecker _m_blockShow;
        protected GGUIWndTileMatchGamePlay _m_gameShow;
        
        protected _ATGGUIWndTileMatchCheckerSubBase([NotNull] GGUIWndTileMatchChecker _blockShow, TileMatchBlockRefObj _blockRefObj, TileMatchBlockShowRefObj _blockShowRefObj, MonoSkin _wnd) : base(_wnd)
        {
            _m_blockShow = _blockShow;
            _m_rBlockRefObj = _blockRefObj;
            _m_rBlockShowRefObj = _blockShowRefObj;
        }

        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            if (hotfixWnd.blockIcon != null)
                _m_wBlockIcon = new GGuiWndSprite(hotfixWnd.blockIcon);
        }

        protected override void _onDiscard()
        {
            _m_wBlockIcon?.discard();
            _m_wBlockIcon = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            if (_m_wBlockIcon != null && _m_rBlockShowRefObj != null)
            {
                _m_wBlockIcon.showWnd();
                _m_wBlockIcon.setTexture(_m_rBlockShowRefObj.icon);
            }
        }

        protected override void _onHideWnd()
        {
            _m_wBlockIcon?.hideWnd();
            
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wBlockIcon?.discardTexture();
        }

        public _AGGUIHotfixBasicSimpleSubWnd getGUIHotfixBasicSimpleSubWnd { get { return this; } }

        public void setGameShow(GGUIWndTileMatchGamePlay _gameShow)
        {
            _m_gameShow = _gameShow;
        }
        
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="_ani"></param>
        /// <param name="_aniName"></param>
        /// <param name="_complete"></param>
        protected void _playAnimation(Animation _ani, string _aniName, Action _complete = null)
        {
            if (_ani == null || string.IsNullOrEmpty(_aniName))
            {
                _complete?.Invoke();
                return;
            }

            long serialize = _m_lShowSerialize;
            _ani.Play(_aniName, null);

            //Alzq，单独将结束回调调快，加快游戏体验节奏
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                _complete?.Invoke();
            }, aniCompleteDelayTimeS);
        }

        protected void _sampleAnimation(Animation _ani, string _aniName, float _normalizedTime = 0f)
        {
            if (_ani == null || string.IsNullOrEmpty(_aniName))
                return;
            
            _ani.Sample(_aniName, _normalizedTime);
        }

        /// <summary>
        /// 播放生成格子动画
        /// </summary>
        /// <param name="_complete"></param>
        public virtual void playCreateAnimation(Action _complete)
        {
            if (hotfixWnd == null)
            {
                _complete?.Invoke();
                return;
            }

            _playAnimation(hotfixWnd.ani, hotfixWnd.createAniName, _complete);
        }
        
        /// <summary>
        /// 播放被消除动画
        /// </summary>
        /// <param name="_complete"></param>
        public virtual void playByClearAnimation(Action _complete)
        {
            if (hotfixWnd == null)
            {
                _complete?.Invoke();
                return;
            }

            _playAnimation(hotfixWnd.ani, hotfixWnd.byClearAniName, _complete);
        }

        public void playFallAnimation(Action _complete)
        {
            if (hotfixWnd == null)
            {
                _complete?.Invoke();
                return;
            }

            _playAnimation(hotfixWnd.ani, hotfixWnd.fallAniName, _complete);
        }

        /// <summary>
        /// 播放idle动画
        /// </summary>
        public virtual void playIdleAnimation()
        {
            if (hotfixWnd == null)
                return;

            _playAnimation(hotfixWnd.ani, hotfixWnd.idleAniName, null);
        }

        /// <summary>
        /// 播放动画后多久延迟直接结束
        /// </summary>
        protected virtual float aniCompleteDelayTimeS { get { return 0.2f; } }

        public abstract void proactiveUniteNormalTriggerShow(Action _complete);

        public abstract void proactiveUniteBoomTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete);

        public abstract void proactiveUniteRocketTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete);
    }
}