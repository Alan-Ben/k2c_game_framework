using System;
using System.Collections.Generic;
using ALBasicProtocolPack;

namespace GOE
{
    public abstract class _ATVarInfo<T_Enum, T_VAROBJ> : _IVarInfo
        where T_Enum : Enum
        where T_VAROBJ : _ATVarObj<T_Enum>
    {
        private List<T_VAROBJ> _m_lObjList;

        public _ATVarInfo()
        {
            _m_lObjList = new List<T_VAROBJ>();
        }

        /**************
         * 添加一个数据类型
         **/
        public void addObj(T_Enum _type, long _value)
        {
            T_VAROBJ obj = _getNewVarObj();
            obj.setInfo(_type, _value);

            _m_lObjList.Add(obj);
        }
        public void addInfo(T_VAROBJ _obj)
        {
            T_VAROBJ obj = _getNewVarObj();
            obj.setInfo(_obj);

            _m_lObjList.Add(obj);
        }

        /*************
         * 拷贝对应的数据
         **/
        public void setInfo(_ATVarInfo<T_Enum, T_VAROBJ> _variableInfo)
        {
            if (null == _variableInfo)
                return;

            for (int i = 0; i < _variableInfo._m_lObjList.Count; i++)
                addInfo(_variableInfo._m_lObjList[i]);
        }

        /***********
         * 根据类型获取对应数值
         **/
        public long getValue(T_Enum _type)
        {
            for (int i = 0; i < _m_lObjList.Count; i++)
                if (_m_lObjList[i].type.Equals(_type))
                    return _m_lObjList[i].value;

            return 0;
        }

        /*************
         * 重置列表并放回缓存
         **/
        public void reset()
        {
            for (int i = 0; i < _m_lObjList.Count; i++)
                _resetVarObj(_m_lObjList[i]);

            _m_lObjList.Clear();
        }

        /***********
         * 获取字节尺寸
         **/
        public int getSize()
        {
            if (_m_lObjList.Count <= 0)
                return 2;

            return 2 + (_m_lObjList.Count * 10);
        }
        public void toBuffer(ALProtocolBuf _buf)
        {
            _buf.putShort((short)_m_lObjList.Count);

            if (_m_lObjList.Count <= 0)
                return;

            for (int i = 0; i < _m_lObjList.Count; i++)
            {
                _buf.putShort(Convert.ToInt16(_m_lObjList[i].type));
                _buf.putLong(_m_lObjList[i].value);
            }
        }

        public void fromBuffer(ALProtocolBuf _buf)
        {
            if (_m_lObjList == null)
                _m_lObjList = new List<T_VAROBJ>();
            _m_lObjList.Clear();
            
            short count = _buf.getShort();
            if (count <= 0)
                return;
            
            for (int i = 0; i < count; i++)
            {
                addObj((T_Enum)Enum.ToObject(typeof(T_Enum), count), _buf.getLong());
            }
        }

        protected abstract T_VAROBJ _getNewVarObj();
        
        protected abstract void _resetVarObj(T_VAROBJ _varObj);
    }
}