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
    unsafe class SceneInfoRefObj_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::SceneInfoRefObj);

            field = type.GetField("camera_setting", flag);
            app.RegisterCLRFieldGetter(field, get_camera_setting_0);
            app.RegisterCLRFieldSetter(field, set_camera_setting_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_camera_setting_0, AssignFromStack_camera_setting_0);
            field = type.GetField("corner_pos", flag);
            app.RegisterCLRFieldGetter(field, get_corner_pos_1);
            app.RegisterCLRFieldSetter(field, set_corner_pos_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_corner_pos_1, AssignFromStack_corner_pos_1);


        }



        static object get_camera_setting_0(ref object o)
        {
            return ((global::SceneInfoRefObj)o).camera_setting;
        }

        static StackObject* CopyToStack_camera_setting_0(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::SceneInfoRefObj)o).camera_setting;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_camera_setting_0(ref object o, object v)
        {
            ((global::SceneInfoRefObj)o).camera_setting = (global::WCGCameraSettingInfo)v;
        }

        static StackObject* AssignFromStack_camera_setting_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::WCGCameraSettingInfo @camera_setting = (global::WCGCameraSettingInfo)typeof(global::WCGCameraSettingInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::SceneInfoRefObj)o).camera_setting = @camera_setting;
            return ptr_of_this_method;
        }

        static object get_corner_pos_1(ref object o)
        {
            return ((global::SceneInfoRefObj)o).corner_pos;
        }

        static StackObject* CopyToStack_corner_pos_1(ref object o, ILIntepreter __intp, StackObject* __ret, AutoList __mStack)
        {
            var result_of_this_method = ((global::SceneInfoRefObj)o).corner_pos;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_corner_pos_1(ref object o, object v)
        {
            ((global::SceneInfoRefObj)o).corner_pos = (UnityEngine.Vector3)v;
        }

        static StackObject* AssignFromStack_corner_pos_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, AutoList __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector3 @corner_pos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)16);
            ((global::SceneInfoRefObj)o).corner_pos = @corner_pos;
            return ptr_of_this_method;
        }



    }
}
