using System;
using System.Collections;
using System.Collections.Generic;

#if AL_ILRUNTIME
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.TypeSystem;
#endif

using ALBasicProtocolPack;

namespace ALPackage
{
#if AL_ILRUNTIME
    public class _IALProtocolStructureAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_IALProtocolStructure);//这是你想继承的那个类
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);//这是实际的适配器类
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);//创建一个新的实例
        }

        //实际的适配器类需要继承你想继承的那个类，并且实现CrossBindingAdaptorType接口
        public class Adapter : _IALProtocolStructure, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            //缓存这个数组来避免调用时的GC Alloc
            object[] param0 = new object[0];
            object[] param1 = new object[1];

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            //缓存这个数组来避免调用时的GC Alloc
            //object[] param1 = new object[1];
            //你需要重写所有你希望在热更脚本里面重写的方法，并且将控制权转到脚本里去
            //public override void TestAbstract(int ab)
            //{
            //    if (!mTestAbstractGot)
            //    {
            //        mTestAbstract = instance.Type.GetMethod("TestAbstract", 1);
            //        mTestAbstractGot = true;
            //    }
            //    if (mTestAbstract != null)
            //    {
            //        param1[0] = ab;
            //        appdomain.Invoke(mTestAbstract, instance, param1);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
            //    }
            //}

            //IMethod mTestVirtual;
            //bool mTestVirtualGot;
            //bool isTestVirtualInvoking = false;
            //public override void TestVirtual(string str)
            //{
            //    if (!mTestVirtualGot)
            //    {
            //        mTestVirtual = instance.Type.GetMethod("TestVirtual", 1);
            //        mTestVirtualGot = true;
            //    }
            //    //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
            //    if (mTestVirtual != null && !isTestVirtualInvoking)
            //    {
            //        isTestVirtualInvoking = true;
            //        param1[0] = str;
            //        appdomain.Invoke(mTestVirtual, instance, param1);
            //        isTestVirtualInvoking = false;
            //    }
            //    else
            //        base.TestVirtual(str);
            //}

            //IMethod mGetValue;
            //bool mGetValueGot;
            //bool isGetValueInvoking = false;
            //public override int Value
            //{
            //    get
            //    {
            //        if(!mGetValueGot)
            //        {
            //            //属性的Getter编译后会以get_XXX存在，如果不确定的话可以打开Reflector等反编译软件看一下函数名称
            //            mGetValue = instance.Type.GetMethod("get_Value", 1);
            //            mGetValueGot = true;
            //        }
            //        //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.Value就会造成无限循环，最终导致爆栈
            //        if(mGetValue != null && !isGetValueInvoking)
            //        {
            //            isGetValueInvoking = true;
            //            var res = (int)appdomain.Invoke(mGetValue, instance, null);
            //            isGetValueInvoking = false;
            //            return res;
            //        }
            //        else
            //            return base.Value;
            //    }
            //}

            bool _m_bIsGetMainOrderGot;
            IMethod _m_iGetMainOrderMethod;
            bool _m_bIsGetMainOrderInvoking = false;
            public byte getMainOrder()
            {
                if(!_m_bIsGetMainOrderGot)
                {
                    _m_iGetMainOrderMethod = instance.Type.GetMethod("getMainOrder", 0);
                    _m_bIsGetMainOrderGot = true;
                }

                if(_m_iGetMainOrderMethod != null && !_m_bIsGetMainOrderInvoking)
                {
                    _m_bIsGetMainOrderInvoking = true;
                    var res = (byte)appdomain.Invoke(_m_iGetMainOrderMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    _m_bIsGetMainOrderInvoking = false;

                    return res;
                }
                else
                    return 0;
            }

            bool _m_bIsGetSubOrderGot;
            IMethod _m_iGetSubOrderMethod;
            bool _m_bIsGetSubOrderInvoking = false;
            public byte getSubOrder()
            {
                if(!_m_bIsGetSubOrderGot)
                {
                    _m_iGetSubOrderMethod = instance.Type.GetMethod("getSubOrder", 0);
                    _m_bIsGetSubOrderGot = true;
                }

                if(_m_iGetSubOrderMethod != null && !_m_bIsGetSubOrderInvoking)
                {
                    _m_bIsGetSubOrderInvoking = true;
                    var res = (byte)appdomain.Invoke(_m_iGetSubOrderMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    _m_bIsGetSubOrderInvoking = false;

                    return res;
                }
                else
                    return 0;
            }

            bool _m_bIsGetBufSizeGot;
            IMethod _m_iGetBufSizeMethod;
            bool _m_bIsGetBufSizeInvoking = false;
            public int GetBufSize()
            {
                if(!_m_bIsGetBufSizeGot)
                {
                    _m_iGetBufSizeMethod = instance.Type.GetMethod("GetBufSize", 0);
                    _m_bIsGetBufSizeGot = true;
                }

                if(_m_iGetBufSizeMethod != null && !_m_bIsGetBufSizeInvoking)
                {
                    _m_bIsGetBufSizeInvoking = true;
                    var res = (int)appdomain.Invoke(_m_iGetBufSizeMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    _m_bIsGetBufSizeInvoking = false;

                    return res;
                }
                else
                    return 0;
            }

            bool _m_bIsGetFullPackBufSizeGot;
            IMethod _m_iGetFullPackBufSizeMethod;
            bool _m_bIsGetFullPackBufSizeInvoking = false;
            public int GetFullPackBufSize()
            {
                if(!_m_bIsGetFullPackBufSizeGot)
                {
                    _m_iGetFullPackBufSizeMethod = instance.Type.GetMethod("GetFullPackBufSize", 0);
                    _m_bIsGetFullPackBufSizeGot = true;
                }

                if(_m_iGetFullPackBufSizeMethod != null && !_m_bIsGetFullPackBufSizeInvoking)
                {
                    _m_bIsGetFullPackBufSizeInvoking = true;
                    var res = (int)appdomain.Invoke(_m_iGetFullPackBufSizeMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    _m_bIsGetFullPackBufSizeInvoking = false;

                    return res;
                }
                else
                    return 0;
            }

            bool _m_bIsMakeFullPackageGot;
            IMethod _m_iMakeFullPackageMethod;
            bool _m_bIsMakeFullPackageInvoking = false;
            public byte[] makeFullPackage()
            {
                if(!_m_bIsMakeFullPackageGot)
                {
                    _m_iMakeFullPackageMethod = instance.Type.GetMethod("makeFullPackage", 0);
                    _m_bIsMakeFullPackageGot = true;
                }

                if(_m_iMakeFullPackageMethod != null && !_m_bIsMakeFullPackageInvoking)
                {
                    _m_bIsMakeFullPackageInvoking = true;
                    var res = (byte[])appdomain.Invoke(_m_iMakeFullPackageMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    _m_bIsMakeFullPackageInvoking = false;

                    return res;
                }
                else
                    return null;
            }

            bool _m_bIsMakeFullPackageActGot;
            IMethod _m_iMakeFullPackageActMethod;
            bool _m_bIsMakeFullPackageActInvoking = false;
            public void makeFullPackage(ALProtocolBuf _recBuf)
            {
                if(!_m_bIsMakeFullPackageActGot)
                {
                    _m_iMakeFullPackageActMethod = instance.Type.GetMethod("makeFullPackage", 1);
                    _m_bIsMakeFullPackageActGot = true;
                }

                if(_m_iMakeFullPackageActMethod != null && !_m_bIsMakeFullPackageActInvoking)
                {
                    _m_bIsMakeFullPackageActInvoking = true;

                    param1[0] = _recBuf;
                    appdomain.Invoke(_m_iMakeFullPackageActMethod, instance, param1);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    param1[0] = null;

                    _m_bIsMakeFullPackageActInvoking = false;
                }
            }

            bool _m_bIsMakePackageGot;
            IMethod _m_iMakePackageMethod;
            bool _m_bIsMakePackageInvoking = false;
            public byte[] makePackage()
            {
                if(!_m_bIsMakePackageGot)
                {
                    _m_iMakePackageMethod = instance.Type.GetMethod("makePackage", 0);
                    _m_bIsMakePackageGot = true;
                }

                if(_m_iMakePackageMethod != null && !_m_bIsMakePackageInvoking)
                {
                    _m_bIsMakePackageInvoking = true;
                    var res = (byte[])appdomain.Invoke(_m_iMakePackageMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    _m_bIsMakePackageInvoking = false;

                    return res;
                }
                else
                    return null;
            }

            bool _m_bIsReadPackageGot;
            IMethod _m_iReadPackageMethod;
            bool _m_bIsReadPackageInvoking = false;
            public void readPackage(ALProtocolBuf _buf)
            {
                if(!_m_bIsReadPackageGot)
                {
                    IType paramType = appdomain.GetType(typeof(ALProtocolBuf));
                    _m_iReadPackageMethod = instance.Type.GetMethod("readPackage", new List<IType>() {paramType}, null);
                    _m_bIsReadPackageGot = true;
                }

                if(_m_iReadPackageMethod != null && !_m_bIsReadPackageInvoking)
                {
                    _m_bIsReadPackageInvoking = true;

                    param1[0] = _buf;
                    appdomain.Invoke(_m_iReadPackageMethod, instance, param1);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                    param1[0] = null;

                    _m_bIsReadPackageInvoking = false;
                }
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
#endif
}
