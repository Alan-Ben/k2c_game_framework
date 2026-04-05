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
    unsafe class NPSORewardRefObj_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NPSORewardRefObj);

            field = type.GetField("show_item_list", flag);
            app.RegisterCLRFieldGetter(field, get_show_item_list_0);
            app.RegisterCLRFieldSetter(field, set_show_item_list_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_show_item_list_0, AssignFromStack_show_item_list_0);
            field = type.GetField("show_pro_list", flag);
            app.RegisterCLRFieldGetter(field, get_show_pro_list_1);
            app.RegisterCLRFieldSetter(field, set_show_pro_list_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_show_pro_list_1, AssignFromStack_show_pro_list_1);


        }



        static object get_show_item_list_0(ref object o)
        {
            return ((global::NPSORewardRefObj)o).show_item_list;
        }

        static StackObject* CopyToStack_show_item_list_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::NPSORewardRefObj)o).show_item_list;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_show_item_list_0(ref object o, object v)
        {
            ((global::NPSORewardRefObj)o).show_item_list = (System.Collections.Generic.List<global::NPCommonCostItem>)v;
        }

        static StackObject* AssignFromStack_show_item_list_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::NPCommonCostItem> @show_item_list = (System.Collections.Generic.List<global::NPCommonCostItem>)typeof(System.Collections.Generic.List<global::NPCommonCostItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::NPSORewardRefObj)o).show_item_list = @show_item_list;
            return ptr_of_this_method;
        }

        static object get_show_pro_list_1(ref object o)
        {
            return ((global::NPSORewardRefObj)o).show_pro_list;
        }

        static StackObject* CopyToStack_show_pro_list_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::NPSORewardRefObj)o).show_pro_list;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_show_pro_list_1(ref object o, object v)
        {
            ((global::NPSORewardRefObj)o).show_pro_list = (System.Collections.Generic.List<System.Int32>)v;
        }

        static StackObject* AssignFromStack_show_pro_list_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.Int32> @show_pro_list = (System.Collections.Generic.List<System.Int32>)typeof(System.Collections.Generic.List<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::NPSORewardRefObj)o).show_pro_list = @show_pro_list;
            return ptr_of_this_method;
        }



    }
}
