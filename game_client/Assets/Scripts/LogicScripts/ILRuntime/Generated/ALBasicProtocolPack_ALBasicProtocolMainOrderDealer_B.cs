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
    unsafe class ALBasicProtocolPack_ALBasicProtocolMainOrderDealer_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ALBasicProtocolPack.ALBasicProtocolMainOrderDealer);
            args = new Type[]{typeof(ALBasicProtocolPack._IALBasicProtocolSubOrderInterface)};
            method = type.GetMethod("regDealer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, regDealer_0);

            args = new Type[]{typeof(System.Byte), typeof(System.Int32)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* regDealer_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack._IALBasicProtocolSubOrderInterface @_dealer = (ALBasicProtocolPack._IALBasicProtocolSubOrderInterface)typeof(ALBasicProtocolPack._IALBasicProtocolSubOrderInterface).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALBasicProtocolMainOrderDealer instance_of_this_method = (ALBasicProtocolPack.ALBasicProtocolMainOrderDealer)typeof(ALBasicProtocolPack.ALBasicProtocolMainOrderDealer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.regDealer(@_dealer);

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_protocolMaxTypeNum = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Byte @_mainOrder = (byte)ptr_of_this_method->Value;


            var result_of_this_method = new ALBasicProtocolPack.ALBasicProtocolMainOrderDealer(@_mainOrder, @_protocolMaxTypeNum);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
