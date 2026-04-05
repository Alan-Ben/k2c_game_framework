using System;
using UnityEngine;
using System.Collections;

namespace GOE
{
    /// <summary>
    /// UI粒子管理器
    /// </summary>
    public static class UIParticleMgr
    {
        /// <summary>
        /// 显示炸裂粒子
        /// </summary>
        /// <param name="_particleId">粒子ID</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_startUIPos">起点</param>
        /// <param name="_endUIPos">终点</param>
        /// <param name="_onGroupLoaded">粒子组加载完成回调，方便外部管理生成出来的粒子组</param>
        /// <param name="_onItemLoaded">粒子加载完成回调</param>
        /// <param name="_doneAction">全部表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        public static void playParticleByUIPos(
            long _particleId,
            int _particleNum,
            Vector2 _startUIPos,
            Vector2 _endUIPos,
            Action<BurstParticleGroupInfo> _onGroupLoaded = null,
            Action<ParticleCacheItem> _onItemLoaded = null,
            Action _doneAction = null,
            Action _onBurstDone = null,
            Action _onFlyDone = null)
        {
            //此处增加加载判断是为了避免多次重复加载
            if (!GGUIWndBurstParticle.instance.isLoaded)
                GGUIWndBurstParticle.instance.load();

            GGUIWndBurstParticle.instance.regLoadDoneDelegate(() => {
                    GGUIWndBurstParticle.instance.showWnd();
                    GGUIWndBurstParticle.instance.showBurstParticleByUIPos(_particleId, _particleNum, _startUIPos, _endUIPos, _onGroupLoaded, _onItemLoaded, _doneAction, _onBurstDone, _onFlyDone);
                });
        }

        /// <summary>
        /// 显示炸裂粒子
        /// </summary>
        /// <param name="_particleId">粒子ID</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_startRect">起点</param>
        /// <param name="_endRect">终点</param>
        /// <param name="_onGroupLoaded">粒子组加载完成回调，方便外部管理生成出来的粒子组</param>
        /// <param name="_onItemLoaded">粒子加载完成回调</param>
        /// <param name="_doneAction">表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        public static void playParticleByUIPos(
            long _particleId,
            int _particleNum,
            RectTransform _startRect,
            RectTransform _endRect,
            Action<BurstParticleGroupInfo> _onGroupLoaded = null,
            Action<ParticleCacheItem> _onItemLoaded = null,
            Action _doneAction = null,
            Action _onBurstDone = null,
            Action _onFlyDone = null)
        {
            //此处增加加载判断是为了避免多次重复加载
            if (!GGUIWndBurstParticle.instance.isLoaded)
                GGUIWndBurstParticle.instance.load();

            GGUIWndBurstParticle.instance.regLoadDoneDelegate(() => {
                GGUIWndBurstParticle.instance.showWnd();
                    GGUIWndBurstParticle.instance.showBurstParticleByUIPos(_particleId, _particleNum, _startRect, _endRect, _onGroupLoaded, _onItemLoaded, _doneAction, _onBurstDone, _onFlyDone);
                });
        }

        /// <summary>
        /// 显示炸裂粒子
        /// </summary>
        /// <param name="_particleId">粒子ID</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_start3DWorldPos">起点</param>
        /// <param name="_end3DWorldPos">终点</param>
        /// <param name="_onGroupLoaded">粒子组加载完成回调，方便外部管理生成出来的粒子组</param>
        /// <param name="_onItemLoaded">粒子加载完成回调</param>
        /// <param name="_doneAction">全部表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        public static void playParticle(
            long _particleId,
            int _particleNum,
            Vector3 _start3DWorldPos,
            Vector3 _end3DWorldPos,
            Action<BurstParticleGroupInfo> _onGroupLoaded = null,
            Action<ParticleCacheItem> _onItemLoaded = null,
            Action _doneAction = null,
            Action _onBurstDone = null,
            Action _onFlyDone = null)
        {
            //此处增加加载判断是为了避免多次重复加载
            if (!GGUIWndBurstParticle.instance.isLoaded)
                GGUIWndBurstParticle.instance.load();

            GGUIWndBurstParticle.instance.regLoadDoneDelegate(() => {
                GGUIWndBurstParticle.instance.showWnd();
                    GGUIWndBurstParticle.instance.showBurstParticle(_particleId, _particleNum, _start3DWorldPos, _end3DWorldPos, _onGroupLoaded, _onItemLoaded, _doneAction, _onBurstDone, _onFlyDone);
                });
        }
    }
}
