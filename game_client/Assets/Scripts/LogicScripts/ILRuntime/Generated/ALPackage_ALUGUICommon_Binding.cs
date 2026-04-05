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
    unsafe class ALPackage_ALUGUICommon_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ALPackage.ALUGUICommon);
            args = new Type[]{typeof(System.Collections.Generic.List<UnityEngine.GameObject>), typeof(System.Boolean)};
            method = type.GetMethod("setGameObjEnable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setGameObjEnable_0);
            args = new Type[]{typeof(System.Collections.Generic.List<UnityEngine.UI.Graphic>), typeof(UnityEngine.Color)};
            method = type.GetMethod("setUIObjColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setUIObjColor_1);
            args = new Type[]{typeof(UnityEngine.UI.Text), typeof(System.Int32)};
            method = type.GetMethod("setLabelTxt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setLabelTxt_2);
            args = new Type[]{typeof(UnityEngine.UI.Text), typeof(System.String)};
            method = type.GetMethod("setLabelTxt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setLabelTxt_3);
            args = new Type[]{typeof(UnityEngine.RectTransform), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("setUIPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setUIPos_4);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.GameObject>)};
            method = type.GetMethod("uncombineBtnClick", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, uncombineBtnClick_5);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.GameObject>)};
            method = type.GetMethod("combineBtnClick", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, combineBtnClick_6);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.EventSystems.PointerEventData>)};
            method = type.GetMethod("uncombineBeginDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, uncombineBeginDrag_7);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.EventSystems.PointerEventData>)};
            method = type.GetMethod("uncombineDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, uncombineDrag_8);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.EventSystems.PointerEventData>)};
            method = type.GetMethod("uncombineEndDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, uncombineEndDrag_9);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.EventSystems.PointerEventData>)};
            method = type.GetMethod("combineBeginDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, combineBeginDrag_10);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.EventSystems.PointerEventData>)};
            method = type.GetMethod("combineDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, combineDrag_11);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<UnityEngine.EventSystems.PointerEventData>)};
            method = type.GetMethod("combineEndDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, combineEndDrag_12);
            args = new Type[]{typeof(UnityEngine.UI.Text), typeof(System.Int64)};
            method = type.GetMethod("setLabelTxt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setLabelTxt_13);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData>), typeof(System.Boolean)};
            method = type.GetMethod("combineBtnPress", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, combineBtnPress_14);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData>)};
            method = type.GetMethod("uncombineBtnPress", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, uncombineBtnPress_15);
            args = new Type[]{typeof(UnityEngine.MonoBehaviour), typeof(System.Boolean)};
            method = type.GetMethod("setGameObjEnable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setGameObjEnable_16);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Boolean)};
            method = type.GetMethod("setGameObjEnable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setGameObjEnable_17);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("setGameObjDisable", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setGameObjDisable_18);


        }


        static StackObject* setGameObjEnable_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @_enable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<UnityEngine.GameObject> @_list = (System.Collections.Generic.List<UnityEngine.GameObject>)typeof(System.Collections.Generic.List<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.setGameObjEnable(@_list, @_enable);

            return __ret;
        }

        static StackObject* setUIObjColor_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<UnityEngine.UI.Graphic> @_graphic = (System.Collections.Generic.List<UnityEngine.UI.Graphic>)typeof(System.Collections.Generic.List<UnityEngine.UI.Graphic>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.setUIObjColor(@_graphic, @_color);

            return __ret;
        }

        static StackObject* setLabelTxt_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_int = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Text @_label = (UnityEngine.UI.Text)typeof(UnityEngine.UI.Text).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = ALPackage.ALUGUICommon.setLabelTxt(@_label, @_int);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* setLabelTxt_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_txt = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Text @_label = (UnityEngine.UI.Text)typeof(UnityEngine.UI.Text).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = ALPackage.ALUGUICommon.setLabelTxt(@_label, @_txt);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* setUIPos_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @_y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @_x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.RectTransform @_rectTrans = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.setUIPos(@_rectTrans, @_x, @_y);

            return __ret;
        }

        static StackObject* uncombineBtnClick_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.GameObject> @_delegate = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.uncombineBtnClick(@_go, @_delegate);

            return __ret;
        }

        static StackObject* combineBtnClick_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.GameObject> @_delegate = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.combineBtnClick(@_go, @_delegate);

            return __ret;
        }

        static StackObject* uncombineBeginDrag_7(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.uncombineBeginDrag(@_go, @_delegate);

            return __ret;
        }

        static StackObject* uncombineDrag_8(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.uncombineDrag(@_go, @_delegate);

            return __ret;
        }

        static StackObject* uncombineEndDrag_9(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.uncombineEndDrag(@_go, @_delegate);

            return __ret;
        }

        static StackObject* combineBeginDrag_10(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.combineBeginDrag(@_go, @_delegate);

            return __ret;
        }

        static StackObject* combineDrag_11(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.combineDrag(@_go, @_delegate);

            return __ret;
        }

        static StackObject* combineEndDrag_12(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.combineEndDrag(@_go, @_delegate);

            return __ret;
        }

        static StackObject* setLabelTxt_13(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @_long = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Text @_label = (UnityEngine.UI.Text)typeof(UnityEngine.UI.Text).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = ALPackage.ALUGUICommon.setLabelTxt(@_label, @_long);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* combineBtnPress_14(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @_triggerEventAgain = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.combineBtnPress(@_go, @_delegate, @_triggerEventAgain);

            return __ret;
        }

        static StackObject* uncombineBtnPress_15(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData> @_delegate = (System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<System.Boolean, UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)8);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.uncombineBtnPress(@_go, @_delegate);

            return __ret;
        }

        static StackObject* setGameObjEnable_16(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @_enable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.MonoBehaviour @_uiObj = (UnityEngine.MonoBehaviour)typeof(UnityEngine.MonoBehaviour).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.setGameObjEnable(@_uiObj, @_enable);

            return __ret;
        }

        static StackObject* setGameObjEnable_17(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @_enable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.setGameObjEnable(@_go, @_enable);

            return __ret;
        }

        static StackObject* setGameObjDisable_18(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @_go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            ALPackage.ALUGUICommon.setGameObjDisable(@_go);

            return __ret;
        }



    }
}
