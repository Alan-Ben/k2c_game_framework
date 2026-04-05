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
    unsafe class GOE_GRefdataCoreMgr_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GOE.GRefdataCoreMgr);
            args = new Type[]{};
            method = type.GetMethod("get_instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_instance_0);
            args = new Type[]{typeof(NPEnum.ENPItemType), typeof(NPEnum.EQuality)};
            method = type.GetMethod("getQuality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getQuality_1);

            field = type.GetField("activityCenterRefCore", flag);
            app.RegisterCLRFieldGetter(field, get_activityCenterRefCore_0);
            app.RegisterCLRFieldSetter(field, set_activityCenterRefCore_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_activityCenterRefCore_0, AssignFromStack_activityCenterRefCore_0);
            field = type.GetField("activityMainRefCore", flag);
            app.RegisterCLRFieldGetter(field, get_activityMainRefCore_1);
            app.RegisterCLRFieldSetter(field, set_activityMainRefCore_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_activityMainRefCore_1, AssignFromStack_activityMainRefCore_1);
            field = type.GetField("rewardMap", flag);
            app.RegisterCLRFieldGetter(field, get_rewardMap_2);
            app.RegisterCLRFieldSetter(field, set_rewardMap_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_rewardMap_2, AssignFromStack_rewardMap_2);
            field = type.GetField("sceneInfoRefCore", flag);
            app.RegisterCLRFieldGetter(field, get_sceneInfoRefCore_3);
            app.RegisterCLRFieldSetter(field, set_sceneInfoRefCore_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_sceneInfoRefCore_3, AssignFromStack_sceneInfoRefCore_3);
            field = type.GetField("npGeneral", flag);
            app.RegisterCLRFieldGetter(field, get_npGeneral_4);
            app.RegisterCLRFieldSetter(field, set_npGeneral_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_npGeneral_4, AssignFromStack_npGeneral_4);
            field = type.GetField("rankCommonRefCore", flag);
            app.RegisterCLRFieldGetter(field, get_rankCommonRefCore_5);
            app.RegisterCLRFieldSetter(field, set_rankCommonRefCore_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_rankCommonRefCore_5, AssignFromStack_rankCommonRefCore_5);
            field = type.GetField("tipMap", flag);
            app.RegisterCLRFieldGetter(field, get_tipMap_6);
            app.RegisterCLRFieldSetter(field, set_tipMap_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_tipMap_6, AssignFromStack_tipMap_6);


        }


        static StackObject* get_instance_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = GOE.GRefdataCoreMgr.instance;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getQuality_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            NPEnum.EQuality @_quality = (NPEnum.EQuality)typeof(NPEnum.EQuality).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            NPEnum.ENPItemType @_itemType = (NPEnum.ENPItemType)typeof(NPEnum.ENPItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            GOE.GRefdataCoreMgr instance_of_this_method = (GOE.GRefdataCoreMgr)typeof(GOE.GRefdataCoreMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getQuality(@_itemType, @_quality);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_activityCenterRefCore_0(ref object o)
        {
            return ((GOE.GRefdataCoreMgr)o).activityCenterRefCore;
        }

        static StackObject* CopyToStack_activityCenterRefCore_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GRefdataCoreMgr)o).activityCenterRefCore;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_activityCenterRefCore_0(ref object o, object v)
        {
            ((GOE.GRefdataCoreMgr)o).activityCenterRefCore = (ALPackage.ALBasicMapListRefCore<GOE.ActivityCenterRefObj>)v;
        }

        static StackObject* AssignFromStack_activityCenterRefCore_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ALPackage.ALBasicMapListRefCore<GOE.ActivityCenterRefObj> @activityCenterRefCore = (ALPackage.ALBasicMapListRefCore<GOE.ActivityCenterRefObj>)typeof(ALPackage.ALBasicMapListRefCore<GOE.ActivityCenterRefObj>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GRefdataCoreMgr)o).activityCenterRefCore = @activityCenterRefCore;
            return ptr_of_this_method;
        }

        static object get_activityMainRefCore_1(ref object o)
        {
            return ((GOE.GRefdataCoreMgr)o).activityMainRefCore;
        }

        static StackObject* CopyToStack_activityMainRefCore_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GRefdataCoreMgr)o).activityMainRefCore;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_activityMainRefCore_1(ref object o, object v)
        {
            ((GOE.GRefdataCoreMgr)o).activityMainRefCore = (ALPackage.ALBasicMapRefCore<GOE.GActivityMainRefObj>)v;
        }

        static StackObject* AssignFromStack_activityMainRefCore_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ALPackage.ALBasicMapRefCore<GOE.GActivityMainRefObj> @activityMainRefCore = (ALPackage.ALBasicMapRefCore<GOE.GActivityMainRefObj>)typeof(ALPackage.ALBasicMapRefCore<GOE.GActivityMainRefObj>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GRefdataCoreMgr)o).activityMainRefCore = @activityMainRefCore;
            return ptr_of_this_method;
        }

        static object get_rewardMap_2(ref object o)
        {
            return ((GOE.GRefdataCoreMgr)o).rewardMap;
        }

        static StackObject* CopyToStack_rewardMap_2(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GRefdataCoreMgr)o).rewardMap;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_rewardMap_2(ref object o, object v)
        {
            ((GOE.GRefdataCoreMgr)o).rewardMap = (ALPackage.ALBasicMapRefCore<global::NPSORewardRefObj>)v;
        }

        static StackObject* AssignFromStack_rewardMap_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ALPackage.ALBasicMapRefCore<global::NPSORewardRefObj> @rewardMap = (ALPackage.ALBasicMapRefCore<global::NPSORewardRefObj>)typeof(ALPackage.ALBasicMapRefCore<global::NPSORewardRefObj>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GRefdataCoreMgr)o).rewardMap = @rewardMap;
            return ptr_of_this_method;
        }

        static object get_sceneInfoRefCore_3(ref object o)
        {
            return ((GOE.GRefdataCoreMgr)o).sceneInfoRefCore;
        }

        static StackObject* CopyToStack_sceneInfoRefCore_3(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GRefdataCoreMgr)o).sceneInfoRefCore;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_sceneInfoRefCore_3(ref object o, object v)
        {
            ((GOE.GRefdataCoreMgr)o).sceneInfoRefCore = (ALPackage.ALBasicMapRefCore<global::SceneInfoRefObj>)v;
        }

        static StackObject* AssignFromStack_sceneInfoRefCore_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ALPackage.ALBasicMapRefCore<global::SceneInfoRefObj> @sceneInfoRefCore = (ALPackage.ALBasicMapRefCore<global::SceneInfoRefObj>)typeof(ALPackage.ALBasicMapRefCore<global::SceneInfoRefObj>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GRefdataCoreMgr)o).sceneInfoRefCore = @sceneInfoRefCore;
            return ptr_of_this_method;
        }

        static object get_npGeneral_4(ref object o)
        {
            return ((GOE.GRefdataCoreMgr)o).npGeneral;
        }

        static StackObject* CopyToStack_npGeneral_4(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GRefdataCoreMgr)o).npGeneral;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_npGeneral_4(ref object o, object v)
        {
            ((GOE.GRefdataCoreMgr)o).npGeneral = (GOE.NPGeneralRefObj)v;
        }

        static StackObject* AssignFromStack_npGeneral_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            GOE.NPGeneralRefObj @npGeneral = (GOE.NPGeneralRefObj)typeof(GOE.NPGeneralRefObj).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GRefdataCoreMgr)o).npGeneral = @npGeneral;
            return ptr_of_this_method;
        }

        static object get_rankCommonRefCore_5(ref object o)
        {
            return ((GOE.GRefdataCoreMgr)o).rankCommonRefCore;
        }

        static StackObject* CopyToStack_rankCommonRefCore_5(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GRefdataCoreMgr)o).rankCommonRefCore;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_rankCommonRefCore_5(ref object o, object v)
        {
            ((GOE.GRefdataCoreMgr)o).rankCommonRefCore = (ALPackage.ALBasicListRefCore<GOE.NPRankRefObj>)v;
        }

        static StackObject* AssignFromStack_rankCommonRefCore_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ALPackage.ALBasicListRefCore<GOE.NPRankRefObj> @rankCommonRefCore = (ALPackage.ALBasicListRefCore<GOE.NPRankRefObj>)typeof(ALPackage.ALBasicListRefCore<GOE.NPRankRefObj>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GRefdataCoreMgr)o).rankCommonRefCore = @rankCommonRefCore;
            return ptr_of_this_method;
        }

        static object get_tipMap_6(ref object o)
        {
            return ((GOE.GRefdataCoreMgr)o).tipMap;
        }

        static StackObject* CopyToStack_tipMap_6(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((GOE.GRefdataCoreMgr)o).tipMap;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_tipMap_6(ref object o, object v)
        {
            ((GOE.GRefdataCoreMgr)o).tipMap = (ALPackage.ALBasicMapRefCore<global::NPCenterTipsRefObj>)v;
        }

        static StackObject* AssignFromStack_tipMap_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ALPackage.ALBasicMapRefCore<global::NPCenterTipsRefObj> @tipMap = (ALPackage.ALBasicMapRefCore<global::NPCenterTipsRefObj>)typeof(ALPackage.ALBasicMapRefCore<global::NPCenterTipsRefObj>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((GOE.GRefdataCoreMgr)o).tipMap = @tipMap;
            return ptr_of_this_method;
        }



    }
}
