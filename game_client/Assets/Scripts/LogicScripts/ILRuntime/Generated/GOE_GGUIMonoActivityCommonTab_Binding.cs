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
    unsafe class GOE_GGUIMonoActivityCommonTab_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.GGUIMonoActivityCommonTab);

            field = type.GetField("monoTab", flag);
            app.RegisterCLRFieldGetter(field, get_monoTab_0);
            app.RegisterCLRFieldSetter(field, set_monoTab_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_monoTab_0, AssignFromStack_monoTab_0);
            field = type.GetField("tabTypeStr", flag);
            app.RegisterCLRFieldGetter(field, get_tabTypeStr_1);
            app.RegisterCLRFieldSetter(field, set_tabTypeStr_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_tabTypeStr_1, AssignFromStack_tabTypeStr_1);
            field = type.GetField("tabSubWndAssetId", flag);
            app.RegisterCLRFieldGetter(field, get_tabSubWndAssetId_2);
            app.RegisterCLRFieldSetter(field, set_tabSubWndAssetId_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_tabSubWndAssetId_2, AssignFromStack_tabSubWndAssetId_2);


        }



        static object get_monoTab_0(ref object o)
        {
            return ((GOE.GGUIMonoActivityCommonTab)o).monoTab;
        }

        static StackObject* CopyToStack_monoTab_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GGUIMonoActivityCommonTab)o).monoTab;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_monoTab_0(ref object o, object v)
        {
            ((GOE.GGUIMonoActivityCommonTab)o).monoTab = (global::NPGGUIMonoCommonTab)v;
        }

        static StackObject* AssignFromStack_monoTab_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::NPGGUIMonoCommonTab @monoTab = (global::NPGGUIMonoCommonTab)typeof(global::NPGGUIMonoCommonTab).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GGUIMonoActivityCommonTab)o).monoTab = @monoTab;
            return ptr_of_this_method;
        }

        static object get_tabTypeStr_1(ref object o)
        {
            return ((GOE.GGUIMonoActivityCommonTab)o).tabTypeStr;
        }

        static StackObject* CopyToStack_tabTypeStr_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GGUIMonoActivityCommonTab)o).tabTypeStr;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_tabTypeStr_1(ref object o, object v)
        {
            ((GOE.GGUIMonoActivityCommonTab)o).tabTypeStr = (System.String)v;
        }

        static StackObject* AssignFromStack_tabTypeStr_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @tabTypeStr = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GGUIMonoActivityCommonTab)o).tabTypeStr = @tabTypeStr;
            return ptr_of_this_method;
        }

        static object get_tabSubWndAssetId_2(ref object o)
        {
            return ((GOE.GGUIMonoActivityCommonTab)o).tabSubWndAssetId;
        }

        static StackObject* CopyToStack_tabSubWndAssetId_2(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GGUIMonoActivityCommonTab)o).tabSubWndAssetId;
            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_tabSubWndAssetId_2(ref object o, object v)
        {
            ((GOE.GGUIMonoActivityCommonTab)o).tabSubWndAssetId = (System.Int64)v;
        }

        static StackObject* AssignFromStack_tabSubWndAssetId_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64 @tabSubWndAssetId = *(long*)&ptr_of_this_method->Value;
            ((GOE.GGUIMonoActivityCommonTab)o).tabSubWndAssetId = @tabSubWndAssetId;
            return ptr_of_this_method;
        }



    }
}
