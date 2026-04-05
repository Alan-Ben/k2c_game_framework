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
    unsafe class GOE_GGUIHotfixCommonMono_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.GGUIHotfixCommonMono);

            field = type.GetField("monoSkin", flag);
            app.RegisterCLRFieldGetter(field, get_monoSkin_0);
            app.RegisterCLRFieldSetter(field, set_monoSkin_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_monoSkin_0, AssignFromStack_monoSkin_0);


        }



        static object get_monoSkin_0(ref object o)
        {
            return ((GOE.GGUIHotfixCommonMono)o).monoSkin;
        }

        static StackObject* CopyToStack_monoSkin_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GGUIHotfixCommonMono)o).monoSkin;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_monoSkin_0(ref object o, object v)
        {
            ((GOE.GGUIHotfixCommonMono)o).monoSkin = (GOE.MonoSkin)v;
        }

        static StackObject* AssignFromStack_monoSkin_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            GOE.MonoSkin @monoSkin = (GOE.MonoSkin)typeof(GOE.MonoSkin).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GGUIHotfixCommonMono)o).monoSkin = @monoSkin;
            return ptr_of_this_method;
        }



    }
}
