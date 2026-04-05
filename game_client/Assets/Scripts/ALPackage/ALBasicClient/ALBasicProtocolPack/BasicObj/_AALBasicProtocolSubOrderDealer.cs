using System;
using ALPackage;

namespace ALBasicProtocolPack
{
    public abstract class _AALBasicProtocolSubOrderDealer<T> : _AALBasicProtocolSubBasicOrderDealer<T> where T : _IALProtocolStructure
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override T _createProtocolObj()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
