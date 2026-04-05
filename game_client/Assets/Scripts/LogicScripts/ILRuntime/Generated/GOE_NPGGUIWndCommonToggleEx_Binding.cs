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
    unsafe class GOE_NPGGUIWndCommonToggleEx_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE.NPGGUIWndCommonToggleEx);
            args = new Type[]{};
            method = type.GetMethod("get_clickDelegate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_clickDelegate_0);
            args = new Type[]{typeof(System.Action<GOE.NPGGUIWndCommonToggleEx>)};
            method = type.GetMethod("set_clickDelegate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_clickDelegate_1);
            args = new Type[]{typeof(System.Boolean), typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("setSelected", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setSelected_2);
            args = new Type[]{};
            method = type.GetMethod("get_isOn", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isOn_3);

            args = new Type[]{typeof(GOE.NPGGUIMonoCommonToggleEx)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* get_clickDelegate_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE.NPGGUIWndCommonToggleEx instance_of_this_method = (GOE.NPGGUIWndCommonToggleEx)typeof(GOE.NPGGUIWndCommonToggleEx).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.clickDelegate;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_clickDelegate_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<GOE.NPGGUIWndCommonToggleEx> @value = (System.Action<GOE.NPGGUIWndCommonToggleEx>)typeof(System.Action<GOE.NPGGUIWndCommonToggleEx>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.NPGGUIWndCommonToggleEx instance_of_this_method = (GOE.NPGGUIWndCommonToggleEx)typeof(GOE.NPGGUIWndCommonToggleEx).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.clickDelegate = value;

            return __ret;
        }

        static StackObject* setSelected_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @_showAni = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @_force = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Boolean @_isSelect = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            GOE.NPGGUIWndCommonToggleEx instance_of_this_method = (GOE.NPGGUIWndCommonToggleEx)typeof(GOE.NPGGUIWndCommonToggleEx).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setSelected(@_isSelect, @_force, @_showAni);

            return __ret;
        }

        static StackObject* get_isOn_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE.NPGGUIWndCommonToggleEx instance_of_this_method = (GOE.NPGGUIWndCommonToggleEx)typeof(GOE.NPGGUIWndCommonToggleEx).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isOn;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE.NPGGUIMonoCommonToggleEx @_wnd = (GOE.NPGGUIMonoCommonToggleEx)typeof(GOE.NPGGUIMonoCommonToggleEx).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new GOE.NPGGUIWndCommonToggleEx(@_wnd);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
