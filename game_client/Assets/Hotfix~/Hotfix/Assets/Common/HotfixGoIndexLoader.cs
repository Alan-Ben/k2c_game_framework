using System;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 热更工程中用于加载 NPGGoIndex 资源的工具类
    /// 热更工程无法直接调用主工程的 GGoResCore.instance.loadObj，通过此类统一封装加载入口
    /// </summary>
    public class HotfixGoIndexLoader
    {
        /// <summary>
        /// 加载 NPGGoIndex 对应的 GameObject，完成后调用回调
        /// </summary>
        /// <param name="_goIndex">要加载的 GoIndex</param>
        /// <param name="_onLoaded">加载完成回调，参数为加载到的 GameObject（失败时为 null）</param>
        public static void loadGameObject(NPGGoIndex _goIndex, Action<GameObject> _onLoaded)
        {
            if (_goIndex == null || !_goIndex.isValid())
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"HotfixGoIndexLoader: NPGGoIndex 为 null 或无效");
#endif
                _onLoaded?.Invoke(null);
                return;
            }

            // 使用 HotfixAssetLoader 通过 GameResCore 加载对应 assetPath/objName 的 GameObject
            HotfixAssetLoader<GameObject> loader = new HotfixAssetLoader<GameObject>(
                GameResCore.instance,
                _goIndex.assetPath,
                _goIndex.objName
            );

            loader.loadAsset(_onLoaded);
        }
    }
}