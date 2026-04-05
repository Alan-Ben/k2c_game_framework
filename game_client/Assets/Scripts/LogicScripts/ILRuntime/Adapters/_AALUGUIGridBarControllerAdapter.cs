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
    public class _AALUGUIGridBarControllerAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(ALPackage._AALUGUIGridBarController);
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

        public class Adapter : ALPackage._AALUGUIGridBarController, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<System.Boolean> mget_isAfterLineBar_0 = new CrossBindingFunctionInfo<System.Boolean>("get_isAfterLineBar");
            CrossBindingFunctionInfo<ALPackage._AALUGUIGridBarController, System.Int32> m_compareWhenEqualIdx_1 = new CrossBindingFunctionInfo<ALPackage._AALUGUIGridBarController, System.Int32>("_compareWhenEqualIdx");
            CrossBindingFunctionInfo<System.Int32> mget_barHeight_2 = new CrossBindingFunctionInfo<System.Int32>("get_barHeight");
            CrossBindingMethodInfo mshow_3 = new CrossBindingMethodInfo("show");
            CrossBindingMethodInfo mhide_4 = new CrossBindingMethodInfo("hide");
            CrossBindingMethodInfo<System.Single, System.Single> msetPos_5 = new CrossBindingMethodInfo<System.Single, System.Single>("setPos");
            CrossBindingMethodInfo m_reset_6 = new CrossBindingMethodInfo("_reset");
            CrossBindingMethodInfo m_discard_7 = new CrossBindingMethodInfo("_discard");

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

            protected override System.Int32 _compareWhenEqualIdx(ALPackage._AALUGUIGridBarController _other)
            {
                if (m_compareWhenEqualIdx_1.CheckShouldInvokeBase(this.instance))
                    return base._compareWhenEqualIdx(_other);
                else
                    return m_compareWhenEqualIdx_1.Invoke(this.instance, _other);
            }

            public override void show()
            {
                mshow_3.Invoke(this.instance);
            }

            public override void hide()
            {
                mhide_4.Invoke(this.instance);
            }

            public override void setPos(System.Single _x, System.Single _y)
            {
                msetPos_5.Invoke(this.instance, _x, _y);
            }

            protected override void _reset()
            {
                m_reset_6.Invoke(this.instance);
            }

            protected override void _discard()
            {
                m_discard_7.Invoke(this.instance);
            }

            public override System.Boolean isAfterLineBar
            {
            get
            {
                if (mget_isAfterLineBar_0.CheckShouldInvokeBase(this.instance))
                    return base.isAfterLineBar;
                else
                    return mget_isAfterLineBar_0.Invoke(this.instance);

            }
            }

            public override System.Int32 barHeight
            {
            get
            {
                return mget_barHeight_2.Invoke(this.instance);

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

