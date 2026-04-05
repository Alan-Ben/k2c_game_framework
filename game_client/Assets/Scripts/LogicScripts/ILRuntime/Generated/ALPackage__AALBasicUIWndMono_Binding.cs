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
    unsafe class ALPackage__AALBasicUIWndMono_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ALPackage._AALBasicUIWndMono);

            field = type.GetField("wndAnimation", flag);
            app.RegisterCLRFieldGetter(field, get_wndAnimation_0);
            app.RegisterCLRFieldSetter(field, set_wndAnimation_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_wndAnimation_0, AssignFromStack_wndAnimation_0);
            field = type.GetField("showAniName", flag);
            app.RegisterCLRFieldGetter(field, get_showAniName_1);
            app.RegisterCLRFieldSetter(field, set_showAniName_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_showAniName_1, AssignFromStack_showAniName_1);


        }



        static object get_wndAnimation_0(ref object o)
        {
            return ((ALPackage._AALBasicUIWndMono)o).wndAnimation;
        }

        static StackObject* CopyToStack_wndAnimation_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((ALPackage._AALBasicUIWndMono)o).wndAnimation;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_wndAnimation_0(ref object o, object v)
        {
            ((ALPackage._AALBasicUIWndMono)o).wndAnimation = (UnityEngine.Animation)v;
        }

        static StackObject* AssignFromStack_wndAnimation_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Animation @wndAnimation = (UnityEngine.Animation)typeof(UnityEngine.Animation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((ALPackage._AALBasicUIWndMono)o).wndAnimation = @wndAnimation;
            return ptr_of_this_method;
        }

        static object get_showAniName_1(ref object o)
        {
            return ((ALPackage._AALBasicUIWndMono)o).showAniName;
        }

        static StackObject* CopyToStack_showAniName_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((ALPackage._AALBasicUIWndMono)o).showAniName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_showAniName_1(ref object o, object v)
        {
            ((ALPackage._AALBasicUIWndMono)o).showAniName = (System.String)v;
        }

        static StackObject* AssignFromStack_showAniName_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @showAniName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((ALPackage._AALBasicUIWndMono)o).showAniName = @showAniName;
            return ptr_of_this_method;
        }



    }
}
