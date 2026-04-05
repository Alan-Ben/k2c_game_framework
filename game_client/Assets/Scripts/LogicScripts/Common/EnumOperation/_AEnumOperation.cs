
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 对应某个枚举的操作
    /// </summary>
    public abstract class _AEnumOperation
    {
        public abstract void deal(object _params);
    }
    public abstract class _AEnumOperation<T> : _AEnumOperation
    {
        public sealed override void deal(object _params)
        {
            if (!(_params is T operationData))
                return;
            
            dealOperation(operationData);
        }

        protected abstract void dealOperation(T _operationData);
    }
    public abstract class _AEnumOperation<T,Y> : _AEnumOperation
    {
        public sealed override void deal(object _params)
        {
            if (!(_params is object[] operationDataArray) ||
                operationDataArray.Length < 2 ||
                !(operationDataArray[0] is T operationDataT) ||
                !(operationDataArray[1] is Y operationDataY))
                return;
            
            dealOperation(operationDataT, operationDataY);
        }

        protected abstract void dealOperation(T _operationDataT, Y _operationDataY);
    }
    
    public abstract class _AEnumOperationWithProtocol<T> : _AEnumOperation 
        where T : _IALProtocolStructure, new()
    {
        public sealed override void deal(object _params)
        {
            if (!(_params is byte[] operationData))
                return;
            
            T protocol = new T();
            protocol.readPackage(new ALProtocolBuf(operationData));
            dealOperation(protocol);
        }

        protected abstract void dealOperation(T _operationData);
    }
}