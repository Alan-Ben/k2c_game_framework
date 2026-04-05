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
public class ArenaCelebrityRankBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_attacker_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "attacker_cid", comment = "挑战者角色ID")
    private long attacker_cid;

    public static final int FIELD_attacker_name =1;
    @DataBaseField(type = "varchar(128)", fieldname = "attacker_name", comment = "挑战者角色名")
    private String attacker_name;

    public static final int FIELD_defender_name =2;
    @DataBaseField(type = "varchar(128)", fieldname = "defender_name", comment = "被挑战者角色名")
    private String defender_name;

    public static final int FIELD_defeat_hero_num =3;
    @DataBaseField(type = "int(11)", fieldname = "defeat_hero_num", comment = "击败对方英雄数量")
    private int defeat_hero_num;

    public static final int FIELD_is_select_attack =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_select_attack", comment = "是否选择挑战")
    private boolean is_select_attack;

    public static final int FIELD_timestamp =5;
    @DataBaseField(type = "bigint(20)", fieldname = "timestamp", comment = "时间戳")
    private long timestamp;

    public ArenaCelebrityRankBO() {
        id = 0;
        attacker_cid = 0L;
        attacker_name = "";
        defender_name = "";
        defeat_hero_num = 0;
        is_select_attack = false;
        timestamp = 0L;
    }

    public ArenaCelebrityRankBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        attacker_cid = rs.getLong(2);
        attacker_name = rs.getString(3);
        defender_name = rs.getString(4);
        defeat_hero_num = rs.getInt(5);
        is_select_attack = rs.getBoolean(6);
        timestamp = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new ArenaCelebrityRankBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `attacker_cid`, `attacker_name`, `defender_name`, `defeat_hero_num`, `is_select_attack`, `timestamp`";
    }

    @Override
    public String getTableName() {
        return "`arena_celebrity_rank`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(attacker_cid).append("', ");
        strBuf.append("'").append(attacker_name == null ? null : attacker_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(defender_name == null ? null : defender_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(defeat_hero_num).append("', ");
        strBuf.append("'").append(is_select_attack ? 1 : 0).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
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

    // 挑战者角色ID
    public long getAttackerCid() { return this.attacker_cid; }
    public void setAttackerCid(BM _bm, long attacker_cid) {
        if(attacker_cid==this.attacker_cid) 
            return;
        this.attacker_cid = attacker_cid; 
        markField(_bm, FIELD_attacker_cid); 
    }
    public void saveAttackerCid(BM _bm, long attacker_cid) {
        if(attacker_cid==this.attacker_cid) 
            return;
        this.attacker_cid = attacker_cid;
        saveField(_bm, "attacker_cid", attacker_cid);
    }

    // 挑战者角色名
    public String getAttackerName() { return this.attacker_name; }
    public void setAttackerName(BM _bm, String attacker_name) {
        if(attacker_name.equals(this.attacker_name)) 
            return;
        this.attacker_name = attacker_name; 
        markField(_bm, FIELD_attacker_name); 
    }
    public void saveAttackerName(BM _bm, String attacker_name) {
        if(attacker_name.equals(this.attacker_name)) 
            return;
        this.attacker_name = attacker_name;
        saveField(_bm, "attacker_name", attacker_name);
    }

    // 被挑战者角色名
    public String getDefenderName() { return this.defender_name; }
    public void setDefenderName(BM _bm, String defender_name) {
        if(defender_name.equals(this.defender_name)) 
            return;
        this.defender_name = defender_name; 
        markField(_bm, FIELD_defender_name); 
    }
    public void saveDefenderName(BM _bm, String defender_name) {
        if(defender_name.equals(this.defender_name)) 
            return;
        this.defender_name = defender_name;
        saveField(_bm, "defender_name", defender_name);
    }

    // 击败对方英雄数量
    public int getDefeatHeroNum() { return this.defeat_hero_num; }
    public void setDefeatHeroNum(BM _bm, int defeat_hero_num) {
        if(defeat_hero_num==this.defeat_hero_num) 
            return;
        this.defeat_hero_num = defeat_hero_num; 
        markField(_bm, FIELD_defeat_hero_num); 
    }
    public void saveDefeatHeroNum(BM _bm, int defeat_hero_num) {
        if(defeat_hero_num==this.defeat_hero_num) 
            return;
        this.defeat_hero_num = defeat_hero_num;
        saveField(_bm, "defeat_hero_num", defeat_hero_num);
    }

    // 是否选择挑战
    public boolean getIsSelectAttack() { return this.is_select_attack; }
    public void setIsSelectAttack(BM _bm, boolean is_select_attack) {
        if(is_select_attack==this.is_select_attack) 
            return;
        this.is_select_attack = is_select_attack; 
        markField(_bm, FIELD_is_select_attack); 
    }
    public void saveIsSelectAttack(BM _bm, boolean is_select_attack) {
        if(is_select_attack==this.is_select_attack) 
            return;
        this.is_select_attack = is_select_attack;
        saveField(_bm, "is_select_attack", is_select_attack ? 1 : 0);
    }

    // 时间戳
    public long getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, long timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `attacker_cid` = '").append(attacker_cid).append("',");
        sBuilder.append(" `attacker_name` = '").append(attacker_name == null ? null : attacker_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `defender_name` = '").append(defender_name == null ? null : defender_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `defeat_hero_num` = '").append(defeat_hero_num).append("',");
        sBuilder.append(" `is_select_attack` = '").append(is_select_attack ? 1 : 0).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_attacker_cid)) sBuilder.append(" `attacker_cid` = '").append(attacker_cid).append("',");
        if(isFieldMarked(FIELD_attacker_name)) sBuilder.append(" `attacker_name` = '").append(attacker_name == null ? null : attacker_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_defender_name)) sBuilder.append(" `defender_name` = '").append(defender_name == null ? null : defender_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_defeat_hero_num)) sBuilder.append(" `defeat_hero_num` = '").append(defeat_hero_num).append("',");
        if(isFieldMarked(FIELD_is_select_attack)) sBuilder.append(" `is_select_attack` = '").append(is_select_attack ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `arena_celebrity_rank` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`attacker_cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '挑战者角色ID',"
                + "`attacker_name` varchar(128) NOT NULL DEFAULT '' COMMENT '挑战者角色名',"
                + "`defender_name` varchar(128) NOT NULL DEFAULT '' COMMENT '被挑战者角色名',"
                + "`defeat_hero_num` int(11) NOT NULL DEFAULT '0' COMMENT '击败对方英雄数量',"
                + "`is_select_attack` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否选择挑战',"
                + "`timestamp` bigint(20) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='竞技场名人榜数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//attacker_cid
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attacker_name);//attacker_name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(defender_name);//defender_name
        _size+=4;//defeat_hero_num
        _size+=1;//is_select_attack
        _size+=8;//timestamp
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(attacker_cid);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, attacker_name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, defender_name);
        buff.putInt(defeat_hero_num);
        buff.put((byte)(is_select_attack?1:0));
        buff.putLong(timestamp);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        attacker_cid=buff.getLong();
        attacker_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        defender_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        defeat_hero_num=buff.getInt();
        is_select_attack=(buff.get()==1);
        timestamp=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
