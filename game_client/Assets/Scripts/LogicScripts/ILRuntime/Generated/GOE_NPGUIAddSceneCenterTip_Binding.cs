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
    unsafe class GOE_NPGUIAddSceneCenterTip_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE.NPGUIAddSceneCenterTip);
            args = new Type[]{};
            method = type.GetMethod("get_instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_instance_0);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("showTextInfo", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showTextInfo_1);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("showTransTextInfo", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showTransTextInfo_2);
            args = new Type[]{typeof(System.String), typeof(System.Int64), typeof(System.Action<GOE.NPGGUIWndTextTip>), typeof(GOE.ETipDealerTagType)};
            method = type.GetMethod("showTextTip", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showTextTip_3);
            args = new Type[]{typeof(global::NPGTextureIndex), typeof(System.String), typeof(System.Int64), typeof(System.Action<GOE.NPGGUIWndIconTextTip>), typeof(GOE.ETipDealerTagType)};
            method = type.GetMethod("showIconTextTip", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showIconTextTip_4);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("showErrorInfo", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showErrorInfo_5);


        }


        static StackObject* get_instance_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = GOE.NPGUIAddSceneCenterTip.instance;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* showTextInfo_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_info = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.NPGUIAddSceneCenterTip instance_of_this_method = (GOE.NPGUIAddSceneCenterTip)typeof(GOE.NPGUIAddSceneCenterTip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showTextInfo(@_info);

            return __ret;
        }

        static StackObject* showTransTextInfo_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_info = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.NPGUIAddSceneCenterTip instance_of_this_method = (GOE.NPGUIAddSceneCenterTip)typeof(GOE.NPGUIAddSceneCenterTip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showTransTextInfo(@_info);

            return __ret;
        }

        static StackObject* showTextTip_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE.ETipDealerTagType @_tag = (GOE.ETipDealerTagType)typeof(GOE.ETipDealerTagType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<GOE.NPGGUIWndTextTip> @_onPop = (System.Action<GOE.NPGGUIWndTextTip>)typeof(System.Action<GOE.NPGGUIWndTextTip>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @_tipId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @_text = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            GOE.NPGUIAddSceneCenterTip instance_of_this_method = (GOE.NPGUIAddSceneCenterTip)typeof(GOE.NPGUIAddSceneCenterTip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showTextTip(@_text, @_tipId, @_onPop, @_tag);

            return __ret;
        }

        static StackObject* showIconTextTip_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE.ETipDealerTagType @_tag = (GOE.ETipDealerTagType)typeof(GOE.ETipDealerTagType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<GOE.NPGGUIWndIconTextTip> @_onPop = (System.Action<GOE.NPGGUIWndIconTextTip>)typeof(System.Action<GOE.NPGGUIWndIconTextTip>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @_tipId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @_text = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::NPGTextureIndex @_icon = (global::NPGTextureIndex)typeof(global::NPGTextureIndex).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            GOE.NPGUIAddSceneCenterTip instance_of_this_method = (GOE.NPGUIAddSceneCenterTip)typeof(GOE.NPGUIAddSceneCenterTip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showIconTextTip(@_icon, @_text, @_tipId, @_onPop, @_tag);

            return __ret;
        }

        static StackObject* showErrorInfo_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_errCode = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.NPGUIAddSceneCenterTip instance_of_this_method = (GOE.NPGUIAddSceneCenterTip)typeof(GOE.NPGUIAddSceneCenterTip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.showErrorInfo(@_errCode);

            return __ret;
        }



    }
}
