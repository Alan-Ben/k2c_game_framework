using System;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 普通格子子窗口
    /// </summary>
    public class GGUIWndTileMatchCheckerSubNormal : _ATGGUIWndTileMatchCheckerSubBase<GGUIMonoTileMatchCheckerSubNormal>
    {
        public GGUIWndTileMatchCheckerSubNormal([NotNull] GGUIWndTileMatchChecker _blockShow, TileMatchBlockRefObj _blockRefObj, TileMatchBlockShowRefObj _blockShowRefObj, MonoSkin _wnd) : base(_blockShow, _blockRefObj, _blockShowRefObj, _wnd)
        {
            initWnd();
        }

        public override void proactiveUniteNormalTriggerShow(Action _complete)
        {
            playByClearAnimation(_complete);
        }

        public override void proactiveUniteBoomTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            playByClearAnimation(_complete);
        }

        public override void proactiveUniteRocketTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete)
        {
            playByClearAnimation(_complete);
        }
    }
}