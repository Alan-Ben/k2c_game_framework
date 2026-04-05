package NPHttpServer.Http.Entity;

import NPCommon.CommonObj.NPCommonCostItem;

import java.util.ArrayList;

/**
 * @description: 后台邮件
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityServerMail
{
    /**
     * 后台邮件id
     */
    private long phpMailId;

    /**
     * 邮件配置id
     */
    private long mailRefId;
    /**
     * 玩家看到的邮件接收时间
     */
    private String sendTime;
    /**
     * 失效时间
     */
    private String expiredTime;
    /**
     * 默认语言
     */
    private String defaultLang;

    /**
     * 邮件审核时间，用于邮件组标识位处理
     */
    private long passedTimeMs;

    /**
     * 邮件内容列表
     */
    private ArrayList<NPEntityAllServerMailText> mailTextList;

    /**
     * 邮件附件列表
     */
    private ArrayList<NPCommonCostItem> itemList;
    
    /**
     * 内容替换数据
     */
    private ArrayList<String> contentReplace;
    
    /**
     * 邮件子标题/子标题替换内容
     */
    private String subTitle;
    private ArrayList<String> subTitleReplace;
    

    public NPEntityServerMail()
    {
        mailTextList = new ArrayList<>();
        itemList = new ArrayList<>();
        contentReplace = new ArrayList<>();
        subTitleReplace = new ArrayList<>();
    }

    public long getPhpMailId()
    {
        return phpMailId;
    }
    public void setPhpMailId(long phpMailId)
    {
        this.phpMailId = phpMailId;
    }

    public String getSendTime()
    {
        return sendTime;
    }
    public void setSendTime(String sendTime)
    {
        this.sendTime = sendTime;
    }

    public String getExpiredTime()
    {
        return expiredTime;
    }
    public void setExpiredTime(String expiredTime)
    {
        this.expiredTime = expiredTime;
    }

    public String getDefaultLang()
    {
        return defaultLang;
    }
    public void setDefaultLang(String defaultLang)
    {
        this.defaultLang = defaultLang;
    }

    public long getPassedTimeMs()
    {
        return passedTimeMs;
    }
    public void setPassedTimeMs(long passedTimeMs)
    {
        this.passedTimeMs = passedTimeMs;
    }

    public ArrayList<NPEntityAllServerMailText> getMailTextList()
    {
        return mailTextList;
    }

    public ArrayList<NPCommonCostItem> getItemList()
    {
        return itemList;
    }

    public long getMailRefId()
    {
        return mailRefId;
    }
    public void setMailRefId(long mailRefId)
    {
        this.mailRefId = mailRefId;
    }

    public ArrayList<String> getContentReplace()
    {
        return contentReplace;
    }

    public String getSubTitle()
    {
        return subTitle;
    }
    public void setSubTitle(String subTitle)
    {
        this.subTitle = subTitle;
    }

    public ArrayList<String> getSubTitleReplace()
    {
        return subTitleReplace;
    }

    /**
     * 增加邮件内容
     * @param _lang    后台语言配置
     * @param _title   邮件名称
     * @param _content 邮件内容
     */
    public void addText(String _lang, String _title, String _content)
    {
        NPEntityAllServerMailText text = new NPEntityAllServerMailText();
        text.setLang(_lang);
        text.setTitle(_title);
        text.setContent(_content);
        mailTextList.add(text);
    }

    /**
     * 增加邮件附件
     * @param _costItem 附件物品
     */
    public void addItem(NPCommonCostItem _costItem)
    {
        itemList.add(_costItem);
    }

}
