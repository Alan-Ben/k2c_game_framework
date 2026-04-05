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
    unsafe class GOE_HotfixMonoAttribute_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.HotfixMonoAttribute);

            field = type.GetField("isSerialize", flag);
            app.RegisterCLRFieldGetter(field, get_isSerialize_0);
            app.RegisterCLRFieldSetter(field, set_isSerialize_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_isSerialize_0, AssignFromStack_isSerialize_0);


        }



        static object get_isSerialize_0(ref object o)
        {
            return ((GOE.HotfixMonoAttribute)o).isSerialize;
        }

        static StackObject* CopyToStack_isSerialize_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.HotfixMonoAttribute)o).isSerialize;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isSerialize_0(ref object o, object v)
        {
            ((GOE.HotfixMonoAttribute)o).isSerialize = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isSerialize_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isSerialize = ptr_of_this_method->Value == 1;
            ((GOE.HotfixMonoAttribute)o).isSerialize = @isSerialize;
            return ptr_of_this_method;
        }



    }
}
