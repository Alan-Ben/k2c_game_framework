using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;
using System.Text;
using System.Text.RegularExpressions;


namespace GOE
{
	/**************
	 * 保存上一次登录成功的缓存
	 * 
	 * 使用_AALBasicSettingInfo保存用户信息，使用说明
	       *  1.定义 文件的保存路径 _m_sPath
	       *  2.构造函数 要设置 文件的保存路径 _m_sPath
	       *  3.调用初始化接口 init() //不进行多次调用
	       *  4.初始化回调 _initSettingStr(_oldSettingInfo) //参数是上一次 保存的数据
	       *  5.调用 saveSetting() //会间接调用 _makeSettingStr() 这个实现的接口返回的字符串
	 **/
	public class LoginTokenSetting : _AALBasicSettingInfo
	{
	    /// <summary>
	    /// 各账号选择服务器的信息
	    /// </summary>
	    public class LoginUidInfo
	    {
	        public string uid;
	        public int serverId;

	        public const char settingSplit = '|';//分隔符

	        public LoginUidInfo()
	        {
	            uid = null;
	            serverId = 0;
	        }
	        public LoginUidInfo(string _uid, int _serverId)
	        {
	            uid = _uid;
	            serverId = _serverId;
	        }

	        /// <summary>
	        /// 构造信息的字符串
	        /// </summary>
	        /// <param name="_str"></param>
	        public void makeStr(StringBuilder _str)
	        {
	            _str.Append(uid)
	                .Append(settingSplit)
	                .Append(serverId);
	        }

	        /// <summary>
	        /// 读取单个配置信息
	        /// </summary>
	        /// <param name="_str"></param>
	        /// <param name="_index"></param>
	        /// <returns></returns>
	        public static LoginUidInfo readStr(string _str)
	        {
	            //检测下一个位置
	            int nextIdx = _str.IndexOf(settingSplit);
	            if (nextIdx < 0)
	            {
	                return null;
	            }

	            //读取中间内容
	            string uidS = _str.Substring(0, nextIdx);
	            string sIdS = _str.Substring(nextIdx + 1, _str.Length - nextIdx - 1);

	            //读取id，失败则直接返回
	            int sid = 0;
	            if (!Int32.TryParse(sIdS, out sid))
	                return null;

	            return new LoginUidInfo(uidS, sid);
	        }
	    }

	    /************ 变量声明 **************************************************************************/

	    /** 登录Token数据 */
	    private string _m_sLoginToken;
	    /** 登录UID数据 */
	    private string _m_sLoginUID;

	    //存储uid对应选择服务器Id的信息列表
	    private List<LoginUidInfo> _m_lLoginUidList;

	    private static LoginTokenSetting _g_instance = new LoginTokenSetting();
	    public static LoginTokenSetting instance
	    {
	        get
	        {
	            if (null == _g_instance)
	            {
	                _g_instance = new LoginTokenSetting();
	            }
	            return _g_instance;
	        }
	    }

	    /************ 构造函数 **************************************************************************/

	    /**************
	     * 构造函数 初始化 文件保存路径，注意设置自己的文件保存路径
	     **/
	    public LoginTokenSetting() : base("login_token_setting")
	    {
	        _m_lLoginUidList = new List<LoginUidInfo>();
	    }

	    /************ 属性 **************************************************************************/

	    /**************
	     * 读取保存的字符串
	     **/
	    protected override void _initSettingStr(string _infoStr)
	    {
	        /** 读取上一次保存的LoginToken数据 */
	        if (string.IsNullOrEmpty(_infoStr)) {
	            return;
	        }

	        ALStringReader reader = new ALStringReader(_infoStr);
	        _m_sLoginUID = reader.readItem('&');
	        _m_sLoginToken = reader.readItem('&');

	        string tmpStr = reader.readItem('&');
	        while(!string.IsNullOrEmpty(tmpStr))
	        {
	            LoginUidInfo info = LoginUidInfo.readStr(tmpStr);
	            if (null != info)
	                _m_lLoginUidList.Add(info);

	            //读取下一段
	            tmpStr = reader.readItem('&');
	        }
	    }

