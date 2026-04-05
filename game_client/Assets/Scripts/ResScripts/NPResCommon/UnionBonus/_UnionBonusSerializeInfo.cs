namespace GOE
{
    /// <summary>
    /// 序列化的UnionBonus类
    /// </summary>
    [System.Serializable]
    public class _UnionBonusSerializeInfo
    {
        //配置字符串
        public string s_condition;
        
        //是否已经初始化过数据
        [System.NonSerialized]
        private bool _m_bIsInited = false;
        //玩家条件集合数据对象
        [System.NonSerialized]
        private UnionBonus _m_unionBonus;
        
        public _UnionBonusSerializeInfo()
        {
            s_condition = string.Empty;

            _m_bIsInited = false;
            _m_unionBonus = null;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            return string.IsNullOrEmpty(s_condition);
        }
        
        /// <summary>
        /// 使用实际的条件对象，根据是否已经初始化过进行判断和处理
        /// </summary>
        public UnionBonus unionBonus
        {
            get
            {
                if (_m_bIsInited)
                    return _m_unionBonus;

                _m_bIsInited = true;
                _m_unionBonus = new UnionBonus();
                _m_unionBonus.parseFromString(s_condition);

                //如果已经读取，则在非编辑器环境下需要设置数据为空，注意这里服务器不能设置为空
#if !UNITY_EDITOR
                s_condition = null;
#endif
                return _m_unionBonus;
            }
        }

        public void ParseFromString(string _str)
        {
            ParseFromString(_str, string.Empty);
        }
        
        public void ParseFromString(string _str, string _fieldName)
        {
            s_condition = _str;

            _m_bIsInited = false;
            _m_unionBonus = null;
        }
        
        public static UnionBonus ReadFromString(string _str)
        {
            UnionBonus info = new UnionBonus();
            info.parseFromString(_str);

            return info;
        }
    }
}