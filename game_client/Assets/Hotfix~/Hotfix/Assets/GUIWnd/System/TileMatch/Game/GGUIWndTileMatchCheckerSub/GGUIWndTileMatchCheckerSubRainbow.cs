using System;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 普通格子子窗口
    /// </summary>
    public class GGUIWndTileMatchCheckerSubRainbow : _ATGGUIWndTileMatchCheckerSubBase<GGUIMonoTileMatchCheckerSubRainbow>
    {
        public GGUIWndTileMatchCheckerSubRainbow([NotNull] GGUIWndTileMatchChecker _blockShow, TileMatchBlockRefObj _blockRefObj, TileMatchBlockShowRefObj _blockShowRefObj, MonoSkin _wnd) : base(_blockShow, _blockRefObj, _blockShowRefObj, _wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 播放动画后多久延迟直接结束
        /// </summary>
        protected override float aniCompleteDelayTimeS { get { return 0.5f; } }

        public override void proactiveUniteNormalTriggerShow(Action _complete)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }
            
            // 联合普通格子触发动画
            _playAnimation(hotfixWnd.ani, hotfixWnd.uniteNormalTriggerAniName, _complete);
        }
        /// <summary>
        /// 显示联合普通格子飞行特效
        /// </summary>
        /// <param name="_targetLogicPos"></param>
        /// <param name="_complete"></param>
        public void showUniteNormalFlySfx(Vector2Int _targetLogicPos, Action _complete = null)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }

            long showSerialize = _m_lShowSerialize;
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(showSerialize != _m_lShowSerialize || _m_gameShow == null || hotfixWnd == null)
                    return;
                
                _m_gameShow.playCheckerboardFlySfx(hotfixWnd.uniteNormalTriggerFlySfxId, _m_blockShow.checkerLogicPos, _targetLogicPos, hotfixWnd.sfxFlyTimeS, _complete);
            }, hotfixWnd.beginTriggerToFlyTimeS);
        }
        
        public override void proactiveUniteBoomTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }
            
            // 联合普通格子触发动画
            _playAnimation(hotfixWnd.ani, hotfixWnd.uniteBoomTriggerAniName, _complete);
        }

        /// <summary>
        /// 显示联合爆炸格子飞行特效
        /// </summary>
        /// <param name="_targetLogicPos"></param>
        /// <param name="_complete"></param>
        public void showUniteBoomFlySfx(Vector2Int _targetLogicPos, Action _complete = null)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }

            long showSerialize = _m_lShowSerialize;
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(showSerialize != _m_lShowSerialize || _m_gameShow == null || hotfixWnd == null)
                    return;
                
                _m_gameShow.playCheckerboardFlySfx(hotfixWnd.uniteBoomTriggerFlySfxId, _m_blockShow.checkerLogicPos, _targetLogicPos, hotfixWnd.sfxFlyTimeS, _complete);
            }, hotfixWnd.beginTriggerToFlyTimeS);
        }

        public override void proactiveUniteRocketTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }
            
            // 联合普通格子触发动画
            _playAnimation(hotfixWnd.ani, hotfixWnd.uniteRocketTriggerAniName, _complete);
        }
        /// <summary>
        /// 显示联合火箭格子飞行特效
        /// </summary>
        /// <param name="_targetLogicPos"></param>
        /// <param name="_complete"></param>
        public void showUniteRocketFlySfx(Vector2Int _targetLogicPos, Action _complete = null)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }

            long showSerialize = _m_lShowSerialize;
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(showSerialize != _m_lShowSerialize || _m_gameShow == null || hotfixWnd == null)
                    return;
                
                _m_gameShow.playCheckerboardFlySfx(hotfixWnd.uniteRocketTriggerFlySfxId, _m_blockShow.checkerLogicPos, _targetLogicPos, hotfixWnd.sfxFlyTimeS, _complete);
            }, hotfixWnd.beginTriggerToFlyTimeS);
        }
        
        /// <summary>
        /// 联合彩虹格子触发动画
        /// </summary>
        /// <param name="_uniteBlockLogicPos"></param>
        /// <param name="_complete"></param>
        public void uniteRainbowTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }
            
            // 联合普通格子触发动画
            _playAnimation(hotfixWnd.ani, hotfixWnd.uniteRainbowTriggerAniName, _complete);
        }
        /// <summary>
        /// 显示联合彩虹格子棋盘清除特效
        /// </summary>
        public void showUniteRainbowCheckerboardClear(Action _complete = null)
        {
            if (_m_gameShow == null || hotfixWnd == null || rectTransform == null)
            {
                _complete?.Invoke();
                return;
            }

            _m_gameShow.playCheckerboardSfx(hotfixWnd.uniteRainbowTriggerCheckerboardClearSfxId, _m_gameShow.getCheckerboardCenterLocalPosition(), 0, _complete);
        }
    }
}