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
    unsafe class GOE__BaseSfxObj_1_NPSfxMono_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE._BaseSfxObj<global::NPSfxMono>);
            args = new Type[]{};
            method = type.GetMethod("forceDiscard", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, forceDiscard_0);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("regLoadDoneDelegate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, regLoadDoneDelegate_1);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("regPlayCompleteDelegate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, regPlayCompleteDelegate_2);
            args = new Type[]{};
            method = type.GetMethod("get_sfxSerialize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_sfxSerialize_3);
            args = new Type[]{typeof(UnityEngine.Vector3)};
            method = type.GetMethod("setLocalPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setLocalPos_4);
            args = new Type[]{typeof(UnityEngine.Quaternion)};
            method = type.GetMethod("setLocalRotation", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setLocalRotation_5);
            args = new Type[]{};
            method = type.GetMethod("get_sfxTrans", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_sfxTrans_6);


        }


        static StackObject* forceDiscard_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._BaseSfxObj<global::NPSfxMono> instance_of_this_method = (GOE._BaseSfxObj<global::NPSfxMono>)typeof(GOE._BaseSfxObj<global::NPSfxMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.forceDiscard();

            return __ret;
        }

        static StackObject* regLoadDoneDelegate_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE._BaseSfxObj<global::NPSfxMono> instance_of_this_method = (GOE._BaseSfxObj<global::NPSfxMono>)typeof(GOE._BaseSfxObj<global::NPSfxMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.regLoadDoneDelegate(@_delegate);

            return __ret;
        }

        static StackObject* regPlayCompleteDelegate_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_delegate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE._BaseSfxObj<global::NPSfxMono> instance_of_this_method = (GOE._BaseSfxObj<global::NPSfxMono>)typeof(GOE._BaseSfxObj<global::NPSfxMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.regPlayCompleteDelegate(@_delegate);

            return __ret;
        }

        static StackObject* get_sfxSerialize_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._BaseSfxObj<global::NPSfxMono> instance_of_this_method = (GOE._BaseSfxObj<global::NPSfxMono>)typeof(GOE._BaseSfxObj<global::NPSfxMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.sfxSerialize;

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* setLocalPos_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @_localPos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE._BaseSfxObj<global::NPSfxMono> instance_of_this_method = (GOE._BaseSfxObj<global::NPSfxMono>)typeof(GOE._BaseSfxObj<global::NPSfxMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setLocalPos(@_localPos);

            return __ret;
        }

        static StackObject* setLocalRotation_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Quaternion @_pos = (UnityEngine.Quaternion)typeof(UnityEngine.Quaternion).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE._BaseSfxObj<global::NPSfxMono> instance_of_this_method = (GOE._BaseSfxObj<global::NPSfxMono>)typeof(GOE._BaseSfxObj<global::NPSfxMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setLocalRotation(@_pos);

            return __ret;
        }

        static StackObject* get_sfxTrans_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            GOE._BaseSfxObj<global::NPSfxMono> instance_of_this_method = (GOE._BaseSfxObj<global::NPSfxMono>)typeof(GOE._BaseSfxObj<global::NPSfxMono>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.sfxTrans;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
