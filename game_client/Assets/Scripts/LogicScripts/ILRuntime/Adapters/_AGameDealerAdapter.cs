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
    public class _AGameDealerAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GOE._AGameDealer);
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

        public class Adapter : GOE._AGameDealer, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo m_onStart_0 = new CrossBindingMethodInfo("_onStart");
            CrossBindingMethodInfo m_onStop_1 = new CrossBindingMethodInfo("_onStop");
            CrossBindingMethodInfo<System.Single> m_onTick_2 = new CrossBindingMethodInfo<System.Single>("_onTick");
            CrossBindingMethodInfo<GOE._AGameUnit> m_onAddGameUnit_3 = new CrossBindingMethodInfo<GOE._AGameUnit>("_onAddGameUnit");
            CrossBindingMethodInfo<GOE._AGameUnit> m_onRemoveGameUnit_4 = new CrossBindingMethodInfo<GOE._AGameUnit>("_onRemoveGameUnit");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter() : base(null)
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance) : base(null)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            protected override void _onStart()
            {
                m_onStart_0.Invoke(this.instance);
            }

            protected override void _onStop()
            {
                m_onStop_1.Invoke(this.instance);
            }

            protected override void _onTick(System.Single _deltaTime)
            {
                m_onTick_2.Invoke(this.instance, _deltaTime);
            }

            protected override void _onAddGameUnit(GOE._AGameUnit _unit)
            {
                m_onAddGameUnit_3.Invoke(this.instance, _unit);
            }

            protected override void _onRemoveGameUnit(GOE._AGameUnit _unit)
            {
                m_onRemoveGameUnit_4.Invoke(this.instance, _unit);
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

