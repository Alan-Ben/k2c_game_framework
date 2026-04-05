
namespace GOE
{
    /// <summary>
    /// 用于ui的聊天会话接口
    /// </summary>
    public interface _INPChatInfo
    {
        //会话名称
        string getChatName();

        //会话头像
        NPGTextureIndex getChatIcon();

        //会话内容
        string getChatContent();

        //会话时间戳
        long getChatTimeMS();

        //排序id 越大排在越上面
        int getSortId();

        //发送间隔
        long getSendCD();
    }
}