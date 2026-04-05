using System;
using ALPackage;

namespace GOE
{
    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public abstract class _ATVarInfoCache<T_Enum, T_VarObj, T_VarInfo> : _AALUnsafeCacheController<T_VarInfo, T_VarInfo> 
        where T_Enum : Enum
        where T_VarObj : _ATVarObj<T_Enum>
        where T_VarInfo : _ATVarInfo<T_Enum, T_VarObj>, new()
    {
        public _ATVarInfoCache() : base(1, 20)
        {
            init(new T_VarInfo());
        }

        protected override T_VarInfo _createItem(T_VarInfo _template)
        {
            return new T_VarInfo();
        }

        //警告信息文字
        protected override string _warningTxt { get { return $"_ATVarInfoCache<{typeof(T_Enum)}>"; } }

        protected override void _discardItem(T_VarInfo _item)
        {
            _item.reset();
            return;
        }

        protected override void _onInit(T_VarInfo _template)
        {
        }

        protected override void _resetItem(T_VarInfo _item)
        {
            _item.reset();
        }
    }
}