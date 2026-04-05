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
    unsafe class GOE_CommonActivityComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE.CommonActivityComponent);
            args = new Type[]{typeof(CommonEnum.ECommonActivityType)};
            method = type.GetMethod("getValidActivityInfoByType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getValidActivityInfoByType_0);
            args = new Type[]{typeof(CommonEnum.ECommonActivityType)};
            method = type.GetMethod("getValidActivityListInfoByType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getValidActivityListInfoByType_1);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("getValidActivityInfoByActivityId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getValidActivityInfoByActivityId_2);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("getActivityInfoByInstanceId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getActivityInfoByInstanceId_3);
            args = new Type[]{typeof(System.Int64), typeof(System.Int64), typeof(System.Boolean), typeof(System.Action<GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList>)};
            method = type.GetMethod("reqActivityRankBaseList", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, reqActivityRankBaseList_4);
            args = new Type[]{typeof(System.Int64), typeof(System.Int64), typeof(System.Int64), typeof(System.Boolean), typeof(System.Action<GS2GC.p017_ActivityOp.GS2GC_017_005_RetActivityRankBaseInfoByKey>)};
            method = type.GetMethod("reqActivityRankBaseInfoByKey", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, reqActivityRankBaseInfoByKey_5);
            args = new Type[]{typeof(System.Int64), typeof(System.Int64), typeof(System.Int32), typeof(System.Boolean), typeof(System.Action<GS2GC.p017_ActivityOp.GS2GC_017_004_RetActivityRankBaseInfoByRank>)};
            method = type.GetMethod("reqActivityRankBaseInfoByRank", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, reqActivityRankBaseInfoByRank_6);


        }


        static StackObject* getValidActivityInfoByType_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            CommonEnum.ECommonActivityType @_type = (CommonEnum.ECommonActivityType)typeof(CommonEnum.ECommonActivityType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.CommonActivityComponent instance_of_this_method = (GOE.CommonActivityComponent)typeof(GOE.CommonActivityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getValidActivityInfoByType(@_type);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getValidActivityListInfoByType_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            CommonEnum.ECommonActivityType @_type = (CommonEnum.ECommonActivityType)typeof(CommonEnum.ECommonActivityType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.CommonActivityComponent instance_of_this_method = (GOE.CommonActivityComponent)typeof(GOE.CommonActivityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getValidActivityListInfoByType(@_type);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getValidActivityInfoByActivityId_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @_activityId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.CommonActivityComponent instance_of_this_method = (GOE.CommonActivityComponent)typeof(GOE.CommonActivityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getValidActivityInfoByActivityId(@_activityId);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getActivityInfoByInstanceId_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @_instanceId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.CommonActivityComponent instance_of_this_method = (GOE.CommonActivityComponent)typeof(GOE.CommonActivityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getActivityInfoByInstanceId(@_instanceId);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* reqActivityRankBaseList_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList> @_callback = (System.Action<GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList>)typeof(System.Action<GS2GC.p017_ActivityOp.GS2GC_017_003_RetActivityRankBaseList>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @_isCross = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @_rankId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int64 @_instanceId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            GOE.CommonActivityComponent instance_of_this_method = (GOE.CommonActivityComponent)typeof(GOE.CommonActivityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.reqActivityRankBaseList(@_instanceId, @_rankId, @_isCross, @_callback);

            return __ret;
        }

        static StackObject* reqActivityRankBaseInfoByKey_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<GS2GC.p017_ActivityOp.GS2GC_017_005_RetActivityRankBaseInfoByKey> @_callback = (System.Action<GS2GC.p017_ActivityOp.GS2GC_017_005_RetActivityRankBaseInfoByKey>)typeof(System.Action<GS2GC.p017_ActivityOp.GS2GC_017_005_RetActivityRankBaseInfoByKey>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @_isCross = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @_key = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int64 @_rankId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Int64 @_instanceId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            GOE.CommonActivityComponent instance_of_this_method = (GOE.CommonActivityComponent)typeof(GOE.CommonActivityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.reqActivityRankBaseInfoByKey(@_instanceId, @_rankId, @_key, @_isCross, @_callback);

            return __ret;
        }

        static StackObject* reqActivityRankBaseInfoByRank_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<GS2GC.p017_ActivityOp.GS2GC_017_004_RetActivityRankBaseInfoByRank> @_callback = (System.Action<GS2GC.p017_ActivityOp.GS2GC_017_004_RetActivityRankBaseInfoByRank>)typeof(System.Action<GS2GC.p017_ActivityOp.GS2GC_017_004_RetActivityRankBaseInfoByRank>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @_isCross = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @_rank = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int64 @_rankId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Int64 @_instanceId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            GOE.CommonActivityComponent instance_of_this_method = (GOE.CommonActivityComponent)typeof(GOE.CommonActivityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.reqActivityRankBaseInfoByRank(@_instanceId, @_rankId, @_rank, @_isCross, @_callback);

            return __ret;
        }



    }
}
