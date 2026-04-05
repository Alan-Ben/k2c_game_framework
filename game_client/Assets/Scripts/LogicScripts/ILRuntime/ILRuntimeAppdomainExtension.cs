using System;
using ILRuntime.CLR.TypeSystem;

namespace ALPackage
{
    /// <summary>
    /// ILRuntime——Appdomain的一些扩展方法
    /// </summary>
    public static class ILRuntimeAppdomainExtension
    {
        /// <summary>
        /// 获取DLL中的类对象
        /// </summary>
        /// <param name="_typeName"></param>
        /// <returns></returns>
        public static Type getDllType(this ILRuntime.Runtime.Enviorment.AppDomain _appDomain, string _typeName)
        {
            if (null == _appDomain || null == _appDomain.LoadedTypes || string.IsNullOrEmpty(_typeName))
                return null;
            
            IType typeIItem = null;
            _appDomain.LoadedTypes.TryGetValue(_typeName, out typeIItem);
            if(null == typeIItem)
                return null;

            return typeIItem.ReflectionType;
        }
    }
}