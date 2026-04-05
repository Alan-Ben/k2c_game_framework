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
public class PlayerReportBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家账号ID")
    private long cid;

    public static final int FIELD_target_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "target_cid", comment = "目标玩家CID")
    private long target_cid;

    public static final int FIELD_content =2;
    @DataBaseField(type = "text", fieldname = "content", comment = "举报内容")
    private String content;

    public static final int FIELD_report_ms =3;
    @DataBaseField(type = "bigint(20)", fieldname = "report_ms", comment = "举报时间（毫秒）")
    private long report_ms;

    public PlayerReportBO() {
        id = 0;
        cid = 0L;
        target_cid = 0L;
        content = "";
        report_ms = 0L;
    }

    public PlayerReportBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        target_cid = rs.getLong(3);
        content = rs.getString(4);
        report_ms = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerReportBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `target_cid`, `content`, `report_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_report`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(target_cid).append("', ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(report_ms).append("', ");
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

    // 玩家账号ID
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

    // 目标玩家CID
    public long getTargetCid() { return this.target_cid; }
    public void setTargetCid(BM _bm, long target_cid) {
        if(target_cid==this.target_cid) 
            return;
        this.target_cid = target_cid; 
        markField(_bm, FIELD_target_cid); 
    }
    public void saveTargetCid(BM _bm, long target_cid) {
        if(target_cid==this.target_cid) 
            return;
        this.target_cid = target_cid;
        saveField(_bm, "target_cid", target_cid);
    }

    // 举报内容
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

    // 举报时间（毫秒）
    public long getReportMs() { return this.report_ms; }
    public void setReportMs(BM _bm, long report_ms) {
        if(report_ms==this.report_ms) 
            return;
        this.report_ms = report_ms; 
        markField(_bm, FIELD_report_ms); 
    }
    public void saveReportMs(BM _bm, long report_ms) {
        if(report_ms==this.report_ms) 
            return;
        this.report_ms = report_ms;
        saveField(_bm, "report_ms", report_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `target_cid` = '").append(target_cid).append("',");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `report_ms` = '").append(report_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_target_cid)) sBuilder.append(" `target_cid` = '").append(target_cid).append("',");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_report_ms)) sBuilder.append(" `report_ms` = '").append(report_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_report` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家账号ID',"
                + "`target_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '目标玩家CID',"
                + "`content` text NULL COMMENT '举报内容',"
                + "`report_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '举报时间（毫秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家举报数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//target_cid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
        _size+=8;//report_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(target_cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);
        buff.putLong(report_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        target_cid=buff.getLong();
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        report_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
