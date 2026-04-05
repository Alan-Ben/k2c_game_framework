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
    unsafe class ALPackage__ATALBasicUISubWnd_1_GGUIHotfixCommonMono_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>);
            args = new Type[]{};
            method = type.GetMethod("get_wnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_wnd_0);
            args = new Type[]{};
            method = type.GetMethod("discard", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, discard_1);
            args = new Type[]{};
            method = type.GetMethod("showWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showWnd_2);
            args = new Type[]{};
            method = type.GetMethod("hideWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, hideWnd_3);
            args = new Type[]{};
            method = type.GetMethod("initWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, initWnd_4);
            args = new Type[]{};
            method = type.GetMethod("resetWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, resetWnd_5);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("showWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showWnd_6);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("hideWnd", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, hideWnd_7);
            args = new Type[]{};
            method = type.GetMethod("get_isShow", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isShow_8);
            args = new Type[]{};
            method = type.GetMethod("get_rectTransform", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_rectTransform_9);
            args = new Type[]{};
            method = type.GetMethod("get_go", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_go_10);


        }


        static StackObject* get_wnd_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.wnd;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* discard_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.discard();

            return __ret;
        }

        static StackObject* showWnd_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showWnd();

            return __ret;
        }

        static StackObject* hideWnd_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.hideWnd();

            return __ret;
        }

        static StackObject* initWnd_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.initWnd();

            return __ret;
        }

        static StackObject* resetWnd_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.resetWnd();

            return __ret;
        }

        static StackObject* showWnd_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delayDoneAction = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showWnd(@_delayDoneAction);

            return __ret;
        }

        static StackObject* hideWnd_7(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delayDoneAction = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.hideWnd(@_delayDoneAction);

            return __ret;
        }

        static StackObject* get_isShow_8(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isShow;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_rectTransform_9(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.rectTransform;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_go_10(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono> instance_of_this_method = (ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>)typeof(ALPackage._ATALBasicUISubWnd<GOE.GGUIHotfixCommonMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.go;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
