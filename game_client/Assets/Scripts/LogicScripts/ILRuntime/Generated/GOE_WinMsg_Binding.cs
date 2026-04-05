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
    unsafe class GOE_WinMsg_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE.WinMsg);
            args = new Type[]{typeof(global::WinMsgType), typeof(ALPackage.MsgRecAction)};
            method = type.GetMethod("RegisterMsg", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RegisterMsg_0);
            args = new Type[]{typeof(global::WinMsgType), typeof(ALPackage.MsgRecAction)};
            method = type.GetMethod("UnregisterMsg", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UnregisterMsg_1);
            args = new Type[]{typeof(global::WinMsgType), typeof(System.Action)};
            method = type.GetMethod("RegisterMsgAct", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RegisterMsgAct_2);
            args = new Type[]{typeof(global::WinMsgType), typeof(System.Action)};
            method = type.GetMethod("UnregisterMsgAct", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UnregisterMsgAct_3);


        }


        static StackObject* RegisterMsg_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage.MsgRecAction @_callback = (ALPackage.MsgRecAction)typeof(ALPackage.MsgRecAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::WinMsgType @_type = (global::WinMsgType)typeof(global::WinMsgType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            GOE.WinMsg.RegisterMsg(@_type, @_callback);

            return __ret;
        }

        static StackObject* UnregisterMsg_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage.MsgRecAction @_callback = (ALPackage.MsgRecAction)typeof(ALPackage.MsgRecAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::WinMsgType @_type = (global::WinMsgType)typeof(global::WinMsgType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            GOE.WinMsg.UnregisterMsg(@_type, @_callback);

            return __ret;
        }

        static StackObject* RegisterMsgAct_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::WinMsgType @_type = (global::WinMsgType)typeof(global::WinMsgType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            GOE.WinMsg.RegisterMsgAct(@_type, @_callback);

            return __ret;
        }

        static StackObject* UnregisterMsgAct_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::WinMsgType @_type = (global::WinMsgType)typeof(global::WinMsgType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            GOE.WinMsg.UnregisterMsgAct(@_type, @_callback);

            return __ret;
        }



    }
}
