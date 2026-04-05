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
    unsafe class GOE_CommonTaskController_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE.CommonTaskController);
            args = new Type[]{typeof(System.Action), typeof(System.Single)};
            method = type.GetMethod("CommonActionAddMonoTask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CommonActionAddMonoTask_0);
            args = new Type[]{typeof(System.Action), typeof(System.Single)};
            method = type.GetMethod("CommonEnableDurationActionAddMonoTask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CommonEnableDurationActionAddMonoTask_1);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("CommonActionAddNextFrameLaterTask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CommonActionAddNextFrameLaterTask_2);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("CommonActionAddMonoTask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CommonActionAddMonoTask_3);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("CommonActionAddNextFrameTask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CommonActionAddNextFrameTask_4);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("CommonEnableTickActionAddMonoTask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CommonEnableTickActionAddMonoTask_5);


        }


        static StackObject* CommonActionAddMonoTask_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @_delayTime = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);


            GOE.CommonTaskController.CommonActionAddMonoTask(@_delegate, @_delayTime);

            return __ret;
        }

        static StackObject* CommonEnableDurationActionAddMonoTask_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @_duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.CommonTaskController.CommonEnableDurationActionAddMonoTask(@_delegate, @_duration);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CommonActionAddNextFrameLaterTask_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);


            GOE.CommonTaskController.CommonActionAddNextFrameLaterTask(@_delegate);

            return __ret;
        }

        static StackObject* CommonActionAddMonoTask_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);


            GOE.CommonTaskController.CommonActionAddMonoTask(@_delegate);

            return __ret;
        }

        static StackObject* CommonActionAddNextFrameTask_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);


            GOE.CommonTaskController.CommonActionAddNextFrameTask(@_delegate);

            return __ret;
        }

        static StackObject* CommonEnableTickActionAddMonoTask_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.CommonTaskController.CommonEnableTickActionAddMonoTask(@_delegate);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
