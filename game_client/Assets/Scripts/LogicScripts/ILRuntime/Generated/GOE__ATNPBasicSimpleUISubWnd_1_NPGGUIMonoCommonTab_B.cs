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
    unsafe class GOE__ATNPBasicSimpleUISubWnd_1_NPGGUIMonoCommonTab_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>);
            args = new Type[]{};
            method = type.GetMethod("resetWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, resetWnd_0);
            args = new Type[]{};
            method = type.GetMethod("discard", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, discard_1);
            args = new Type[]{};
            method = type.GetMethod("initWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, initWnd_2);
            args = new Type[]{};
            method = type.GetMethod("hideWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, hideWnd_3);
            args = new Type[]{};
            method = type.GetMethod("showWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showWnd_4);
            args = new Type[]{};
            method = type.GetMethod("get_isShow", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isShow_5);
            args = new Type[]{};
            method = type.GetMethod("get_wnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_wnd_6);


        }


        static StackObject* resetWnd_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab> instance_of_this_method = (GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>)typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.resetWnd();

            return __ret;
        }

        static StackObject* discard_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab> instance_of_this_method = (GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>)typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.discard();

            return __ret;
        }

        static StackObject* initWnd_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab> instance_of_this_method = (GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>)typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.initWnd();

            return __ret;
        }

        static StackObject* hideWnd_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab> instance_of_this_method = (GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>)typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.hideWnd();

            return __ret;
        }

        static StackObject* showWnd_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab> instance_of_this_method = (GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>)typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showWnd();

            return __ret;
        }

        static StackObject* get_isShow_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab> instance_of_this_method = (GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>)typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isShow;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_wnd_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab> instance_of_this_method = (GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>)typeof(GOE._ATNPBasicSimpleUISubWnd<global::NPGGUIMonoCommonTab>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.wnd;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
