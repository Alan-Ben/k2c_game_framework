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
    unsafe class GOE_GActivityMainRefObj_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.GActivityMainRefObj);

            field = type.GetField("type_id", flag);
            app.RegisterCLRFieldGetter(field, get_type_id_0);
            app.RegisterCLRFieldSetter(field, set_type_id_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_type_id_0, AssignFromStack_type_id_0);
            field = type.GetField("activity_id", flag);
            app.RegisterCLRFieldGetter(field, get_activity_id_1);
            app.RegisterCLRFieldSetter(field, set_activity_id_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_activity_id_1, AssignFromStack_activity_id_1);
            field = type.GetField("rank_id_list", flag);
            app.RegisterCLRFieldGetter(field, get_rank_id_list_2);
            app.RegisterCLRFieldSetter(field, set_rank_id_list_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_rank_id_list_2, AssignFromStack_rank_id_list_2);


        }



        static object get_type_id_0(ref object o)
        {
            return ((GOE.GActivityMainRefObj)o).type_id;
        }

        static StackObject* CopyToStack_type_id_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GActivityMainRefObj)o).type_id;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_type_id_0(ref object o, object v)
        {
            ((GOE.GActivityMainRefObj)o).type_id = (CommonEnum.ECommonActivityType)v;
        }

        static StackObject* AssignFromStack_type_id_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            CommonEnum.ECommonActivityType @type_id = (CommonEnum.ECommonActivityType)typeof(CommonEnum.ECommonActivityType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            ((GOE.GActivityMainRefObj)o).type_id = @type_id;
            return ptr_of_this_method;
        }

        static object get_activity_id_1(ref object o)
        {
            return ((GOE.GActivityMainRefObj)o).activity_id;
        }

        static StackObject* CopyToStack_activity_id_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GActivityMainRefObj)o).activity_id;
            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_activity_id_1(ref object o, object v)
        {
            ((GOE.GActivityMainRefObj)o).activity_id = (System.Int64)v;
        }

        static StackObject* AssignFromStack_activity_id_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64 @activity_id = *(long*)&ptr_of_this_method->Value;
            ((GOE.GActivityMainRefObj)o).activity_id = @activity_id;
            return ptr_of_this_method;
        }

        static object get_rank_id_list_2(ref object o)
        {
            return ((GOE.GActivityMainRefObj)o).rank_id_list;
        }

        static StackObject* CopyToStack_rank_id_list_2(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GActivityMainRefObj)o).rank_id_list;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_rank_id_list_2(ref object o, object v)
        {
            ((GOE.GActivityMainRefObj)o).rank_id_list = (System.Collections.Generic.List<System.Int64>)v;
        }

        static StackObject* AssignFromStack_rank_id_list_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.Int64> @rank_id_list = (System.Collections.Generic.List<System.Int64>)typeof(System.Collections.Generic.List<System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GActivityMainRefObj)o).rank_id_list = @rank_id_list;
            return ptr_of_this_method;
        }



    }
}
