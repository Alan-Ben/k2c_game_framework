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
    unsafe class ALPackage_ALSOGlobalSetting_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ALPackage.ALSOGlobalSetting);
            args = new Type[]{};
            method = type.GetMethod("get_Instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Instance_0);

            field = type.GetField("logLevel", flag);
            app.RegisterCLRFieldGetter(field, get_logLevel_0);
            app.RegisterCLRFieldSetter(field, set_logLevel_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_logLevel_0, AssignFromStack_logLevel_0);


        }


        static StackObject* get_Instance_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ALPackage.ALSOGlobalSetting.Instance;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_logLevel_0(ref object o)
        {
            return ((ALPackage.ALSOGlobalSetting)o).logLevel;
        }

        static StackObject* CopyToStack_logLevel_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((ALPackage.ALSOGlobalSetting)o).logLevel;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_logLevel_0(ref object o, object v)
        {
            ((ALPackage.ALSOGlobalSetting)o).logLevel = (ALPackage.ALLogLevel)v;
        }

        static StackObject* AssignFromStack_logLevel_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ALPackage.ALLogLevel @logLevel = (ALPackage.ALLogLevel)typeof(ALPackage.ALLogLevel).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            ((ALPackage.ALSOGlobalSetting)o).logLevel = @logLevel;
            return ptr_of_this_method;
        }



    }
}
