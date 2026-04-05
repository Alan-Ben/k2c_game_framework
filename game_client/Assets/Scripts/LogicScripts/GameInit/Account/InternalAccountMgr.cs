using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

using ALPackage;

namespace GOE
{
    /// <summary>
    /// 单个账号管理信息
    /// </summary>
    public class InternalAccountInfo
    {
        public string userName;
        public string password;
        public string customMsg;

        public const char settingLocalSaveSplit = '|';//分隔符

        public InternalAccountInfo()
        {
            userName = null;
            password = null;
            customMsg = null;
        }
        public InternalAccountInfo(string _userName, string _password, string _customMsg)
        {
            userName = _userName;
            password = _password;
            customMsg = _customMsg;
        }

        /// <summary>
        /// 构造账号信息的字符串
        /// </summary>
        /// <param name="_str"></param>
        public void makeAccountStr(StringBuilder _str)
        {
            _str.Append(userName)
                .Append(settingLocalSaveSplit)
                .Append(password)
                .Append(settingLocalSaveSplit)
                .Append(customMsg);
        }
    }

    /// <summary>
    /// 账号管理对象
    /// </summary>
    public class InternalAccountMgr : _AALBasicSettingInfo
    {
        private static InternalAccountMgr _g_instance = new InternalAccountMgr();
        public static InternalAccountMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new InternalAccountMgr();
                return _g_instance;
            }
        }

        //最后一次登录使用的登录方式
        private NPEnum.ENPLoginWayType _m_eLastLoginWay;
        //最后一次登录使用的名称，用于显示，如最后一次用的是作弊方式，那么将使用下面的用户名
        private string _m_sLastLoginName;

        //下面的是用于正常流程记录最后一次登录的用户信息字段
        //最后一次登陆成功的用户ID
        private string _m_lLastLoginUid;
        //最后一次登录使用的token
        private string _m_sLastLoginToken;

        //当前使用的用户名
        private string _m_sCurUseUserName;
        //存储账号信息队列
        private List<InternalAccountInfo> _m_lAccountList;

        protected InternalAccountMgr()
            : base("__np_account_setting")
        {
            _m_eLastLoginWay = NPEnum.ENPLoginWayType.NONE;
            _m_sLastLoginName = string.Empty;
            _m_lLastLoginUid = string.Empty;
            _m_sLastLoginToken = string.Empty;
            _m_sCurUseUserName = string.Empty;
            _m_lAccountList = new List<InternalAccountInfo>();
        }

        public NPEnum.ENPLoginWayType lastLoginWay { get { return _m_eLastLoginWay; } }
        public string lastLoginName { get { return _m_sLastLoginName; } }
        public string lastLoginUid { get { return _m_lLastLoginUid; } }
        public string lastLoginToken { get { return _m_sLastLoginToken; } }

        public string curUseUserName { get { return _m_sCurUseUserName; } }
        public int accountCount { get { return _m_lAccountList.Count; } }

        /** 拷贝用户数据队列 */
        public List<InternalAccountInfo> cloneList()
        {
            List<InternalAccountInfo> newList = new List<InternalAccountInfo>();
            newList.AddRange(_m_lAccountList);

            return newList;
        }

        /*************
         * 构建需要保存的字符串
         **/
        protected override string _makeSettingStr()
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append(_m_eLastLoginWay.ToString());
            strBuilder.Append(InternalAccountInfo.settingLocalSaveSplit);
            strBuilder.Append(_m_sLastLoginName);
            strBuilder.Append(InternalAccountInfo.settingLocalSaveSplit);
            strBuilder.Append(_m_lLastLoginUid.ToString());
            strBuilder.Append(InternalAccountInfo.settingLocalSaveSplit);
            strBuilder.Append(_m_sLastLoginToken);
            strBuilder.Append(InternalAccountInfo.settingLocalSaveSplit);

            strBuilder.Append(_m_sCurUseUserName);
            strBuilder.Append(InternalAccountInfo.settingLocalSaveSplit);
            //放入队列长度
            strBuilder.Append(_m_lAccountList.Count.ToString());

            //逐个账号放入
            InternalAccountInfo accountInfo = null;
            for(int i = 0; i < _m_lAccountList.Count; i++)
            {
                accountInfo = _m_lAccountList[i];
                if(null == accountInfo)
                    continue;

                //放入分割符
                strBuilder.Append(InternalAccountInfo.settingLocalSaveSplit);
                //放入信息
                accountInfo.makeAccountStr(strBuilder);
            }

            //返回字符串
            return strBuilder.ToString();
        }
        /**************
         * 读取保存的字符串
         **/
        protected override void _initSettingStr(string _infoStr)
        {
            if(_infoStr.Length <= 0)
                return;

            int index = 0;
            //获取登录信息
            _m_eLastLoginWay = (NPEnum.ENPLoginWayType)ALCommon.EnumParse(typeof(NPEnum.ENPLoginWayType), __readSettingStrItem(_infoStr, ref index));
            _m_sLastLoginName = __readSettingStrItem(_infoStr, ref index);
            _m_lLastLoginUid = __readSettingStrItem(_infoStr, ref index);
            _m_sLastLoginToken = __readSettingStrItem(_infoStr, ref index);
            //读取当前使用账号
            _m_sCurUseUserName = __readSettingStrItem(_infoStr, ref index);

            int count = 0;
            if(!int.TryParse(__readSettingStrItem(_infoStr, ref index), out count))
                return;

            for(int i = 0; i < count; i++)
            {
                InternalAccountInfo accountInfo = new InternalAccountInfo();
                //读取账号相关设置
                accountInfo.userName = __readSettingStrItem(_infoStr, ref index);
                accountInfo.password = __readSettingStrItem(_infoStr, ref index);
                accountInfo.customMsg = __readSettingStrItem(_infoStr, ref index);

                //加入队列
                _m_lAccountList.Add(accountInfo);
            }
        }

        /// <summary>
        /// 读取单个配置信息
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_index"></param>
        /// <returns></returns>
        private string __readSettingStrItem(string _str, ref int _index)
        {
            if(_index < 0)
                return string.Empty;

            //检测下一个位置
            int nextIdx = _str.IndexOf(InternalAccountInfo.settingLocalSaveSplit, _index);
            if(nextIdx < 0)
            {
                string resS = _str.Substring(_index);
                _index = -1;

                return resS;
            }
            else
            {
                //读取中间内容
                string resS = _str.Substring(_index, nextIdx - _index);
                _index = nextIdx + 1;

                return resS;
            }
        }

        //记录登录过的账号密码
        public void RecordUser(string _user, string _psw, string _customMsg)
        {
            Debug.Log("RecordUser......user: " + _user + "\t\tpsw:  " + _psw);

            //判断是否有对应用户
            InternalAccountInfo accountInfo = getAccountInfo(_user);
            if(null == accountInfo)
            {
                //添加节点
                _addUser(_user, _psw, _customMsg);
            }
            else
            {
                //设置密码
                accountInfo.password = _psw;
            }

            //保存总数据
            saveSetting();
        }

        //设置最后一次的相关登录信息
        public void setLastLoginInfo(string _uid, string _lastToken)
        {
            _m_lLastLoginUid = _uid;
            _m_sLastLoginToken = _lastToken;

            //保存信息
            saveSetting();
        }
        public void setLastLoginTag(NPEnum.ENPLoginWayType _loginWayType, string _lastName)
        {
            _m_eLastLoginWay = _loginWayType;
            _m_sLastLoginName = _lastName;

            //保存信息
            saveSetting();
        }

        //设置当前使用账号信息
        public void setCurUseUser(string _user)
        {
            _m_sCurUseUserName = _user;
            //保存信息
            saveSetting();
        }

        //清除某个账号记录
        public void DeleteUser(string _user)
        {
            //删除用户
            if(!_removeUser(_user))
                return;

            //保存信息
            saveSetting();
        }

        public void ClearRecord()
        {
            _m_lAccountList.Clear();

            //保存信息
            saveSetting();
        }

        /// <summary>
        /// 查询对应账号信息
        /// </summary>
        /// <param name="_userName"></param>
        /// <returns></returns>
        public InternalAccountInfo getAccountInfo(string _userName)
        {
            if(null == _userName)
                return null;

            //构造整体字符串
            InternalAccountInfo tmpInfo = null;
            for(int i = 0; i < _m_lAccountList.Count; i++)
            {
                tmpInfo = _m_lAccountList[i];
                if(null == tmpInfo)
                    continue;

                if(tmpInfo.userName.Equals(_userName))
                {
                    return tmpInfo;
                }
            }

            return null;
        }

        /********************
         * 获取列表里一个可用的本地账户
         **/
        public InternalAccountInfo getAUsefulAccountInfo()
        {
            //构造整体字符串
            InternalAccountInfo tmpInfo = null;
            for(int i = 0; i < _m_lAccountList.Count; i++)
            {
                tmpInfo = _m_lAccountList[i];
                if(null == tmpInfo)
                    continue;

                return tmpInfo;
            }

            return null;
        }

        /**************
         * 添加新节点
         **/
        protected void _addUser(string _username, string _password, string _customMsg)
        {
            InternalAccountInfo accounInfo = new InternalAccountInfo(_username, _password, _customMsg);
            if(string.IsNullOrEmpty(accounInfo.userName) || string.IsNullOrEmpty(accounInfo.password))
            {
                return;
            }

            _m_lAccountList.Add(accounInfo);
        }

        /**************
         * 删除用户
         **/
        protected bool _removeUser(string _username)
        {
            //判断是否删除当前使用账号，是则清空使用账号
            if(_username.Equals(_m_sCurUseUserName))
            {
                _m_sCurUseUserName = string.Empty;
            }

            //构造整体字符串
            InternalAccountInfo tmpInfo = null;
            for(int i = 0; i < _m_lAccountList.Count; i++)
            {
                tmpInfo = _m_lAccountList[i];
                if(null == tmpInfo)
                    continue;

                if(tmpInfo.userName.Equals(_username))
                {
                    _m_lAccountList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}
