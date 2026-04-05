using System;
using ALPackage;

namespace GOE
{
    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public abstract class _ATVarObjCache<T_Enum, T_VAROBJ> : _AALUnsafeCacheController<T_VAROBJ, T_VAROBJ> 
        where T_Enum : Enum
        where T_VAROBJ : _ATVarObj<T_Enum>, new()
    {
        public _ATVarObjCache() : base(32, 128)
        {
            init(new T_VAROBJ());
        }

        protected override T_VAROBJ _createItem(T_VAROBJ _template)
        {
            return new T_VAROBJ();
        }

        //警告信息文字
        protected override string _warningTxt { get { return $"_ATVarObjCache<{typeof(T_Enum)}>"; } }

        protected override void _discardItem(T_VAROBJ _item)
        {
            _item.reset();
            return;
        }

        protected override void _onInit(T_VAROBJ _template)
        {
        }

        protected override void _resetItem(T_VAROBJ _item)
        {
            _item.reset();
        }
    }
}