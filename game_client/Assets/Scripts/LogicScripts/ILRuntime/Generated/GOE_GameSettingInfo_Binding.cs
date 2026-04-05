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
    unsafe class GOE_GameSettingInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.GameSettingInfo);

            field = type.GetField("printProtocol", flag);
            app.RegisterCLRFieldGetter(field, get_printProtocol_0);
            app.RegisterCLRFieldSetter(field, set_printProtocol_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_printProtocol_0, AssignFromStack_printProtocol_0);
            field = type.GetField("protocolPrintMinSize", flag);
            app.RegisterCLRFieldGetter(field, get_protocolPrintMinSize_1);
            app.RegisterCLRFieldSetter(field, set_protocolPrintMinSize_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_protocolPrintMinSize_1, AssignFromStack_protocolPrintMinSize_1);
            field = type.GetField("protocolErrorMinSize", flag);
            app.RegisterCLRFieldGetter(field, get_protocolErrorMinSize_2);
            app.RegisterCLRFieldSetter(field, set_protocolErrorMinSize_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_protocolErrorMinSize_2, AssignFromStack_protocolErrorMinSize_2);


        }



        static object get_printProtocol_0(ref object o)
        {
            return ((GOE.GameSettingInfo)o).printProtocol;
        }

        static StackObject* CopyToStack_printProtocol_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GameSettingInfo)o).printProtocol;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_printProtocol_0(ref object o, object v)
        {
            ((GOE.GameSettingInfo)o).printProtocol = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_printProtocol_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @printProtocol = ptr_of_this_method->Value == 1;
            ((GOE.GameSettingInfo)o).printProtocol = @printProtocol;
            return ptr_of_this_method;
        }

        static object get_protocolPrintMinSize_1(ref object o)
        {
            return ((GOE.GameSettingInfo)o).protocolPrintMinSize;
        }

        static StackObject* CopyToStack_protocolPrintMinSize_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GameSettingInfo)o).protocolPrintMinSize;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_protocolPrintMinSize_1(ref object o, object v)
        {
            ((GOE.GameSettingInfo)o).protocolPrintMinSize = (System.Int32)v;
        }

        static StackObject* AssignFromStack_protocolPrintMinSize_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @protocolPrintMinSize = ptr_of_this_method->Value;
            ((GOE.GameSettingInfo)o).protocolPrintMinSize = @protocolPrintMinSize;
            return ptr_of_this_method;
        }

        static object get_protocolErrorMinSize_2(ref object o)
        {
            return ((GOE.GameSettingInfo)o).protocolErrorMinSize;
        }

        static StackObject* CopyToStack_protocolErrorMinSize_2(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GameSettingInfo)o).protocolErrorMinSize;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_protocolErrorMinSize_2(ref object o, object v)
        {
            ((GOE.GameSettingInfo)o).protocolErrorMinSize = (System.Int32)v;
        }

        static StackObject* AssignFromStack_protocolErrorMinSize_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @protocolErrorMinSize = ptr_of_this_method->Value;
            ((GOE.GameSettingInfo)o).protocolErrorMinSize = @protocolErrorMinSize;
            return ptr_of_this_method;
        }



    }
}
