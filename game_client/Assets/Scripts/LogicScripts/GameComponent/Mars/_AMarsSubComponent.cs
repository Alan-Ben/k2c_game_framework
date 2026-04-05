using System;
using ALBasicProtocolPack;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// Mars子组件基类 - 简化版本
    /// </summary>
    public abstract class _ABaseMarsSubComponent
    {
        [NotNull] protected readonly MarsComponent _m_parentComponent;
        

        public _ABaseMarsSubComponent([NotNull] MarsComponent _parentComp)
        {
            _m_parentComponent = _parentComp;
        }
        

        [NotNull] public MarsComponent parentComponent { get { return _m_parentComponent; } }


        public abstract void discard();
    }
    public abstract class _AMarsSubComponent : _ABaseMarsSubComponent
    {
        protected _AMarsSubComponent([NotNull] MarsComponent _parentComp) 
            : base(_parentComp)
        {
        }
        

        public abstract void init(Action<bool> _complete);
    }
    public abstract class _AMarsSubComponent<T_C2S_MSG, T_S2C_MSG> : _ABaseMarsSubComponent 
        where T_C2S_MSG : _IALProtocolStructure, new() 
        where T_S2C_MSG : _IALProtocolStructure, new()
    {
        protected _AMarsSubComponent([NotNull] MarsComponent _parentComp) 
            : base(_parentComp)
        {
        }


        public void sendInitProtocol(Action<T_S2C_MSG> _protocolCallback)
        {
            NPGSClientListener.sendRequestByLog(new T_C2S_MSG(), new CommonRequestSucFailSameCallbackProtocolDealer<T_S2C_MSG>((_, _msg) =>
            {
                _protocolCallback?.Invoke(_msg);
            }));
        }
        public abstract void init(T_S2C_MSG _msg, Action<bool> _complete);
    }
}