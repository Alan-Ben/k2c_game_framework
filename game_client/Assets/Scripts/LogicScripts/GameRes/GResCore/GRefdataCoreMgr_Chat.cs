
namespace GOE
{
    //聊天相关
    public partial class GRefdataCoreMgr
    {
        private void _initChat()
        {
            chatEmoteItemRefCore.dealAllRef((_emoteItemRef) =>
            {
                if(null == _emoteItemRef)
                    return;

                GChatEmoteGroupRefObj groupRefObj = chatEmoteGroupRefCore.getRef(_emoteItemRef.group_id);
                if (null == groupRefObj)
                    return;
                groupRefObj.addEmoteItem(_emoteItemRef);
            });
        }
    }
}