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
public class GlobalMailBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_senderName =0;
    @DataBaseField(type = "varchar(500)", fieldname = "senderName", comment = "发送者名称")
    private String senderName;

    public static final int FIELD_title =1;
    @DataBaseField(type = "varchar(200)", fieldname = "title", comment = "邮件标题")
    private String title;

    public static final int FIELD_content =2;
    @DataBaseField(type = "varchar(500)", fieldname = "content", comment = "邮件描述")
    private String content;

    public static final int FIELD_createTime =3;
    @DataBaseField(type = "int(11)", fieldname = "createTime", comment = "邮件发送时间(s)")
    private int createTime;

    public static final int FIELD_existTime =4;
    @DataBaseField(type = "int(11)", fieldname = "existTime", comment = "邮件生存时间(s)")
    private int existTime;

    public static final int FIELD_reward =5;
    @DataBaseField(type = "varchar(500)", fieldname = "reward", comment = "奖励信息")
    private String reward;

    public static final int FIELD_sendType =6;
    @DataBaseField(type = "int(11)", fieldname = "sendType", comment = "发送玩家类型0全部玩家1旧玩家2新玩家")
    private int sendType;

    public static final int FIELD_channel_list =7;
    @DataBaseField(type = "varchar(2048)", fieldname = "channel_list", comment = "指定渠道列表，空位全渠道")
    private String channel_list;

    public static final int FIELD_isSend =8;
    @DataBaseField(type = "tinyint(1)", fieldname = "isSend", comment = "列表发送或不发送")
    private boolean isSend;

    public GlobalMailBO() {
        id = 0;
        senderName = "";
        title = "";
        content = "";
        createTime = 0;
        existTime = 0;
        reward = "";
        sendType = 0;
        channel_list = "";
        isSend = false;
    }

    public GlobalMailBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        senderName = rs.getString(2);
        title = rs.getString(3);
        content = rs.getString(4);
        createTime = rs.getInt(5);
        existTime = rs.getInt(6);
        reward = rs.getString(7);
        sendType = rs.getInt(8);
        channel_list = rs.getString(9);
        isSend = rs.getBoolean(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GlobalMailBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `senderName`, `title`, `content`, `createTime`, `existTime`, `reward`, `sendType`, `channel_list`, `isSend`";
    }

    @Override
    public String getTableName() {
        return "`global_mail`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(senderName == null ? null : senderName.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(createTime).append("', ");
        strBuf.append("'").append(existTime).append("', ");
        strBuf.append("'").append(reward == null ? null : reward.replace("'","''").replace("\\","\\\\")).append("', ");
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

    // 发送者名称
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

    // 邮件发送时间(s)
    public int getCreateTime() { return this.createTime; }
    public void setCreateTime(BM _bm, int createTime) {
        if(createTime==this.createTime) 
            return;
        this.createTime = createTime; 
        markField(_bm, FIELD_createTime); 
    }
    public void saveCreateTime(BM _bm, int createTime) {
        if(createTime==this.createTime) 
            return;
        this.createTime = createTime;
        saveField(_bm, "createTime", createTime);
    }

    // 邮件生存时间(s)
    public int getExistTime() { return this.existTime; }
    public void setExistTime(BM _bm, int existTime) {
        if(existTime==this.existTime) 
            return;
        this.existTime = existTime; 
        markField(_bm, FIELD_existTime); 
    }
    public void saveExistTime(BM _bm, int existTime) {
        if(existTime==this.existTime) 
            return;
        this.existTime = existTime;
        saveField(_bm, "existTime", existTime);
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

    // 发送玩家类型0全部玩家1旧玩家2新玩家
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
        sBuilder.append(" `senderName` = '").append(senderName == null ? null : senderName.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `createTime` = '").append(createTime).append("',");
        sBuilder.append(" `existTime` = '").append(existTime).append("',");
        sBuilder.append(" `reward` = '").append(reward == null ? null : reward.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `sendType` = '").append(sendType).append("',");
        sBuilder.append(" `channel_list` = '").append(channel_list == null ? null : channel_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `isSend` = '").append(isSend ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_senderName)) sBuilder.append(" `senderName` = '").append(senderName == null ? null : senderName.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_title)) sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_createTime)) sBuilder.append(" `createTime` = '").append(createTime).append("',");
        if(isFieldMarked(FIELD_existTime)) sBuilder.append(" `existTime` = '").append(existTime).append("',");
        if(isFieldMarked(FIELD_reward)) sBuilder.append(" `reward` = '").append(reward == null ? null : reward.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_sendType)) sBuilder.append(" `sendType` = '").append(sendType).append("',");
        if(isFieldMarked(FIELD_channel_list)) sBuilder.append(" `channel_list` = '").append(channel_list == null ? null : channel_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_isSend)) sBuilder.append(" `isSend` = '").append(isSend ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `global_mail` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`senderName` varchar(500) NOT NULL DEFAULT '' COMMENT '发送者名称',"
                + "`title` varchar(200) NOT NULL DEFAULT '' COMMENT '邮件标题',"
                + "`content` varchar(500) NOT NULL DEFAULT '' COMMENT '邮件描述',"
                + "`createTime` int(11) NOT NULL DEFAULT '0' COMMENT '邮件发送时间(s)',"
                + "`existTime` int(11) NOT NULL DEFAULT '0' COMMENT '邮件生存时间(s)',"
                + "`reward` varchar(500) NOT NULL DEFAULT '' COMMENT '奖励信息',"
                + "`sendType` int(11) NOT NULL DEFAULT '0' COMMENT '发送玩家类型0全部玩家1旧玩家2新玩家',"
                + "`channel_list` varchar(2048) NOT NULL DEFAULT '' COMMENT '指定渠道列表，空位全渠道',"
                + "`isSend` tinyint(1) NOT NULL DEFAULT '0' COMMENT '列表发送或不发送',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='User Info 全局邮件' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);//senderName
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);//title
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
        _size+=4;//createTime
        _size+=4;//existTime
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);//reward
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
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, senderName);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, title);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);
        buff.putInt(createTime);
        buff.putInt(existTime);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, reward);
        buff.putInt(sendType);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, channel_list);
        buff.put((byte)(isSend?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        senderName=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        title=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        createTime=buff.getInt();
        existTime=buff.getInt();
        reward=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
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
