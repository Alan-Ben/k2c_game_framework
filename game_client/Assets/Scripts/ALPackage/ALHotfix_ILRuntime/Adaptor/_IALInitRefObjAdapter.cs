using System;
using System.Collections;
using System.Collections.Generic;

#if AL_ILRUNTIME
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#endif

using ALBasicProtocolPack;

namespace ALPackage
{
#if AL_ILRUNTIME
    public class _IALInitRefObjAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_IALInitRefObj);//这是你想继承的那个类
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
        public class Adapter : _IALInitRefObj, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            //缓存这个数组来避免调用时的GC Alloc
            object[] param2 = new object[2];

            public Adapter()
            {
            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }


            bool initGot;
            IMethod initMethod;
            public void init(Action _call, Action<Type, _IALInitRefObj> _onFail)
            {
                if (!initGot)
                {
                    initMethod = instance.Type.GetMethod("init", 2);
                    initGot = true;
                }
                if (initMethod != null)
                {
                    param2[0] = _call;
                    param2[1] = _onFail;
                    appdomain.Invoke(initMethod, instance, param2);
                }
            }

            bool forceDoneGot;
            IMethod forceDoneMethod;
            public void forceDone()
            {
                if (!forceDoneGot)
                {
                    forceDoneMethod = instance.Type.GetMethod("forceDone", 0);
                    forceDoneGot = true;
                }
                if (forceDoneMethod != null)
                {
                    appdomain.Invoke(forceDoneMethod, instance, null);
                }
            }

            bool finalDelegateGot;
            IMethod finalDelegateMethod;
            bool finalDelegateInvoking = false;
            public Action finalDelegate
            {
                get
                {
                    if(!finalDelegateGot)
                    {
                        finalDelegateMethod = instance.Type.GetMethod("get_finalDelegate", 0);
                        finalDelegateGot = true;
                    }

                    if(finalDelegateMethod != null && !finalDelegateInvoking)
                    {
                        finalDelegateInvoking = true;
                        Action res = (Action)appdomain.Invoke(finalDelegateMethod, instance, null);//没有参数建议显式传递null为参数列表，否则会自动new object[0]导致GC Alloc
                        finalDelegateInvoking = false;

                        return res;
                    }
                    else
                        return null;
                }
            }
        }
    }
#endif
}
