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
    unsafe class ALBasicProtocolPack_ALProtocolBuf_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ALBasicProtocolPack.ALProtocolBuf);
            args = new Type[]{};
            method = type.GetMethod("getBuf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getBuf_0);
            args = new Type[]{};
            method = type.GetMethod("get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_1);
            args = new Type[]{};
            method = type.GetMethod("getCurPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getCurPos_2);
            args = new Type[]{};
            method = type.GetMethod("getInt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getInt_3);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("setPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setPosition_4);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("putInt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, putInt_5);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("allocate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, allocate_6);
            args = new Type[]{typeof(System.Byte)};
            method = type.GetMethod("put", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, put_7);
            args = new Type[]{};
            method = type.GetMethod("getShort", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getShort_8);
            args = new Type[]{typeof(System.Int16)};
            method = type.GetMethod("putShort", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, putShort_9);
            args = new Type[]{};
            method = type.GetMethod("getLong", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getLong_10);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("putLong", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, putLong_11);
            args = new Type[]{};
            method = type.GetMethod("getByteBuffer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getByteBuffer_12);
            args = new Type[]{typeof(System.Byte[])};
            method = type.GetMethod("putByteBuffer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, putByteBuffer_13);

            args = new Type[]{typeof(System.Byte[])};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* getBuf_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getBuf();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_1(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.get();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* getCurPos_2(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getCurPos();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* getInt_3(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getInt();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* setPosition_4(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_pos = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setPosition(@_pos);

            return __ret;
        }

        static StackObject* putInt_5(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_num = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.putInt(@_num);

            return __ret;
        }

        static StackObject* allocate_6(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @_bufSize = ptr_of_this_method->Value;


            var result_of_this_method = ALBasicProtocolPack.ALProtocolBuf.allocate(@_bufSize);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* put_7(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Byte @_num = (byte)ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.put(@_num);

            return __ret;
        }

        static StackObject* getShort_8(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getShort();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* putShort_9(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int16 @_num = (short)ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.putShort(@_num);

            return __ret;
        }

        static StackObject* getLong_10(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getLong();

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* putLong_11(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @_num = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.putLong(@_num);

            return __ret;
        }

        static StackObject* getByteBuffer_12(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getByteBuffer();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* putByteBuffer_13(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Byte[] @_buf = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ALBasicProtocolPack.ALProtocolBuf instance_of_this_method = (ALBasicProtocolPack.ALProtocolBuf)typeof(ALBasicProtocolPack.ALProtocolBuf).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.putByteBuffer(@_buf);

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, AutoList __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Byte[] @_buf = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new ALBasicProtocolPack.ALProtocolBuf(@_buf);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
