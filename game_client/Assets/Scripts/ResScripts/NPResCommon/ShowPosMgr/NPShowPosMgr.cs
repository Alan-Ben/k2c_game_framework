using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NP中根据不同对象类型，可以通过custommono注册对应的显示位置
/// 而引导中的对象可以根据注册标记将某个显示UI移动到对应位置
/// </summary>
namespace GOE
{
    public class NPShowPosMgr
    {
        private static NPShowPosMgr _g_instance = new NPShowPosMgr();
        public static NPShowPosMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPShowPosMgr();

                return _g_instance;
            }
        }

        //根据标记注册显示位置
        private Dictionary<string, _INPShowPos> _m_dicShowPosDic;

        protected NPShowPosMgr()
        {
            _m_dicShowPosDic = new Dictionary<string, _INPShowPos>();
        }

        /// <summary>
        /// 注册显示位置标记
        /// </summary>
        /// <param name="_tag"></param>
        /// <param name="_pos"></param>
        public void regPos(string _tag, _INPShowPos _pos)
        {
            if (null == _pos)
                return;

            //注册到数据集
            if (_m_dicShowPosDic != null && !_m_dicShowPosDic.ContainsKey(_tag)) 
                _m_dicShowPosDic.Add(_tag, _pos);
        }

        /// <summary>
        /// 注销显示位置标记
        /// </summary>
        /// <param name="_tag"></param>
        /// <param name="_pos"></param>
        public void unregPos(string _tag)
        {
            if (_m_dicShowPosDic != null && _m_dicShowPosDic.ContainsKey(_tag)) 
                _m_dicShowPosDic.Remove(_tag);
        }

        /// <summary>
        /// 获取对应标记的位置信息
        /// </summary>
        /// <param name="_tag"></param>
        /// <returns></returns>
        public _INPShowPos getPos(string _tag)
        {
            _INPShowPos pos = null;
            if (!_m_dicShowPosDic.TryGetValue(_tag, out pos))
                return null;

            return pos;
        }
    }
}

