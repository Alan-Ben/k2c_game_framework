package CSDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class FixedtimeMailBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_mailId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "mailId", comment = "php邮件ID")
    private long mailId;

    public static final int FIELD_senderName =1;
    @DataBaseField(type = "varchar(500)", fieldname = "senderName", comment = "发送者")
    private String senderName;

    public static final int FIELD_title =2;
    @DataBaseField(type = "varchar(500)", fieldname = "title", comment = "邮件标题")
    private String title;

    public static final int FIELD_content =3;
    @DataBaseField(type = "varchar(500)", fieldname = "content", comment = "邮件描述")
    private String content;

    public static final int FIELD_reward =4;
    @DataBaseField(type = "varchar(500)", fieldname = "reward", comment = "奖励信息")
    private String reward;

    public static final int FIELD_beginTime =5;
    @DataBaseField(type = "int(11)", fieldname = "beginTime", comment = "开始时间")
    private int beginTime;

    public static final int FIELD_hasSendNum =6;
    @DataBaseField(type = "int(11)", fieldname = "hasSendNum", comment = "已发次数")
    private int hasSendNum;

    public static final int FIELD_cyclesNum =7;
    @DataBaseField(type = "int(11)", fieldname = "cyclesNum", comment = "循环次数")
    private int cyclesNum;

    public static final int FIELD_exitstime =8;
    @DataBaseField(type = "int(11)", fieldname = "exitstime", comment = "邮件有效时长，秒")
    private int exitstime;

    public static final int FIELD_sendType =9;
    @DataBaseField(type = "int(11)", fieldname = "sendType", comment = "发送玩家类型:0全部玩家 1旧玩家 2新玩家")
    private int sendType;

    public static final int FIELD_channel_list =10;
    @DataBaseField(type = "varchar(2048)", fieldname = "channel_list", comment = "指定渠道列表，空位全渠道")
    private String channel_list;

    public static final int FIELD_isSend =11;
    @DataBaseField(type = "tinyint(1)", fieldname = "isSend", comment = "列表发送或不发送")
    private boolean isSend;

    public FixedtimeMailBO() {
        id = 0;
        mailId = 0L;
        senderName = "";
        title = "";
        content = "";
        reward = "";
        beginTime = 0;
        hasSendNum = 0;
        cyclesNum = 0;
        exitstime = 0;
        sendType = 0;
        channel_list = "";
        isSend = false;
    }

    public FixedtimeMailBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        mailId = rs.getLong(2);
        senderName = rs.getString(3);
        title = rs.getString(4);
        content = rs.getString(5);
        reward = rs.getString(6);
        beginTime = rs.getInt(7);
        hasSendNum = rs.getInt(8);
        cyclesNum = rs.getInt(9);
        exitstime = rs.getInt(10);
        sendType = rs.getInt(11);
        channel_list = rs.getString(12);
        isSend = rs.getBoolean(13);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new FixedtimeMailBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `mailId`, `senderName`, `title`, `content`, `reward`, `beginTime`, `hasSendNum`, `cyclesNum`, `exitstime`, `sendType`, `channel_list`, `isSend`";
    }

    @Override
    public String getTableName() {
        return "`fixedtime_mail`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(mailId).append("', ");
        strBuf.append("'").append(senderName == null ? null : senderName.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(reward == null ? null : reward.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(beginTime).append("', ");
        strBuf.append("'").append(hasSendNum).append("', ");
        strBuf.append("'").append(cyclesNum).append("', ");
        strBuf.append("'").append(exitstime).append("', ");
        strBuf.append("'").append(sendType).append("', ");
        strBuf.append("'").append(channel_list == null ? null : channel_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(isSend ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // php邮件ID
    public long getMailId() { return this.mailId; }
    public void setMailId(BM _bm, long mailId) {
        if(mailId==this.mailId) 
            return;
        this.mailId = mailId; 
        markField(_bm, FIELD_mailId); 
    }
    public void saveMailId(BM _bm, long mailId) {
        if(mailId==this.mailId) 
            return;
        this.mailId = mailId;
        saveField(_bm, "mailId", mailId);
    }

    // 发送者
    public String getSenderName() { return this.senderName; }
    public void setSenderName(BM _bm, String senderName) {
        if(senderName.equals(this.senderName)) 
            return;
        this.senderName = senderName; 
        markField(_bm, FIELD_senderName); 
    }
    public void saveSenderName(BM _bm, String senderName) {
        if(senderName.equals(this.senderName)) 
            return;
        this.senderName = senderName;
        saveField(_bm, "senderName", senderName);
    }

    // 邮件标题
    public String getTitle() { return this.title; }
    public void setTitle(BM _bm, String title) {
        if(title.equals(this.title)) 
            return;
        this.title = title; 
        markField(_bm, FIELD_title); 
    }
    public void saveTitle(BM _bm, String title) {
        if(title.equals(this.title)) 
            return;
        this.title = title;
        saveField(_bm, "title", title);
    }

    // 邮件描述
    public String getContent() { return this.content; }
    public void setContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content; 
        markField(_bm, FIELD_content); 
    }
    public void saveContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content;
        saveField(_bm, "content", content);
    }

    // 奖励信息
    public String getReward() { return this.reward; }
    public void setReward(BM _bm, String reward) {
        if(reward.equals(this.reward)) 
            return;
        this.reward = reward; 
        markField(_bm, FIELD_reward); 
    }
    public void saveReward(BM _bm, String reward) {
        if(reward.equals(this.reward)) 
            return;
        this.reward = reward;
        saveField(_bm, "reward", reward);
    }

    // 开始时间
    public int getBeginTime() { return this.beginTime; }
    public void setBeginTime(BM _bm, int beginTime) {
        if(beginTime==this.beginTime) 
            return;
        this.beginTime = beginTime; 
        markField(_bm, FIELD_beginTime); 
    }
    public void saveBeginTime(BM _bm, int beginTime) {
        if(beginTime==this.beginTime) 
            return;
        this.beginTime = beginTime;
        saveField(_bm, "beginTime", beginTime);
    }

    // 已发次数
    public int getHasSendNum() { return this.hasSendNum; }
    public void setHasSendNum(BM _bm, int hasSendNum) {
        if(hasSendNum==this.hasSendNum) 
            return;
        this.hasSendNum = hasSendNum; 
        markField(_bm, FIELD_hasSendNum); 
    }
    public void saveHasSendNum(BM _bm, int hasSendNum) {
        if(hasSendNum==this.hasSendNum) 
            return;
        this.hasSendNum = hasSendNum;
        saveField(_bm, "hasSendNum", hasSendNum);
    }

    // 循环次数
    public int getCyclesNum() { return this.cyclesNum; }
    public void setCyclesNum(BM _bm, int cyclesNum) {
        if(cyclesNum==this.cyclesNum) 
            return;
        this.cyclesNum = cyclesNum; 
        markField(_bm, FIELD_cyclesNum); 
    }
    public void saveCyclesNum(BM _bm, int cyclesNum) {
        if(cyclesNum==this.cyclesNum) 
            return;
        this.cyclesNum = cyclesNum;
        saveField(_bm, "cyclesNum", cyclesNum);
    }

    // 邮件有效时长，秒
    public int getExitstime() { return this.exitstime; }
    public void setExitstime(BM _bm, int exitstime) {
        if(exitstime==this.exitstime) 
            return;
        this.exitstime = exitstime; 
        markField(_bm, FIELD_exitstime); 
    }
    public void saveExitstime(BM _bm, int exitstime) {
        if(exitstime==this.exitstime) 
            return;
        this.exitstime = exitstime;
        saveField(_bm, "exitstime", exitstime);
    }

    // 发送玩家类型:0全部玩家 1旧玩家 2新玩家
    public int getSendType() { return this.sendType; }
    public void setSendType(BM _bm, int sendType) {
        if(sendType==this.sendType) 
            return;
        this.sendType = sendType; 
        markField(_bm, FIELD_sendType); 
    }
    public void saveSendType(BM _bm, int sendType) {
        if(sendType==this.sendType) 
            return;
        this.sendType = sendType;
        saveField(_bm, "sendType", sendType);
    }

    // 指定渠道列表，空位全渠道
    public String getChannelList() { return this.channel_list; }
    public void setChannelList(BM _bm, String channel_list) {
        if(channel_list.equals(this.channel_list)) 
            return;
        this.channel_list = channel_list; 
        markField(_bm, FIELD_channel_list); 
    }
    public void saveChannelList(BM _bm, String channel_list) {
        if(channel_list.equals(this.channel_list)) 
            return;
        this.channel_list = channel_list;
        saveField(_bm, "channel_list", channel_list);
    }

    // 列表发送或不发送
    public boolean getIsSend() { return this.isSend; }
    public void setIsSend(BM _bm, boolean isSend) {
        if(isSend==this.isSend) 
            return;
        this.isSend = isSend; 
        markField(_bm, FIELD_isSend); 
    }
    public void saveIsSend(BM _bm, boolean isSend) {
        if(isSend==this.isSend) 
            return;
        this.isSend = isSend;
        saveField(_bm, "isSend", isSend ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `mailId` = '").append(mailId).append("',");
        sBuilder.append(" `senderName` = '").append(senderName == null ? null : senderName.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `reward` = '").append(reward == null ? null : reward.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `beginTime` = '").append(beginTime).append("',");
        sBuilder.append(" `hasSendNum` = '").append(hasSendNum).append("',");
        sBuilder.append(" `cyclesNum` = '").append(cyclesNum).append("',");
        sBuilder.append(" `exitstime` = '").append(exitstime).append("',");
        sBuilder.append(" `sendType` = '").append(sendType).append("',");
        sBuilder.append(" `channel_list` = '").append(channel_list == null ? null : channel_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `isSend` = '").append(isSend ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_mailId)) sBuilder.append(" `mailId` = '").append(mailId).append("',");
        if(isFieldMarked(FIELD_senderName)) sBuilder.append(" `senderName` = '").append(senderName == null ? null : senderName.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_title)) sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_reward)) sBuilder.append(" `reward` = '").append(reward == null ? null : reward.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_beginTime)) sBuilder.append(" `beginTime` = '").append(beginTime).append("',");
        if(isFieldMarked(FIELD_hasSendNum)) sBuilder.append(" `hasSendNum` = '").append(hasSendNum).append("',");
        if(isFieldMarked(FIELD_cyclesNum)) sBuilder.append(" `cyclesNum` = '").append(cyclesNum).append("',");
        if(isFieldMarked(FIELD_exitstime)) sBuilder.append(" `exitstime` = '").append(exitstime).append("',");
        if(isFieldMarked(FIELD_sendType)) sBuilder.append(" `sendType` = '").append(sendType).append("',");
        if(isFieldMarked(FIELD_channel_list)) sBuilder.append(" `channel_list` = '").append(channel_list == null ? null : channel_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_isSend)) sBuilder.append(" `isSend` = '").append(isSend ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `fixedtime_mail` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`mailId` bigint(20) NOT NULL DEFAULT '0' COMMENT 'php邮件ID',"
                + "`senderName` varchar(500) NOT NULL DEFAULT '' COMMENT '发送者',"
                + "`title` varchar(500) NOT NULL DEFAULT '' COMMENT '邮件标题',"
                + "`content` varchar(500) NOT NULL DEFAULT '' COMMENT '邮件描述',"
                + "`reward` varchar(500) NOT NULL DEFAULT '' COMMENT '奖励信息',"
                + "`beginTime` int(11) NOT NULL DEFAULT '0' COMMENT '开始时间',"
                + "`hasSendNum` int(11) NOT NULL DEFAULT '0' COMMENT '已发次数',"
                + "`cyclesNum` int(11) NOT NULL DEFAULT '0' COMMENT '循环次数',"
                + "`exitstime` int(11) NOT NULL DEFAULT '0' COMMENT '邮件有效时长，秒',"
                + "`sendType` int(11) NOT NULL DEFAULT '0' COMMENT '发送玩家类型:0全部玩家 1旧玩家 2新玩家',"
                + "`channel_list` varchar(2048) NOT NULL DEFAULT '' COMMENT '指定渠道列表，空位全渠道',"
                + "`isSend` tinyint(1) NOT NULL DEFAULT '0' COMMENT '列表发送或不发送',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Method 后台推送定时邮件' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//mailId
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);//senderName
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);//title
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);//reward
        _size+=4;//beginTime
        _size+=4;//hasSendNum
        _size+=4;//cyclesNum
        _size+=4;//exitstime
        _size+=4;//sendType
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(channel_list);//channel_list
        _size+=1;//isSend
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(mailId);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, senderName);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, title);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, reward);
        buff.putInt(beginTime);
        buff.putInt(hasSendNum);
        buff.putInt(cyclesNum);
        buff.putInt(exitstime);
        buff.putInt(sendType);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, channel_list);
        buff.put((byte)(isSend?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        mailId=buff.getLong();
        senderName=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        title=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        reward=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        beginTime=buff.getInt();
        hasSendNum=buff.getInt();
        cyclesNum=buff.getInt();
        exitstime=buff.getInt();
        sendType=buff.getInt();
        channel_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        isSend=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
