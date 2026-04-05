using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif
namespace ILRuntime.Runtime.Generated
{
    unsafe class GS2GC_p017_ActivityOp_GS2GC_017_003_RetActivityRankBaseList_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList);
            args = new Type[]{};
            method = type.GetMethod("getBaseItemlist", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getBaseItemlist_0);


        }


        static StackObject* getBaseItemlist_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList instance_of_this_method = (GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList)typeof(GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getBaseItemlist();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
