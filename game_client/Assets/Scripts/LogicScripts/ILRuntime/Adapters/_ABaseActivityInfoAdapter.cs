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
    public class _ABaseActivityInfoAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GOE._ABaseActivityInfo);
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

        public class Adapter : GOE._ABaseActivityInfo, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo m_onActivityStart_0 = new CrossBindingMethodInfo("_onActivityStart");
            CrossBindingMethodInfo m_onActivityEnd_1 = new CrossBindingMethodInfo("_onActivityEnd");
            CrossBindingMethodInfo m_onActivityClosed_2 = new CrossBindingMethodInfo("_onActivityClosed");
            CrossBindingMethodInfo m_onInit_3 = new CrossBindingMethodInfo("_onInit");
            CrossBindingMethodInfo<System.Action> m_onSubDataInit_4 = new CrossBindingMethodInfo<System.Action>("_onSubDataInit");
            CrossBindingMethodInfo m_onDiscard_5 = new CrossBindingMethodInfo("_onDiscard");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override void _onActivityStart()
            {
                m_onActivityStart_0.Invoke(this.instance);
            }

            protected override void _onActivityEnd()
            {
                m_onActivityEnd_1.Invoke(this.instance);
            }

            protected override void _onActivityClosed()
            {
                m_onActivityClosed_2.Invoke(this.instance);
            }

            protected override void _onInit()
            {
                m_onInit_3.Invoke(this.instance);
            }

            protected override void _onSubDataInit(System.Action _doneDelegate)
            {
                m_onSubDataInit_4.Invoke(this.instance, _doneDelegate);
            }

            protected override void _onDiscard()
            {
                m_onDiscard_5.Invoke(this.instance);
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

