using ALPackage;
using CommonEnum;
using System;

namespace GOE
{
    /// <summary>
    /// 属性加成具体信息类
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class _AAttrPropertyBonusModifier : _IBonusReader
    {
        //属性加成对象
        private PlayerBonusPropertyModifier _m_pbpModifier;

        public _AAttrPropertyBonusModifier()
        {
            _m_pbpModifier = new PlayerBonusPropertyModifier();
        }


        //endregion

        public void ParseFromString(string _sValue)
        {
            if (_sValue == null || _sValue.Trim().Length <= 0)
                return ;

            readStr(_sValue);

            return ;
        }

        /**
         * 读取字符串初始化本对象数据
         * 加成类型枚举|属性枚举:数量;属性枚举:数量
         * @param _str 配置字符串
         */
        private void readStr(String _str)
        {
            //读取加成类型
            ALStringReader strR = new ALStringReader(_str);
            String typeStr = strR.readItem('|');

            try
            {
                //通过存储对象的readStr接口添加属性
                _readStr(typeStr);

                //读取加成信息
                _m_pbpModifier.readStr(strR.readItem('|'), "");
            }
            catch (Exception e)
            {
                ALLog.Error("Read PropBonus Error: " + e.StackTrace);
            }
        }

        /*********
         * 获取实际加成数据
         * @return
         */
        public PlayerBonusPropertyModifier getBonusPropertyModifier()
        {
            return _m_pbpModifier;
        }

        /***************
         * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
         * @param _subIdInfo
         */
        protected abstract void _readStr(String _subIdInfo);

        /************
         * 获取加成类型
         * @return
         */
        public abstract EBonusFilterType getFilterType();

        /**
         * 获取对应的子加成id筛选数据，如无筛选则返回0
         * @return ENPPropBonusType
         */
        public abstract long getBonusId();
    }
}