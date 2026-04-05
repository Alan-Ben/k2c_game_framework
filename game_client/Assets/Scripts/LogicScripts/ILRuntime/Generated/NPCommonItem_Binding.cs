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
    unsafe class NPCommonItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NPCommonItem);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("readFromStr", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, readFromStr_0);
            args = new Type[]{typeof(global::NPCommonItem), typeof(global::NPCommonItem)};
            method = type.GetMethod("op_Inequality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Inequality_1);
            args = new Type[]{typeof(global::NPCommonItem), typeof(global::NPCommonItem)};
            method = type.GetMethod("op_Equality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Equality_2);

            field = type.GetField("itemType", flag);
            app.RegisterCLRFieldGetter(field, get_itemType_0);
            app.RegisterCLRFieldSetter(field, set_itemType_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_itemType_0, AssignFromStack_itemType_0);
            field = type.GetField("itemId", flag);
            app.RegisterCLRFieldGetter(field, get_itemId_1);
            app.RegisterCLRFieldSetter(field, set_itemId_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_itemId_1, AssignFromStack_itemId_1);


        }


        static StackObject* readFromStr_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NPCommonItem.readFromStr(@_str);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* op_Inequality_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::NPCommonItem @_b = (global::NPCommonItem)typeof(global::NPCommonItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::NPCommonItem @_a = (global::NPCommonItem)typeof(global::NPCommonItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = _a != _b;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* op_Equality_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::NPCommonItem @_b = (global::NPCommonItem)typeof(global::NPCommonItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::NPCommonItem @_a = (global::NPCommonItem)typeof(global::NPCommonItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = _a == _b;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_itemType_0(ref object o)
        {
            return ((global::NPCommonItem)o).itemType;
        }

        static StackObject* CopyToStack_itemType_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::NPCommonItem)o).itemType;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_itemType_0(ref object o, object v)
        {
            ((global::NPCommonItem)o).itemType = (NPEnum.ENPItemType)v;
        }

        static StackObject* AssignFromStack_itemType_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            NPEnum.ENPItemType @itemType = (NPEnum.ENPItemType)typeof(NPEnum.ENPItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            ((global::NPCommonItem)o).itemType = @itemType;
            return ptr_of_this_method;
        }

        static object get_itemId_1(ref object o)
        {
            return ((global::NPCommonItem)o).itemId;
        }

        static StackObject* CopyToStack_itemId_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::NPCommonItem)o).itemId;
            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_itemId_1(ref object o, object v)
        {
            ((global::NPCommonItem)o).itemId = (System.Int64)v;
        }

        static StackObject* AssignFromStack_itemId_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64 @itemId = *(long*)&ptr_of_this_method->Value;
            ((global::NPCommonItem)o).itemId = @itemId;
            return ptr_of_this_method;
        }



    }
}
