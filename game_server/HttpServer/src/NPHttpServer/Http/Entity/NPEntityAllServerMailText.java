package NPHttpServer.Http.Entity;

/**
 * @description: 后台邮件
 * @author: ricci
 * @date: 2023-03-25 00:08:00
 */
public class NPEntityAllServerMailText
{
    /**
     * 语言编号(查看公共参数中的语言ID对应表)
     */
    private String lang;
    /**
     * 对应语种的邮件标题
     */
    private String title;
    /**
     * 对应语种的邮件内容
     */
    private String content;

    public String getLang()
    {
        return lang;
    }

    public void setLang(String lang)
    {
        this.lang = lang;
    }

    public String getTitle()
    {
        return title;
    }

    public void setTitle(String title)
    {
        this.title = title;
    }

    public String getContent()
    {
        return content;
    }

    public void setContent(String content)
    {
        this.content = content;
    }
}