	    /*************
	    * 构建需要保存的字符串
	    **/
	    protected override string _makeSettingStr()
	    {
	        /** 调用saveSetting()会回调这个接口，返回要保存的数据 */
	        if (string.IsNullOrEmpty(_m_sLoginToken) || string.IsNullOrEmpty(_m_sLoginUID))
	        {
	            return "";
	        }
	        StringBuilder strBuild = new StringBuilder();
	        strBuild.Append(_m_sLoginUID);
	        strBuild.Append('&');
	        strBuild.Append(_m_sLoginToken);

	        //逐个输出队列
	        for(int i = 0; i < _m_lLoginUidList.Count; i++)
	        {
	            //构造字符串
	            strBuild.Append('&');
	            _m_lLoginUidList[i].makeStr(strBuild);
	        }

	        return strBuild.ToString();
	    }

	    /************ 其他函数 **************************************************************************/

	    /// <summary>
	    /// 清除最后一次登录信息
	    /// </summary>
	    public void clearLastLoginInfo()
	    {
	        _m_sLoginToken = "";
	        _m_sLoginUID = "";

	        saveSetting();
	    }

	    /**************
	     * 清除已经保存的数据
	     **/
	    public void ClearSettinInfo() {
	        _m_sLoginToken = "";
	        _m_sLoginUID = "";

	        //清空数据
	        _m_lLoginUidList.Clear();

	        saveSetting();
	    }

	    /**************
	     * 保存Token和UID数据
	     **/
	    public void SaveLoginToken(string _loginUID,string _loginToken) {
	        _m_sLoginUID = _loginUID;
	        _m_sLoginToken = _loginToken;
	        if (null == _loginToken || null ==_loginUID)
	        {
	            _m_sLoginUID = "";
	            _m_sLoginToken = "";
	        }

	        saveSetting();
	    }

	    /**************
	     * 获取LoginUID
	     **/
	    public string GetLoginUID() {
	        if (null ==_m_sLoginUID) {
	            return "";
	        }
	        return _m_sLoginUID;
	    }

	    /**************
	     * 获取LoginToken
	     **/
	    public string GetLoginToken()
	    {
	        if (null == _m_sLoginToken)
	        {
	            return "";
	        }
	        return _m_sLoginToken;
	    }

	    /// <summary>
	    /// 检索用户选择的服务器Id
	    /// </summary>
	    /// <param name="_uid"></param>
	    /// <returns></returns>
	    public int getUidServerId(string _uid)
	    {
	        for (int i = 0; i < _m_lLoginUidList.Count; i++)
	        {
	            if (_m_lLoginUidList[i].uid.Equals(_uid, StringComparison.OrdinalIgnoreCase))
	                return _m_lLoginUidList[i].serverId;
	        }

	        return -1;
	    }

	    /// <summary>
	    /// 删除用户选择的服务器Id
	    /// </summary>
	    /// <param name="_uid"></param>
	    /// <returns></returns>
	    public void removeUidServerId(string _uid)
	    {
	        //是否已保存
	        bool saved = false;
	        for (int i = 0; i < _m_lLoginUidList.Count; i++)
	        {
	            if (_m_lLoginUidList[i].uid.Equals(_uid, StringComparison.OrdinalIgnoreCase))
	            {
	                saved = true;
	                _m_lLoginUidList[i].serverId = 0;
	            }
	        }

	        //如果未保存则保存数据
	        if (saved)
	            saveSetting();
	    }

	    /// <summary>
	    /// 保存用户选择的服务器Id
	    /// </summary>
	    /// <param name="_uid"></param>
	    /// <returns></returns>
	    public void setUidServerId(string _uid, int _serverId)
	    {
	        //非法数据不保存
	        if (_serverId <= 0)
	            return;

	        //是否已保存
	        bool saved = false;
	        for (int i = 0; i < _m_lLoginUidList.Count; i++)
	        {
	            if (_m_lLoginUidList[i].uid.Equals(_uid, StringComparison.OrdinalIgnoreCase))
	            {
	                saved = true;
	                _m_lLoginUidList[i].serverId = _serverId;
	            }
	        }

	        //如果未保存则添加
	        if (!saved)
	            _m_lLoginUidList.Add(new LoginUidInfo(_uid, _serverId));

	        //保存数据
	        saveSetting();
	    }
	}
}