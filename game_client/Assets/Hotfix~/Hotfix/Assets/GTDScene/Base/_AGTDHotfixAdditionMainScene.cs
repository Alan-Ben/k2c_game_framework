using System;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// Hotfix里面场景的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AGTDHotfixAdditionMainScene<T> : _ABasicAdditionMainTDScene
        where T : _AHotfixBaseMono, new()
    {
        //对应业务场景的mono
        private T _m_monoMain;
        protected T mono { get { return _m_monoMain; } }
        
        protected override void _onRootGOLoaded(GameObject _go)
        {
            if(null == _go)
                return;

            //已经有root的mono了不在处理
            if(null != _m_monoMain)
                return;
            
            MonoSkin monoSkin = _go.GetComponent<MonoSkin>();
            if(null == monoSkin)
                return;
            
            _m_monoMain = new T();
            _m_monoMain.init(monoSkin);
        }

        protected sealed override void _onQuitTDScene()
        {
            _m_monoMain = null;

            _onQuitTDSceneHotfix();
        }

        //Hotfix里的_onQuitTDScene
        protected abstract void _onQuitTDSceneHotfix();
    }
}