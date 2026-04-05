package USDB.Bo;
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
public class MarsGoRouteStageMsgBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "发送留言的玩家CID")
    private long cid;

    public static final int FIELD_stage =1;
    @DataBaseField(type = "int(11)", fieldname = "stage", comment = "所在阶段号")
    private int stage;

    public static final int FIELD_playerName =2;
    @DataBaseField(type = "varchar(500)", fieldname = "playerName", comment = "玩家名称")
    private String playerName;

    public static final int FIELD_content =3;
    @DataBaseField(type = "varchar(500)", fieldname = "content", comment = "留言内容")
    private String content;

    public static final int FIELD_createdMs =4;
    @DataBaseField(type = "bigint(20)", fieldname = "createdMs", comment = "留言创建时间（毫秒）")
    private long createdMs;

    public MarsGoRouteStageMsgBO() {
        id = 0;
        cid = 0L;
        stage = 0;
        playerName = "";
        content = "";
        createdMs = 0L;
    }

    public MarsGoRouteStageMsgBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        stage = rs.getInt(3);
        playerName = rs.getString(4);
        content = rs.getString(5);
        createdMs = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new MarsGoRouteStageMsgBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `stage`, `playerName`, `content`, `createdMs`";
    }

    @Override
    public String getTableName() {
        return "`mars_go_route_stage_msg`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(stage).append("', ");
        strBuf.append("'").append(playerName == null ? null : playerName.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(createdMs).append("', ");
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

    // 发送留言的玩家CID
    public long getCid() { return this.cid; }
    public void setCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid; 
        markField(_bm, FIELD_cid); 
    }
    public void saveCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid;
        saveField(_bm, "cid", cid);
    }

    // 所在阶段号
    public int getStage() { return this.stage; }
    public void setStage(BM _bm, int stage) {
        if(stage==this.stage) 
            return;
        this.stage = stage; 
        markField(_bm, FIELD_stage); 
    }
    public void saveStage(BM _bm, int stage) {
        if(stage==this.stage) 
            return;
        this.stage = stage;
        saveField(_bm, "stage", stage);
    }

    // 玩家名称
    public String getPlayerName() { return this.playerName; }
    public void setPlayerName(BM _bm, String playerName) {
        if(playerName.equals(this.playerName)) 
            return;
        this.playerName = playerName; 
        markField(_bm, FIELD_playerName); 
    }
    public void savePlayerName(BM _bm, String playerName) {
        if(playerName.equals(this.playerName)) 
            return;
        this.playerName = playerName;
        saveField(_bm, "playerName", playerName);
    }

    // 留言内容
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

    // 留言创建时间（毫秒）
    public long getCreatedMs() { return this.createdMs; }
    public void setCreatedMs(BM _bm, long createdMs) {
        if(createdMs==this.createdMs) 
            return;
        this.createdMs = createdMs; 
        markField(_bm, FIELD_createdMs); 
    }
    public void saveCreatedMs(BM _bm, long createdMs) {
        if(createdMs==this.createdMs) 
            return;
        this.createdMs = createdMs;
        saveField(_bm, "createdMs", createdMs);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `stage` = '").append(stage).append("',");
        sBuilder.append(" `playerName` = '").append(playerName == null ? null : playerName.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `createdMs` = '").append(createdMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_stage)) sBuilder.append(" `stage` = '").append(stage).append("',");
        if(isFieldMarked(FIELD_playerName)) sBuilder.append(" `playerName` = '").append(playerName == null ? null : playerName.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_createdMs)) sBuilder.append(" `createdMs` = '").append(createdMs).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `mars_go_route_stage_msg` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '发送留言的玩家CID',"
                + "`stage` int(11) NOT NULL DEFAULT '0' COMMENT '所在阶段号',"
                + "`playerName` varchar(500) NOT NULL DEFAULT '' COMMENT '玩家名称',"
                + "`content` varchar(500) NOT NULL DEFAULT '' COMMENT '留言内容',"
                + "`createdMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '留言创建时间（毫秒）',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='火星前往路线-阶段留言' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//cid
        _size+=4;//stage
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(playerName);//playerName
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
        _size+=8;//createdMs
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(stage);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, playerName);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);
        buff.putLong(createdMs);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        stage=buff.getInt();
        playerName=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        createdMs=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
