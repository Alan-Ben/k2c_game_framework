
using System.Collections.Generic;
using Common.CommObj;
using JetBrains.Annotations;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 聊天消息的设置管理器
    /// </summary>
    /// <remarks>
    /// <para>内部存放着msgType对应的相关数据设置</para>
    /// <para>使用方法：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>MsgInfoSetting的set，reset，get</term>
    ///             <description>
    ///             对于聊天消息，你必须先设置聊天消息的相关处理方法才可以运作，你可以在这个类中找到管理设置，清除和获取的接口，你也可以使用<see cref="Chat"/>下的相关接口
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>createMsgInfo</term>
    ///             <description>
    ///             对于服务端的<see cref="RoomMsg"/>和<see cref="PrivateChatMsg"/>，你可以使用这个方法构建一个客户端用的<see cref="MsgInfo"/>，但前提是你已经设置好了这个msgType的相关处理
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class ChatMsgInfoSettingMgr
    {
        // 消息类型映射到设置的字典
        [NotNull] private readonly Dictionary<int, _AChatDataSetting> _m_msgType2Setting;

        internal ChatMsgInfoSettingMgr()
        {
            // 构建常驻的成员
            _m_msgType2Setting = new Dictionary<int, _AChatDataSetting>();
        }
        
        /// <summary>
        /// 注册对应msgType的设置
        /// </summary>
        public void setMsgInfoSetting(int _msgType, _AChatDataSetting _setting)
        {
            // 重复注册检查
            if (_m_msgType2Setting.ContainsKey(_msgType))
            {
                ChatUtility.logError_DebugOnly($"你已经注册过msgType为（{_msgType}）的数据类型了");
                return;
            }

            // 添加设置
            _m_msgType2Setting.Add(_msgType, _setting);
        }
        /// <summary>
        /// 重置对应的msgType设置
        /// </summary>
        public bool resetMsgInfoSetting(int _msgType)
        {
            // 直接移除
            return _m_msgType2Setting.Remove(_msgType);
        }
        /// <summary>
        /// 获取对应msgType的设置
        /// </summary>
        public _AChatDataSetting getMsgInfoSetting(int _msgType)
        {
            // 尝试获取并返回
            if (_m_msgType2Setting.TryGetValue(_msgType, out _AChatDataSetting setting))
                return setting;

            return null;
        }
        /// <summary>
        /// 构建一个MsgInfo
        /// </summary>
        public MsgInfo createMsgInfo(long _msgId, long _timeMs, Common_Msg _msg)
        {
            if (_msg == null)
                return null;

            // 获取对应msgType的设置
            _AChatDataSetting setting = getMsgInfoSetting(_msg.getMsgType());
            if (setting == null)
            {
                ChatUtility.logError_DebugOnly($"你没有注册msgType为（{_msg.getMsgType()}）的设置，请使用ChatData.setMsgInfoSetting来设置");
                return null;
            }

            // 使用设置中的方法，构建一个消息内容
            _AMsgDetailInfo detailInfo = setting.createMsgDetailInfo();
            if (detailInfo == null)
            {
                ChatUtility.logError_DebugOnly($"你注册的msgType为（{_msg.getMsgType()}）的设置，没有正确实现createMsgDetailInfo方法");
                return null;
            }

            // 给消息内容赋值
            detailInfo.readByBytes(_msg.getGameContent(), _msg.getGameUser());
            // 将消息内容和唯一id填充到MsgInfo中，并返回
            return new MsgInfo(_msgId, _timeMs, detailInfo);
        }

        /// <summary>
        /// 构建一个MsgInfo
        /// </summary>
        public MsgInfo createMsgInfo(HistorySaverData _data)
        {
            if (null == _data)
                return null;

            // 获取对应msgType的设置
            _AChatDataSetting setting = getMsgInfoSetting(_data.msgType);
            if (setting == null)
            {
                ChatUtility.logError_DebugOnly($"你没有注册msgType为（{_data.msgType}）的设置，请使用ChatData.setMsgInfoSetting来设置");
                return null;
            }

            // 使用设置中的方法，构建一个消息内容
            _AMsgDetailInfo detailInfo = setting.createMsgDetailInfo();
            if (detailInfo == null)
            {
                ChatUtility.logError_DebugOnly($"你注册的msgType为（{_data.msgType}）的设置，没有正确实现createMsgDetailInfo方法");
                return null;
            }

            // 给消息内容赋值
            detailInfo.readByBytes(_data.gameContent, _data.gameSender);
            // 将消息内容和唯一id填充到MsgInfo中，并返回
            return new MsgInfo(_data.msgId, _data.timeMs, detailInfo);
        }
    }
}