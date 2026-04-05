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
    unsafe class GOE_ActivityCenterRefObj_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.ActivityCenterRefObj);

            field = type.GetField("activity_id", flag);
            app.RegisterCLRFieldGetter(field, get_activity_id_0);
            app.RegisterCLRFieldSetter(field, set_activity_id_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_activity_id_0, AssignFromStack_activity_id_0);


        }



        static object get_activity_id_0(ref object o)
        {
            return ((GOE.ActivityCenterRefObj)o).activity_id;
        }

        static StackObject* CopyToStack_activity_id_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.ActivityCenterRefObj)o).activity_id;
            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_activity_id_0(ref object o, object v)
        {
            ((GOE.ActivityCenterRefObj)o).activity_id = (System.Int64)v;
        }

        static StackObject* AssignFromStack_activity_id_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64 @activity_id = *(long*)&ptr_of_this_method->Value;
            ((GOE.ActivityCenterRefObj)o).activity_id = @activity_id;
            return ptr_of_this_method;
        }



    }
}
