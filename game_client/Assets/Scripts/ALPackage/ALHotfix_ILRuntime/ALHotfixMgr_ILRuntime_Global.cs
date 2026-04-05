using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;

using ALPackage;
using System.Threading;
using JetBrains.Annotations;

namespace ALPackage
{
    //全局唯一的管理对象
    public class ALHotfixMgr_ILRuntime_Global
    {
        /// <summary>
        /// 热更脚本版本号（SC == SourceCode）
        /// 初始化后，从热更工程读取
        /// </summary>
        public class HotfixSCVersion
        {
            public int main;
            public int sub;
            public int patch;
            public int build;
            public int curVersionNum { get { return (main * 1000000) + (sub * 10000) + (patch * 100) + build; } }
            public string versionStrNum { get { return $"{main}.{sub}.{patch}.{build}"; } }

            public HotfixSCVersion()
            {
                main = 0;
                sub = 0;
                patch = 0;
                build = 0;
            }

            public void setVersion(int _main, int _sub, int _patch, int _build)
            {
                main = _main;
                sub = _sub;
                patch = _patch;
                build = _build;
            }
        }

#if AL_ILRUNTIME
        private static ALHotfixMgr_ILRuntime_Global _g_instance = new ALHotfixMgr_ILRuntime_Global();
        [NotNull]
        public static ALHotfixMgr_ILRuntime_Global instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new ALHotfixMgr_ILRuntime_Global();

