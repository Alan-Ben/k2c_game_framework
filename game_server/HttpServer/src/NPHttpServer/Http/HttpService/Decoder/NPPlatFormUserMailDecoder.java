package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPCommonItem;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityServerMail;
import NPHttpServer.Http.Entity.NPEntityUserMail;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 玩家邮件json字符串解析器
 * {
 * "cidList": [
 * "2010999"
 * ],
 * "phpMail": {
 * "phpMailId": 505,
 * "sendTime": "2020-12-13 00:00:00",
 * "expiredTime": "2020-12-21 00:00:00",
 * "defaultLang": 1,
 * "mailText": [
 * {
 * "lang": "1",
 * "title": "Event Notice",
 * "content": "We will open the brand-new Cross-server Ranking Rush since Dec 14th. It's time to show your power and win glory! Good luck!"
 * },
 * {
 * "lang": "2",
 * "title": "跨服冲榜活动通知",
 * "content": "尊敬的可汗，为了让更多的可汗见证您的实力，本服将于12月14日开启全新跨服冲榜活动，届时您将与其他区服的可汗共同追逐荣耀，期待您获得胜利！"
 * },
 * ],
 * "itemList": [{
 * "subId": "currency-1",
 * "count": 1
 * },
 * {
 * "subId": "ICON-1301",
 * "count": 1
 * }],
 * "passedTimeMs": 1607874511260
 * }
 * }
 * @author: ricci
 * @date: 2023-03-25 00:12:35
 */
public class NPPlatFormUserMailDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityUserMail>
{
    //////单例的//////
    private static final NPPlatFormUserMailDecoder _s_instance = new NPPlatFormUserMailDecoder();

    public static NPPlatFormUserMailDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormUserMailDecoder()
    {
    }

    @Override
    public NPEntityUserMail decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityUserMail obj = new NPEntityUserMail();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();
            //读取服务器列表
            for (JsonElement jsonElement : jsonObject.get("cidList").getAsJsonArray())
            {
                long cid = jsonElement.getAsLong();
                obj.addCid(cid);
            }
            //获取待初始化的邮件属性对象
            NPEntityServerMail platFromMail = obj.getPlatFromMail();

            //读取后台邮件模版
            JsonObject phpMail = jsonObject.get("phpMail").getAsJsonObject();
            platFromMail.setPhpMailId(phpMail.get("phpMailId").getAsInt());

            //配置邮件相关数据
            if(phpMail.has("mailId")) //邮件配置ID
            {
            	platFromMail.setMailRefId(phpMail.get("mailId").getAsLong());
            }
            if(phpMail.has("mailParams")) //替换文本内容
            {
            	String contentReplace = phpMail.get("mailParams").getAsString();
                String[] contentReplaceStrs = CommonFunc.charSplit(contentReplace, '|');
                for(int i = 0; i < contentReplaceStrs.length; i++)
                {
                	platFromMail.getContentReplace().add(contentReplaceStrs[i]);
                }
            }
            
            platFromMail.setSendTime(phpMail.get("sendTime").getAsString());
            platFromMail.setExpiredTime(phpMail.get("expiredTime").getAsString());
            platFromMail.setDefaultLang(phpMail.get("defaultLang").getAsString());
            platFromMail.setPassedTimeMs(phpMail.get("passedTimeMs").getAsLong());

            //解析邮件内容
            for (JsonElement mailText : phpMail.get("mailText").getAsJsonArray())
            {
                JsonObject mailTextObj = mailText.getAsJsonObject();
                platFromMail.addText(mailTextObj.get("lang").getAsString()
                        , mailTextObj.get("title").getAsString()
                        , mailTextObj.get("content").getAsString());
            }
            //解析邮件附件
            for (JsonElement itemObj : phpMail.get("itemList").getAsJsonArray())
            {
                JsonObject itemObjAsJsonObject = itemObj.getAsJsonObject();
                //解析物品的类型和id
                NPCommonItem item = new NPCommonItem();
                item.parseFromString(itemObjAsJsonObject.get("subId").getAsString());
                //创建costItem接受物品类型、id、数量
                NPCommonCostItem costItem = new NPCommonCostItem(item.getItemType(), item.getItemId()
                        , itemObjAsJsonObject.get("count").getAsLong());
                platFromMail.addItem(costItem);
            }
            
            //邮件子标题相关数据
            if(phpMail.has("subTitle")) //邮件子标题
            {
            	platFromMail.setSubTitle(phpMail.get("subTitle").getAsString());
            }
            if(phpMail.has("subTitleParams")) //子标题替换文本内容
            {
            	String replace = phpMail.get("subTitleParams").getAsString();
                String[] replaceStrs = CommonFunc.charSplit(replace, '|');
                for(int i = 0; i < replaceStrs.length; i++)
                {
                	platFromMail.getSubTitleReplace().add(replaceStrs[i]);
                }
            }
            
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
