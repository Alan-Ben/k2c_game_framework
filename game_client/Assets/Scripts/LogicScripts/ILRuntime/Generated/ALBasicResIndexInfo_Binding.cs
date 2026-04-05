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
    unsafe class ALBasicResIndexInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ALBasicResIndexInfo);
            args = new Type[]{typeof(System.String), typeof(System.String)};
            method = type.GetMethod("readIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, readIndex_0);

            field = type.GetField("mainId", flag);
            app.RegisterCLRFieldGetter(field, get_mainId_0);
            app.RegisterCLRFieldSetter(field, set_mainId_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_mainId_0, AssignFromStack_mainId_0);
            field = type.GetField("subId", flag);
            app.RegisterCLRFieldGetter(field, get_subId_1);
            app.RegisterCLRFieldSetter(field, set_subId_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_subId_1, AssignFromStack_subId_1);


        }


        static StackObject* readIndex_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_columnName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @_str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::ALBasicResIndexInfo instance_of_this_method = (global::ALBasicResIndexInfo)typeof(global::ALBasicResIndexInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.readIndex(@_str, @_columnName);

            return __ret;
        }


        static object get_mainId_0(ref object o)
        {
            return ((global::ALBasicResIndexInfo)o).mainId;
        }

        static StackObject* CopyToStack_mainId_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::ALBasicResIndexInfo)o).mainId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_mainId_0(ref object o, object v)
        {
            ((global::ALBasicResIndexInfo)o).mainId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_mainId_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @mainId = ptr_of_this_method->Value;
            ((global::ALBasicResIndexInfo)o).mainId = @mainId;
            return ptr_of_this_method;
        }

        static object get_subId_1(ref object o)
        {
            return ((global::ALBasicResIndexInfo)o).subId;
        }

        static StackObject* CopyToStack_subId_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::ALBasicResIndexInfo)o).subId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_subId_1(ref object o, object v)
        {
            ((global::ALBasicResIndexInfo)o).subId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_subId_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @subId = ptr_of_this_method->Value;
            ((global::ALBasicResIndexInfo)o).subId = @subId;
            return ptr_of_this_method;
        }



    }
}
