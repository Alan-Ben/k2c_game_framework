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
    public class _AGameLogicAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GOE._AGameLogic);
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

        public class Adapter : GOE._AGameLogic, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<System.Single> mget_deltaTime_0 = new CrossBindingFunctionInfo<System.Single>("get_deltaTime");
            CrossBindingMethodInfo<System.Action, System.Action> m_startGameOp_1 = new CrossBindingMethodInfo<System.Action, System.Action>("_startGameOp");
            CrossBindingMethodInfo m_onStart_2 = new CrossBindingMethodInfo("_onStart");
            CrossBindingMethodInfo m_onStop_3 = new CrossBindingMethodInfo("_onStop");
            CrossBindingMethodInfo<System.Single> m_onTick_4 = new CrossBindingMethodInfo<System.Single>("_onTick");

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

            protected override void _startGameOp(System.Action _complete, System.Action _failed)
            {
                m_startGameOp_1.Invoke(this.instance, _complete, _failed);
            }

            protected override void _onStart()
            {
                m_onStart_2.Invoke(this.instance);
            }

            protected override void _onStop()
            {
                m_onStop_3.Invoke(this.instance);
            }

            protected override void _onTick(System.Single _deltaTime)
            {
                m_onTick_4.Invoke(this.instance, _deltaTime);
            }

            public override System.Single deltaTime
            {
            get
            {
                if (mget_deltaTime_0.CheckShouldInvokeBase(this.instance))
                    return base.deltaTime;
                else
                    return mget_deltaTime_0.Invoke(this.instance);

            }
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