                return _g_instance;
            }
        }
        public static ILRuntime.Runtime.Enviorment.AppDomain g_AppDomain
        {
            get
            {
                return instance.appDomain;
            }
        }


        private bool _m_bIsInit = false;
        private ILRuntime.Runtime.Enviorment.AppDomain _m_appDomain;

        private HotfixSCVersion _m_version = new HotfixSCVersion();

        //返回全局环境变量
        public ILRuntime.Runtime.Enviorment.AppDomain appDomain { get { _checkAppDomain(); return _m_appDomain; } }

        protected ALHotfixMgr_ILRuntime_Global()
        {
            _m_bIsInit = false;
            _m_appDomain = null;
        }

        /// <summary>
        /// 热更代码版本号，项目里，更新完热更代码后，需要读取热更代码中的版本，然后设置到这里
        /// </summary>
        public HotfixSCVersion version { get { return _m_version; } }

        /// <summary>
        /// 设置热更版本号
        /// </summary>
        /// <param name="_main"></param>
        /// <param name="_sub"></param>
        /// <param name="_patch"></param>
        /// <param name="_build"></param>
        public void setVersion(int _main, int _sub, int _patch, int _build)
        {
            _m_version.setVersion(_main, _sub, _patch, _build);
        }

        /// <summary>
        /// 检测并创建appdomain对象
        /// </summary>
        private void _checkAppDomain()
        {
            if(_m_bIsInit)
                return;

            _m_bIsInit = true;

            _m_appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
#if UNITY_EDITOR
            //这边写死VS调试插件监听端口56000
            _m_appDomain.DebugService.StartDebugService(56000);
            
            //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
            _m_appDomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif

            //注册Alpackage里面的adapter
            bindILRuntimeAdapterFromAlpackage(_m_appDomain);
        }

        //统一注册函数，方便生成CLRbinding
        public static void bindILRuntimeAdapterFromAlpackage(ILRuntime.Runtime.Enviorment.AppDomain _appDomain)
        {
            _appDomain.RegisterCrossBindingAdaptor(new _IALMonoTaskAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _IALBasicUIWndInterfaceAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());

            _appDomain.RegisterCrossBindingAdaptor(new _IALProtocolStructureAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _IALBasicProtocolSubOrderInterfaceAdapter());

            _appDomain.RegisterCrossBindingAdaptor(new _AALBasicRefdataCoreMgrAdapter());
            _appDomain.RegisterCrossBindingAdaptor(new _IALInitRefObjAdapter());
        }

        /// <summary>
        /// 注册跨域基类处理对象
        /// </summary>
        /// <param name="adaptor"></param>
        public void RegisterCrossBindingAdaptor(ILRuntime.Runtime.Enviorment.CrossBindingAdaptor _adaptor)
        {
            regAdapter(_adaptor);
        }
        public void regAdapter(ILRuntime.Runtime.Enviorment.CrossBindingAdaptor _adaptor)
        {
            if(null == _adaptor)
                return;

            if(_m_appDomain == null)
            {
                Debug.LogError("regAdapter " + _adaptor + " 时，_m_appDomain还未初始化完成");
                return;
            }
            _m_appDomain.RegisterCrossBindingAdaptor(_adaptor);
        }

        /// <summary>
        /// 调用dll中的静态无参数函数
        /// </summary>
        /// <param name="_classPath">类路径（包含类名）</param>
        /// <param name="_funcName">函数名称</param>
        /// TODO 这种调用方式尽量不要频繁调用，每次都是通过字符串去找到对应函数执行，频繁调用可以先获取IMethod后缓存起来
        public object dealStaticFunc(string _classPath, string _funcName)
        {
            if(_m_appDomain == null)
            {
                Debug.LogError("调用dealStaticFunc " + _classPath + " " + _funcName + " 时，_m_appDomain还未初始化完成");
                return null;
            }

            return _m_appDomain.Invoke(_classPath, _funcName, null, null);
        }
        public object dealStaticFunc(string _classPath, string _funcName, params object[] _params)
        {
            if(_m_appDomain == null)
            {
                Debug.LogError("调用dealStaticFunc " + _classPath + " " + _funcName + " 时，_m_appDomain还未初始化完成");
                return null;
            }
            return _m_appDomain.Invoke(_classPath, _funcName, null, _params);
        }

        /// <summary>
        /// 获取DLL中的类对象
        /// </summary>
        /// <param name="_typeName"></param>
        /// <returns></returns>
        public Type getDllType(string _typeName)
        {
            if(_m_appDomain == null)
            {
                Debug.LogError("getDllType " + _typeName + "时，_m_appDomain还未初始化完成");
                return null;
            }
            ILRuntime.CLR.TypeSystem.IType type = _m_appDomain.LoadedTypes[_typeName];
            if(null == type)
                return null;

            return type.ReflectionType;
        }

        /// <summary>
        /// 创建DLL中类对象
        /// </summary>
        /// <param name="_typeName"></param>
        /// <returns></returns>
        public object createDllObj(string _typeName)
        {
            if(_m_appDomain == null)
            {
                Debug.LogError("createDllObj " + _typeName + "时，_m_appDomain还未初始化完成");
                return null;
            }
            return _m_appDomain.Instantiate(_typeName);
        }

        /// <summary>
        /// 调用Dll中类的对应函数
        /// </summary>
        /// <param name="_typeName"></param>
        /// <param name="_funcName"></param>
        public void invokeDllFunc(string _typeName, string _funcName)
        {
            Type dllType = getDllType(_typeName);
            if(null == dllType)
                return;

            if(_m_appDomain == null)
            {
                Debug.LogError("invokeDllFunc " + _typeName + "" + _funcName + "时，_m_appDomain还未初始化完成");
                return;
            }
            object typeObj = _m_appDomain.Instantiate(_typeName);
            if(null == typeObj)
                return;

            //执行处理
            MethodInfo mi = dllType.GetMethod(_funcName);
            if(null == mi)
                return;

            mi.Invoke(typeObj, null);
        }
        public void invokeDllFuncByIL(string _typeName, string _funcName)
        {
            Type dllType = getDllType(_typeName);
            if(null == dllType)
                return;

            if(_m_appDomain == null)
            {
                Debug.LogError("invokeDllFuncByIL " + _typeName + "" + _funcName + "时，_m_appDomain还未初始化完成");
                return;
            }
            object typeObj = _m_appDomain.Instantiate(_typeName);
            if(null == typeObj)
                return;

            //执行处理
            MethodInfo mi = dllType.GetMethod(_funcName, 0);
            if(null == mi)
                return;

            mi.Invoke(typeObj, null);
        }

        /// <summary>
        /// 设置Dll中对应实例的Field值
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_fieldName"></param>
        /// <param name="_objInstance"></param>
        /// <param name="_val"></param>
        public void setDllFieldVal(string _typeName, string _fieldName, object _objInstance, object _val)
        {
            setDllFieldVal(getDllType(_typeName), _fieldName, _objInstance, _val);
        }
        public void setDllFieldVal(Type _type, string _fieldName, object _objInstance, object _val)
        {
            if(null == _type)
                return;

            FieldInfo fi = _type.GetField(_fieldName);
            if(null == fi)
                return;

            fi.SetValue(_objInstance, _val);
        }

        /// <summary>
        /// 获取Dll中对应实例的Field值
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_fieldName"></param>
        /// <param name="_objInstance"></param>
        /// <param name="_val"></param>
        /// <returns></returns>
        public object getDllFieldVal(string _typeName, string _fieldName, object _objInstance, object _val)
        {
            return getDllFieldVal(getDllType(_typeName), _fieldName, _objInstance, _val);
        }
        public object getDllFieldVal(Type _type, string _fieldName, object _objInstance, object _val)
        {
            if(null == _type)
                return null;

            FieldInfo fi = _type.GetField(_fieldName);
            if(null == fi)
                return null;

            return fi.GetValue(_objInstance);
        }

        /// <summary>
        /// 获取对应类下的field的attributes
        /// </summary>
        /// <param name="_typeName"></param>
        /// <param name="_fieldName"></param>
        /// <returns></returns>
        public object[] getDllAttributes(string _typeName, string _fieldName, string _attributeTypeName)
        {
            return getDllAttributes(getDllType(_typeName), _fieldName, getDllType(_attributeTypeName));
        }
        public object[] getDllAttributes(Type _type, string _fieldName, string _attributeTypeName)
        {
            return getDllAttributes(_type, _fieldName, getDllType(_attributeTypeName));
        }
        public object[] getDllAttributes(string _typeName, string _fieldName, Type _attributeType)
        {
            return getDllAttributes(getDllType(_typeName), _fieldName, _attributeType);
        }
        public object[] getDllAttributes(Type _type, string _fieldName, Type _attributeType)
        {
            if(null == _type || null == _attributeType)
                return null;

            FieldInfo fi = _type.GetField(_fieldName);
            if(null == fi)
                return null;

            return fi.GetCustomAttributes(_attributeType, false);
        }
#endif
    }
}

