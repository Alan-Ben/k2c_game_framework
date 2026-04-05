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
    unsafe class ALPackage__ATALBasicUISubWnd_1_GGUISubMonoQualityShowGo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>);
            args = new Type[]{};
            method = type.GetMethod("showWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showWnd_0);
            args = new Type[]{};
            method = type.GetMethod("hideWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, hideWnd_1);
            args = new Type[]{};
            method = type.GetMethod("resetWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, resetWnd_2);
            args = new Type[]{};
            method = type.GetMethod("discard", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, discard_3);


        }


        static StackObject* showWnd_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showWnd();

            return __ret;
        }

        static StackObject* hideWnd_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.hideWnd();

            return __ret;
        }

        static StackObject* resetWnd_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.resetWnd();

            return __ret;
        }

        static StackObject* discard_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUISubMonoQualityShowGo>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.discard();

            return __ret;
        }



    }
}
