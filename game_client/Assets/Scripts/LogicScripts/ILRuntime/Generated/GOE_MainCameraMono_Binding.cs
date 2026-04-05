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
    unsafe class GOE_MainCameraMono_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.MainCameraMono);
            args = new Type[]{};
            method = type.GetMethod("get_selfInstance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_selfInstance_0);
            args = new Type[]{};
            method = type.GetMethod("openAllInputMask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, openAllInputMask_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("closeAllInputMask", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, closeAllInputMask_2);

            field = type.GetField("gameSetting", flag);
            app.RegisterCLRFieldGetter(field, get_gameSetting_0);
            app.RegisterCLRFieldSetter(field, set_gameSetting_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_gameSetting_0, AssignFromStack_gameSetting_0);


        }


        static StackObject* get_selfInstance_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = GOE.MainCameraMono.selfInstance;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* openAllInputMask_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE.MainCameraMono instance_of_this_method = (GOE.MainCameraMono)typeof(GOE.MainCameraMono).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.openAllInputMask();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* closeAllInputMask_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_serialize = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.MainCameraMono instance_of_this_method = (GOE.MainCameraMono)typeof(GOE.MainCameraMono).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.closeAllInputMask(@_serialize);

            return __ret;
        }


        static object get_gameSetting_0(ref object o)
        {
            return ((GOE.MainCameraMono)o).gameSetting;
        }

        static StackObject* CopyToStack_gameSetting_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.MainCameraMono)o).gameSetting;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_gameSetting_0(ref object o, object v)
        {
            ((GOE.MainCameraMono)o).gameSetting = (GOE.GameSettingInfo)v;
        }

        static StackObject* AssignFromStack_gameSetting_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            GOE.GameSettingInfo @gameSetting = (GOE.GameSettingInfo)typeof(GOE.GameSettingInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.MainCameraMono)o).gameSetting = @gameSetting;
            return ptr_of_this_method;
        }



    }
}
