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
    unsafe class NPCommonAssetPathInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NPCommonAssetPathInfo);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("readFromStr", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, readFromStr_0);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("readFromUiResId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, readFromUiResId_1);
            args = new Type[]{typeof(global::NPCommonAssetPathInfo), typeof(global::NPCommonAssetPathInfo)};
            method = type.GetMethod("op_Equality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Equality_2);
            args = new Type[]{};
            method = type.GetMethod("get_enable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_enable_3);
            args = new Type[]{typeof(global::NPCommonAssetPathInfo), typeof(global::NPCommonAssetPathInfo)};
            method = type.GetMethod("op_Inequality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, op_Inequality_4);

            field = type.GetField("asset_path", flag);
            app.RegisterCLRFieldGetter(field, get_asset_path_0);
            app.RegisterCLRFieldSetter(field, set_asset_path_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_asset_path_0, AssignFromStack_asset_path_0);
            field = type.GetField("obj_name", flag);
            app.RegisterCLRFieldGetter(field, get_obj_name_1);
            app.RegisterCLRFieldSetter(field, set_obj_name_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_obj_name_1, AssignFromStack_obj_name_1);


        }


        static StackObject* readFromStr_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NPCommonAssetPathInfo.readFromStr(@_str);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* readFromUiResId_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @_id = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = global::NPCommonAssetPathInfo.readFromUiResId(@_id);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* op_Equality_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::NPCommonAssetPathInfo @b = (global::NPCommonAssetPathInfo)typeof(global::NPCommonAssetPathInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::NPCommonAssetPathInfo @a = (global::NPCommonAssetPathInfo)typeof(global::NPCommonAssetPathInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = a == b;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_enable_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::NPCommonAssetPathInfo instance_of_this_method = (global::NPCommonAssetPathInfo)typeof(global::NPCommonAssetPathInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.enable;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* op_Inequality_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::NPCommonAssetPathInfo @b = (global::NPCommonAssetPathInfo)typeof(global::NPCommonAssetPathInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::NPCommonAssetPathInfo @a = (global::NPCommonAssetPathInfo)typeof(global::NPCommonAssetPathInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = a != b;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_asset_path_0(ref object o)
        {
            return ((global::NPCommonAssetPathInfo)o).asset_path;
        }

        static StackObject* CopyToStack_asset_path_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::NPCommonAssetPathInfo)o).asset_path;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_asset_path_0(ref object o, object v)
        {
            ((global::NPCommonAssetPathInfo)o).asset_path = (System.String)v;
        }

        static StackObject* AssignFromStack_asset_path_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @asset_path = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::NPCommonAssetPathInfo)o).asset_path = @asset_path;
            return ptr_of_this_method;
        }

        static object get_obj_name_1(ref object o)
        {
            return ((global::NPCommonAssetPathInfo)o).obj_name;
        }

        static StackObject* CopyToStack_obj_name_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::NPCommonAssetPathInfo)o).obj_name;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_obj_name_1(ref object o, object v)
        {
            ((global::NPCommonAssetPathInfo)o).obj_name = (System.String)v;
        }

        static StackObject* AssignFromStack_obj_name_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @obj_name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::NPCommonAssetPathInfo)o).obj_name = @obj_name;
            return ptr_of_this_method;
        }



    }
}
