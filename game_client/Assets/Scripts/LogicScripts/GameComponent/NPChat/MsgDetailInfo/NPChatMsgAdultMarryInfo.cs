// using ChatPackage;
// using Common.ChildObj;
// using Common.NpChatObj;
// using NPEnum;
//
// namespace GOE
// {
//     /// <summary>
//     /// 成年子嗣联姻分享消息
//     /// </summary>
//     public class NPChatMsgAdultMarryInfo : _ANPPlayerChatMsgDetailInfo<NPCommon_ChatContent_AdultMarryServerApply>, _INPChatMiniShowInfo
//     {
//         public NPChatMsgAdultMarryInfo() : base((int)ENPChatMsgType.ADULT_MARRY_SERVER_APPLY)
//         {
//
//         }
//
//         public string getMiniSender()
//         {
//             return sender.getCName();
//         }
//
//         public string getMiniContent()
//         {
//             return TextTranslate.instance.getLanguage(TransKeyConst.adult_share_chat_content_str, sender.getCName());//{0}发起联姻邀请
//         }
//
//         public void setAdultItem(AdultItem _item)
//         {
//             if (null == _item)
//                 return;
//
//             Adult_Info info = new Adult_Info();
//             info.setInstanceId(_item.instanceId);
//             info.setName(_item.name);
//             info.setInitResId(_item.resRef.id);
//             info.setAttrValueSum(_item.attrValue);
//             info.setGradeId(_item.gradeRef.id);
//             content.setAdult(info);
//         }
//     }
// }
