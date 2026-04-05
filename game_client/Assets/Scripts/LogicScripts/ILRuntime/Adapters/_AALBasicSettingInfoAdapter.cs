using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif

namespace GOE
{   
    public class _AALBasicSettingInfoAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(ALPackage._AALBasicSettingInfo);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : ALPackage._AALBasicSettingInfo, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo m_onInitErr_0 = new CrossBindingMethodInfo("_onInitErr");
            CrossBindingFunctionInfo<System.String> m_makeSettingStr_1 = new CrossBindingFunctionInfo<System.String>("_makeSettingStr");
            CrossBindingMethodInfo<System.String> m_initSettingStr_2 = new CrossBindingMethodInfo<System.String>("_initSettingStr");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter() : base(string.Empty)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance) : base(string.Empty)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override void _onInitErr()
            {
                if (m_onInitErr_0.CheckShouldInvokeBase(this.instance))
                    base._onInitErr();
                else
                    m_onInitErr_0.Invoke(this.instance);
            }

            protected override System.String _makeSettingStr()
            {
                return m_makeSettingStr_1.Invoke(this.instance);
            }

            protected override void _initSettingStr(System.String _infoStr)
            {
                m_initSettingStr_2.Invoke(this.instance, _infoStr);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

