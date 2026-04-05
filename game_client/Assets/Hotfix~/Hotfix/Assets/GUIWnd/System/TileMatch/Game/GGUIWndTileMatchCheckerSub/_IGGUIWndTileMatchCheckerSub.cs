using System;
using GOE;
using UnityEngine;

namespace Hotfix
{
    public interface _IGGUIWndTileMatchCheckerSub
    {
        _AGGUIHotfixBasicSimpleSubWnd getGUIHotfixBasicSimpleSubWnd { get; }

        void setGameShow(GGUIWndTileMatchGamePlay _gameShow);

        /// <summary>
        /// 播放生成格子动画
        /// </summary>
        /// <param name="_complete"></param>
        void playCreateAnimation(Action _complete);

        /// <summary>
        /// 播放被消除动画
        /// </summary>
        /// <param name="_complete"></param>
        void playByClearAnimation(Action _complete);

        /// <summary>
        /// 播放下落动画
        /// </summary>
        /// <param name="_complete"></param>
        void playFallAnimation(Action _complete);

        /// <summary>
        /// 播放idle动画
        /// </summary>
        void playIdleAnimation();

        /// <summary>
        /// 主动联合普通格子触发表现
        /// </summary>
        /// <param name="_complete"></param>
        void proactiveUniteNormalTriggerShow(Action _complete);
        
        /// <summary>
        /// 主动联合炸弹格子触发表现
        /// </summary>
        /// <param name="_complete"></param>
        void proactiveUniteBoomTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete);
        
        /// <summary>
        /// 主动联合火箭格子触发表现
        /// </summary>
        /// <param name="_complete"></param>
        void proactiveUniteRocketTriggerShow(Vector2Int _uniteBlockLogicPos, Action _complete);
    }
}