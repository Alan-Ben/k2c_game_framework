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
    unsafe class GOE__AGGUIHotfixBasicGridWnd_1_GOE__AGGUIHotfixBasicGridItemWndAdapter_Binding_Adapter_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("setItemCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setItemCount_0);
            args = new Type[]{};
            method = type.GetMethod("forceRefreshAllItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, forceRefreshAllItem_1);
            args = new Type[]{typeof(ALPackage._AALUGUIGridBarController)};
            method = type.GetMethod("removeBar", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, removeBar_2);
            args = new Type[]{typeof(ALPackage._AALUGUIGridBarController)};
            method = type.GetMethod("addBar", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, addBar_3);


        }


        static StackObject* setItemCount_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_count = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter> instance_of_this_method = (GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>)typeof(GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setItemCount(@_count);

            return __ret;
        }

        static StackObject* forceRefreshAllItem_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter> instance_of_this_method = (GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>)typeof(GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.forceRefreshAllItem();

            return __ret;
        }

        static StackObject* removeBar_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._AALUGUIGridBarController @_barController = (ALPackage._AALUGUIGridBarController)typeof(ALPackage._AALUGUIGridBarController).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter> instance_of_this_method = (GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>)typeof(GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.removeBar(@_barController);

            return __ret;
        }

        static StackObject* addBar_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALPackage._AALUGUIGridBarController @_barController = (ALPackage._AALUGUIGridBarController)typeof(ALPackage._AALUGUIGridBarController).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter> instance_of_this_method = (GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>)typeof(GOE._AGGUIHotfixBasicGridWnd<GOE._AGGUIHotfixBasicGridItemWndAdapter.Adapter>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.addBar(@_barController);

            return __ret;
        }



    }
}
