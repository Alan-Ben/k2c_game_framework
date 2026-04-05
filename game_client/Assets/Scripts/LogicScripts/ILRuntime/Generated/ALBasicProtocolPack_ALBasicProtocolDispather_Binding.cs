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
    unsafe class ALBasicProtocolPack_ALBasicProtocolDispather_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ALBasicProtocolPack.ALBasicProtocolDispather);
            args = new Type[]{typeof(ALBasicProtocolPack.ALBasicProtocolMainOrderDealer)};
            method = type.GetMethod("RegProtocol", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RegProtocol_0);
            args = new Type[]{typeof(ALBasicProtocolPack._IALProtocolDealer), typeof(System.Byte[])};
            method = type.GetMethod("DealProtocol", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DealProtocol_1);


        }


        static StackObject* RegProtocol_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALBasicProtocolMainOrderDealer @_dispathRegister = (ALBasicProtocolPack.ALBasicProtocolMainOrderDealer)typeof(ALBasicProtocolPack.ALBasicProtocolMainOrderDealer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALBasicProtocolDispather instance_of_this_method = (ALBasicProtocolPack.ALBasicProtocolDispather)typeof(ALBasicProtocolPack.ALBasicProtocolDispather).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RegProtocol(@_dispathRegister);

            return __ret;
        }

        static StackObject* DealProtocol_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Byte[] @_msg = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack._IALProtocolDealer @_dealer = (ALBasicProtocolPack._IALProtocolDealer)typeof(ALBasicProtocolPack._IALProtocolDealer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            ALBasicProtocolPack.ALBasicProtocolDispather instance_of_this_method = (ALBasicProtocolPack.ALBasicProtocolDispather)typeof(ALBasicProtocolPack.ALBasicProtocolDispather).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.DealProtocol(@_dealer, @_msg);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }



    }
}
