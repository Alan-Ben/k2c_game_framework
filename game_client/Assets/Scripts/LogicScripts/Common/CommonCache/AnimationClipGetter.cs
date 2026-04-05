
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class AnimationClipGetter : _AUniqueObjectSharedGetter<GClipIndex, AnimationClip>
    {
        [NotNull] public static AnimationClipGetter instance { get { return _g_instance ??= new AnimationClipGetter(); } }
        private static AnimationClipGetter _g_instance;
        
        protected override void _loadObject(GClipIndex _key, Action<AnimationClip> _complete)
        {
            if (_key == null || !_key.isValid())
            {
                _complete.Invoke(null);
                return;
            }

            void _onAssetBundleLoaded(bool _isSuc, ALAssetBundleObj _abObj)
            {
                if (!_isSuc)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("Load AnimationClip: " + _key.mainId + " - " + _key.subId + " Fail!");
#endif
                    _complete.Invoke(null);
                    return;
                }

                AnimationClip clip = _abObj?.load<AnimationClip>(_key.objName) as AnimationClip;
                _complete.Invoke(clip);
            }
            
#if UNITY_EDITOR
            ALLocalResLoaderMgr.instance.loadTemplateObjectAsset(_key.assetPath, _key.objName, ".fbx", "t:AnimationClip", _onAssetBundleLoaded, null, _complete, GameResCore.instance);
#else
            GameResCore.instance.loadAsset(_key.assetPath, _onAssetBundleLoaded, null);
#endif   
        }

        protected override void _discardObject(GClipIndex _key, AnimationClip _object)
        {
            
        }
    }
}