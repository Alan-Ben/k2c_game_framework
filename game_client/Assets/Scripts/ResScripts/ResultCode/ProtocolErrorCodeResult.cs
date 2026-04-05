using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace GOE
{
    /******
     * 这是全局错误码管理器，提供注册和查询服务
     */
    public class ProtocolErrorCodeResult
    {
        private static ProtocolErrorCodeResult _g_instance;
        public static ProtocolErrorCodeResult instance
        {
            get
            {
                if(_g_instance == null) {
                    _g_instance = new ProtocolErrorCodeResult();
                }
                return _g_instance;
            }
        }

#if UNITY_EDITOR
        //静态初始化方法
        public static void InitRegistResults()
        {
            new MainRegister();
        }  
#endif
        
        [NotNull]private Dictionary<int, RESULT> _m_dic = new Dictionary<int, RESULT>();
        
        //注册数据
        public void RegistResult(RESULT _result)
        {
            if(null == _result)
                return;
            
            _m_dic.TryAdd(_result.getCode(), _result);
        }
        
        //获取对应错误码结果的注释描述
        public string getResultMsg(int _errCode)
        {
            if (_m_dic.TryGetValue(_errCode, out RESULT result))
            {
                if (result != null) 
                    return result.getMsg();
            }
            return string.Empty;
        }

        internal void _log()
        {
            foreach (KeyValuePair<int, RESULT> item in _m_dic)
            {
                UnityEngine.Debug.Log($"RESULT[{item.Key}] = {item.Value.getMsg()}");
            }
        }
    }
}
