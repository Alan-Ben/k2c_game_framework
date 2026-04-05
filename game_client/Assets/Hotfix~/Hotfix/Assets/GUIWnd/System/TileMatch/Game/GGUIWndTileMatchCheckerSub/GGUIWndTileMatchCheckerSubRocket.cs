using System;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 火箭格子子窗口
    /// </summary>
    public class GGUIWndTileMatchCheckerSubRocket : _ATGGUIWndTileMatchCheckerSubBase<GGUIMonoTileMatchCheckerSubRocket>
    {
        public GGUIWndTileMatchCheckerSubRocket([NotNull] GGUIWndTileMatchChecker _blockShow, TileMatchBlockRefObj _blockRefObj, TileMatchBlockShowRefObj _blockShowRefObj, MonoSkin _wnd) : base(_blockShow, _blockRefObj, _blockShowRefObj, _wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 播放动画后多久延迟直接结束
        /// </summary>
        protected override float aniCompleteDelayTimeS { get { return 0.5f; } }

        /// <summary>
        /// 播放通过彩虹生成格子动画
        /// </summary>
        /// <param name="_complete"></param>
        public void playCreateByRainbowAnimation(Action _complete)
        {
            if (hotfixWnd == null)
            {
                _complete?.Invoke();
                return;
            }

            _playAnimation(hotfixWnd.ani, hotfixWnd.createByRainbowAniName, _complete);
        }

        public override void proactiveUniteNormalTriggerShow(Action _complete)
        {
            if (_m_gameShow != null && hotfixWnd != null && _m_blockShow.rectTransform != null)
            {
                _m_gameShow.playCheckerboardSfx(hotfixWnd.proactiveUniteNormalTriggerCheckerboardClearSfxId, _m_blockShow.rectTransform.localPosition, 0, null);
                _complete?.Invoke();
            }
            else
            {
                _complete?.Invoke();
            }
        }

        public override void proactiveUniteBoomTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            if (_m_gameShow != null && hotfixWnd != null && _m_blockShow.rectTransform != null)
            {
                ETileMatchDirection direction = TileMatchUtil.getPosDirection(_m_blockShow.checkerLogicPos, _uniteBlockLogicPos);
                // 火箭默认是横向特效, 所以当前格子和联合触发格子是上下方向时, 需要旋转90度
                _m_gameShow.playCheckerboardSfx(hotfixWnd.proactiveUniteBoomTriggerCheckerboardClearSfxId, _m_blockShow.rectTransform.localPosition, direction==ETileMatchDirection.Down || direction==ETileMatchDirection.Up ? -90 : 0, null);
                _complete?.Invoke();
            }
            else
            {
                _complete?.Invoke();
            }
        }

        public override void proactiveUniteRocketTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            if (_m_gameShow != null && hotfixWnd != null && _m_blockShow.rectTransform != null)
            {
                ETileMatchDirection direction = TileMatchUtil.getPosDirection(_m_blockShow.checkerLogicPos, _uniteBlockLogicPos);
                // 火箭默认是横向特效, 所以当前格子和联合触发格子是上下方向时, 需要旋转90度
                _m_gameShow.playCheckerboardSfx(hotfixWnd.proactiveUniteRocketTriggerCheckerboardClearSfxId, _m_blockShow.rectTransform.localPosition, direction==ETileMatchDirection.Down || direction==ETileMatchDirection.Up ? -90 : 0, null);
                _complete?.Invoke();
            }
            else
            {
                _complete?.Invoke();
            }
        }
    }
}