using System;
using System.Collections;
using System.Collections.Generic;

#if AL_ILRUNTIME
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#endif

using ALPackage;

namespace ALPackage
{
    #if AL_ILRUNTIME
    public class _IALBasicUIWndInterfaceAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_IALBasicUIWndInterface);//这是你想继承的那个类
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
        class Adapter : _IALBasicUIWndInterface, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            //缓存这个数组来避免调用时的GC Alloc
            object[] param0 = new object[0];

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

            bool _m_bIsGetGameObjGot = false;
            IMethod _m_iGetGameObjMethod;
            public UnityEngine.GameObject getGameObj()
            {
                if(!_m_bIsGetGameObjGot)
                {
                    _m_iGetGameObjMethod = instance.Type.GetMethod("getGameObj", 0);
                    _m_bIsGetGameObjGot = true;
                }

                if(_m_iGetGameObjMethod != null)
                {
                    return appdomain.Invoke(_m_iGetGameObjMethod, instance, param0) as UnityEngine.GameObject;//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                }
                else
                {
                    return null;
                }
            }

            bool _m_bIsShowWndGot = false;
            IMethod _m_iShowWndMethod;
            public void showWnd()
            {
                if(!_m_bIsShowWndGot)
                {
                    _m_iShowWndMethod = instance.Type.GetMethod("showWnd", 0);
                    _m_bIsShowWndGot = true;
                }

                if(_m_iShowWndMethod != null)
                {
                    appdomain.Invoke(_m_iShowWndMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                }
            }

            bool _m_bIsHideWndGot = false;
            IMethod _m_iHideWndMethod;
            public void hideWnd()
            {
                if(!_m_bIsHideWndGot)
                {
                    _m_iHideWndMethod = instance.Type.GetMethod("hideWnd", 0);
                    _m_bIsHideWndGot = true;
                }

                if(_m_iHideWndMethod != null)
                {
                    appdomain.Invoke(_m_iHideWndMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                }
            }

            bool _m_bIsShowWndWithoutAniGot = false;
            IMethod _m_iShowWndWithoutAniMethod;
            public void showWndWithoutAni()
            {
                if (!_m_bIsShowWndWithoutAniGot)
                {
                    _m_iShowWndWithoutAniMethod = instance.Type.GetMethod("showWndWithoutAni", 0);
                    _m_bIsShowWndWithoutAniGot = true;
                }

                if (_m_iShowWndWithoutAniMethod != null)
                {
                    appdomain.Invoke(_m_iShowWndWithoutAniMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                }
            }

            bool _m_bIsHideWndWithoutAniGot = false;
            IMethod _m_iHideWndWithoutAniMethod;
            public void hideWndWithoutAni()
            {
                if (!_m_bIsHideWndWithoutAniGot)
                {
                    _m_iHideWndWithoutAniMethod = instance.Type.GetMethod("hideWndWithoutAni", 0);
                    _m_bIsHideWndWithoutAniGot = true;
                }

                if (_m_iHideWndWithoutAniMethod != null)
                {
                    appdomain.Invoke(_m_iHideWndWithoutAniMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                }
            }


            bool _m_bIsResetWndGot = false;
            IMethod _m_iResetWndMethod;
            public void resetWnd()
            {
                if(!_m_bIsResetWndGot)
                {
                    _m_iResetWndMethod = instance.Type.GetMethod("resetWnd", 0);
                    _m_bIsResetWndGot = true;
                }

                if(_m_iResetWndMethod != null)
                {
                    appdomain.Invoke(_m_iResetWndMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                }
            }


            bool _m_bIsDiscardGot = false;
            IMethod _m_iDiscardMethod;
            public void discard()
            {
                if(!_m_bIsDiscardGot)
                {
                    _m_iDiscardMethod = instance.Type.GetMethod("discard", 0);
                    _m_bIsDiscardGot = true;
                }

                if(_m_iDiscardMethod != null)
                {
                    appdomain.Invoke(_m_iDiscardMethod, instance, param0);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
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
