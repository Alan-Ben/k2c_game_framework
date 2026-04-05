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
    public class _ACommonListHotRefPatchDealerAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(GOE._ACommonListHotRefPatchDealer);
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

        public class Adapter : GOE._ACommonListHotRefPatchDealer, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<System.String> mget_getTableName_0 = new CrossBindingFunctionInfo<System.String>("get_getTableName");
            CrossBindingFunctionInfo<System.Int64, System.Collections.Generic.Dictionary<System.String, System.String>, System.Boolean> m_applyPatch_1 = new CrossBindingFunctionInfo<System.Int64, System.Collections.Generic.Dictionary<System.String, System.String>, System.Boolean>("_applyPatch");

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

            protected override System.Boolean _applyPatch(System.Int64 _refId, System.Collections.Generic.Dictionary<System.String, System.String> _valueDict)
            {
                return m_applyPatch_1.Invoke(this.instance, _refId, _valueDict);
            }

            public override System.String getTableName
            {
            get
            {
                return mget_getTableName_0.Invoke(this.instance);

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

