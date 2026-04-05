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
    unsafe class GOE_GCommon_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(GOE.GCommon);
            args = new Type[]{typeof(System.Int64), typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("lazycdEnough", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, lazycdEnough_0);
            args = new Type[]{typeof(NPEnum.ENPItemType), typeof(System.Int64), typeof(System.Int64), typeof(System.Boolean), typeof(global::ENPInsteadItemType)};
            method = type.GetMethod("isItemEnough", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, isItemEnough_1);
            args = new Type[]{typeof(NPEnum.ENPItemType), typeof(System.Int64)};
            method = type.GetMethod("getItemQuality", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getItemQuality_2);
            args = new Type[]{typeof(System.Int64), typeof(System.String)};
            method = type.GetMethod("getActivityPrefabSkinAssetPath", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getActivityPrefabSkinAssetPath_3);
            args = new Type[]{typeof(System.Int64), typeof(System.String)};
            method = type.GetMethod("getActivityPrefabSkinObjName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getActivityPrefabSkinObjName_4);
            args = new Type[]{typeof(global::NPCommonItem), typeof(global::ENPInsteadItemType)};
            method = type.GetMethod("getItemCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getItemCount_5);
            args = new Type[]{typeof(System.Collections.Generic.List<NPCommon.NPCommon_ItemInfo>), typeof(System.Boolean)};
            method = type.GetMethod("showGainRewardTip", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, showGainRewardTip_6);
            args = new Type[]{typeof(global::NPCommonCostItem), typeof(System.Boolean), typeof(global::ENPInsteadItemType)};
            method = type.GetMethod("isItemEnough", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, isItemEnough_7);
            args = new Type[]{typeof(NPEnum.ENPItemType), typeof(System.Int64), typeof(System.Int64), typeof(GOE.AccessAdditionData), typeof(System.Int32)};
            method = type.GetMethod("popItemAccessWays", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, popItemAccessWays_8);
            args = new Type[]{typeof(NPEnum.ENPItemType), typeof(System.Int64)};
            method = type.GetMethod("getItemTexIcon", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getItemTexIcon_9);
            args = new Type[]{typeof(System.Int64), typeof(System.Action), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Action)};
            method = type.GetMethod("enterDialogueNode", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, enterDialogueNode_10);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.Color)};
            method = type.GetMethod("addColorForRichText", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, addColorForRichText_11);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("NetRecv", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, NetRecv_12);
            args = new Type[]{typeof(System.Object), typeof(System.Text.StringBuilder)};
            method = type.GetMethod("GetInfoPropertys", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetInfoPropertys_13);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("NetWaring", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, NetWaring_14);


        }


        static StackObject* lazycdEnough_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @_popNotEnough = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @_useTime = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @_lazyCdId = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = GOE.GCommon.lazycdEnough(@_lazyCdId, @_useTime, @_popNotEnough);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* isItemEnough_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::ENPInsteadItemType @_itemAlterType = (global::ENPInsteadItemType)typeof(global::ENPInsteadItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @_popNotEnough = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @_count = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int64 @_subId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            NPEnum.ENPItemType @_itemType = (NPEnum.ENPItemType)typeof(NPEnum.ENPItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.isItemEnough(@_itemType, @_subId, @_count, @_popNotEnough, @_itemAlterType);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* getItemQuality_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @_subId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            NPEnum.ENPItemType @_type = (NPEnum.ENPItemType)typeof(NPEnum.ENPItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.getItemQuality(@_type, @_subId);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getActivityPrefabSkinAssetPath_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_key = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64 @_activityId = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = GOE.GCommon.getActivityPrefabSkinAssetPath(@_activityId, @_key);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getActivityPrefabSkinObjName_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_key = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64 @_activityId = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = GOE.GCommon.getActivityPrefabSkinObjName(@_activityId, @_key);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getItemCount_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::ENPInsteadItemType @_itemAlterType = (global::ENPInsteadItemType)typeof(global::ENPInsteadItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::NPCommonItem @_item = (global::NPCommonItem)typeof(global::NPCommonItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.getItemCount(@_item, @_itemAlterType);

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* showGainRewardTip_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @_useCommaNum = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<NPCommon.NPCommon_ItemInfo> @_itemList = (System.Collections.Generic.List<NPCommon.NPCommon_ItemInfo>)typeof(System.Collections.Generic.List<NPCommon.NPCommon_ItemInfo>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            GOE.GCommon.showGainRewardTip(@_itemList, @_useCommaNum);

            return __ret;
        }

        static StackObject* isItemEnough_7(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::ENPInsteadItemType @_itemAlterType = (global::ENPInsteadItemType)typeof(global::ENPInsteadItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @_popNotEnough = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::NPCommonCostItem @_costItem = (global::NPCommonCostItem)typeof(global::NPCommonCostItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.isItemEnough(@_costItem, @_popNotEnough, @_itemAlterType);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* popItemAccessWays_8(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @findDeep = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            GOE.AccessAdditionData @_additionData = (GOE.AccessAdditionData)typeof(GOE.AccessAdditionData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @_customHasItemNum = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int64 @_subId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            NPEnum.ENPItemType @_itemType = (NPEnum.ENPItemType)typeof(NPEnum.ENPItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.popItemAccessWays(@_itemType, @_subId, @_customHasItemNum, @_additionData, @findDeep);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* getItemTexIcon_9(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @_subId = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            NPEnum.ENPItemType @_type = (NPEnum.ENPItemType)typeof(NPEnum.ENPItemType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)20);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.getItemTexIcon(@_type, @_subId);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* enterDialogueNode_10(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @_dealCloseDialog = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @_needAutoPlay = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Boolean @_isMain = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Action @_doneAction = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Int64 @_dialogId = *(long*)&ptr_of_this_method->Value;


            GOE.GCommon.enterDialogueNode(@_dialogId, @_doneAction, @_isMain, @_needAutoPlay, @_dealCloseDialog);

            return __ret;
        }

        static StackObject* addColorForRichText_11(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @txt = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.addColorForRichText(@txt, @color);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* NetRecv_12(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_msg = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            GOE.GCommon.NetRecv(@_msg);

            return __ret;
        }

        static StackObject* GetInfoPropertys_13(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Text.StringBuilder @strB = (System.Text.StringBuilder)typeof(System.Text.StringBuilder).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Object @objInfo = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = GOE.GCommon.GetInfoPropertys(@objInfo, @strB);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* NetWaring_14(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_msg = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            GOE.GCommon.NetWaring(@_msg);

            return __ret;
        }



    }
}
