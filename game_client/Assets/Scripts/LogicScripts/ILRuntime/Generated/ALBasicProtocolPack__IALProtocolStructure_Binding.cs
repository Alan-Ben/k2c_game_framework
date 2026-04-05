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
    unsafe class ALBasicProtocolPack__IALProtocolStructure_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ALBasicProtocolPack._IALProtocolStructure);
            args = new Type[]{typeof(ALBasicProtocolPack.ALProtocolBuf)};
            method = type.GetMethod("readPackage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, readPackage_0);
            args = new Type[]{};
            method = type.GetMethod("getMainOrder", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getMainOrder_1);
            args = new Type[]{};
            method = type.GetMethod("getSubOrder", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getSubOrder_2);
            args = new Type[]{};
            method = type.GetMethod("GetFullPackBufSize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetFullPackBufSize_3);


        }


        static StackObject* readPackage_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf @_buf = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack._IALProtocolStructure instance_of_this_method = (ALBasicProtocolPack._IALProtocolStructure)typeof(ALBasicProtocolPack._IALProtocolStructure).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.readPackage(@_buf);

            return __ret;
        }

        static StackObject* getMainOrder_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack._IALProtocolStructure instance_of_this_method = (ALBasicProtocolPack._IALProtocolStructure)typeof(ALBasicProtocolPack._IALProtocolStructure).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getMainOrder();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* getSubOrder_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack._IALProtocolStructure instance_of_this_method = (ALBasicProtocolPack._IALProtocolStructure)typeof(ALBasicProtocolPack._IALProtocolStructure).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getSubOrder();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetFullPackBufSize_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack._IALProtocolStructure instance_of_this_method = (ALBasicProtocolPack._IALProtocolStructure)typeof(ALBasicProtocolPack._IALProtocolStructure).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetFullPackBufSize();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }



    }
}
