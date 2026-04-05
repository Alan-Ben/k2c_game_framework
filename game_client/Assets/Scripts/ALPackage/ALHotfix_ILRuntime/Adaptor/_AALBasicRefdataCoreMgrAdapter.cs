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
    public class _AALBasicRefdataCoreMgrAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(_AALBasicRefdataCoreMgr);//这是你想继承的那个类
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
        class Adapter : _AALBasicRefdataCoreMgr, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            //缓存这个数组来避免调用时的GC Alloc
            object[] param1 = new object[1];
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

            bool _onRefInitFailGot;
            IMethod _onRefInitFailMethod;
            bool _onRefInitFailInvoking = false;
            protected override void _onRefInitFail(Type _class, _IALInitRefObj _obj)
            {
                if (!_onRefInitFailGot)
                {
                    _onRefInitFailMethod = instance.Type.GetMethod("_onRefInitFail", 2);
                    _onRefInitFailGot = true;
                }
                //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
                if (_onRefInitFailMethod != null && !_onRefInitFailInvoking)
                {
                    _onRefInitFailInvoking = true;
                    param2[0] = _class;
                    param2[1] = _obj;
                    appdomain.Invoke(_onRefInitFailMethod, instance, param2);//有参数记得赋值！！
                    _onRefInitFailInvoking = false;
                }
            }

            bool _initLoadListGot;
            IMethod _initLoadListMethod;
            bool _initLoadListInvoking = false;
            protected override void _initLoadList(List<_IALInitRefObj> _list)
            {
                if (!_initLoadListGot)
                {
                    _initLoadListMethod = instance.Type.GetMethod("_initLoadList", 1);
                    _initLoadListGot = true;
                }
                //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
                if (_initLoadListMethod != null && !_initLoadListInvoking)
                {
                    _initLoadListInvoking = true;
                    param1[0] = _list;
                    appdomain.Invoke(_initLoadListMethod, instance, param1);//有参数记得赋值！！
                    _initLoadListInvoking = false;
                }
            }

            bool _onInitAllRefCoreGot;
            IMethod _onInitAllRefCoreMethod;
            bool _onInitAllRefCoreInvoking = false;
            protected override void _onInitAllRefCore()
            {
                if (!_onInitAllRefCoreGot)
                {
                    _onInitAllRefCoreMethod = instance.Type.GetMethod("_onInitAllRefCore", 0);
                    _onInitAllRefCoreGot = true;
                }
                //对于虚函数而言，必须设定一个标识位来确定是否当前已经在调用中，否则如果脚本类中调用base.TestVirtual()就会造成无限循环，最终导致爆栈
                if (_onInitAllRefCoreMethod != null && !_onInitAllRefCoreInvoking)
                {
                    _onInitAllRefCoreInvoking = true;
                    appdomain.Invoke(_onInitAllRefCoreMethod, instance, null);//有参数记得赋值！！
                    _onInitAllRefCoreInvoking = false;
                }
            }
        }
    }
#endif
}
