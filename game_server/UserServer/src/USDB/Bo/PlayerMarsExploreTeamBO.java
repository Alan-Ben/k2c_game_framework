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
public class PlayerMarsExploreTeamBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_teamId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "teamId", comment = "队伍ID")
    private long teamId;

    public static final int FIELD_curState =2;
    @DataBaseField(type = "int(11)", fieldname = "curState", comment = "当前状态")
    private int curState;

    public static final int FIELD_name =3;
    @DataBaseField(type = "varchar(500)", fieldname = "name", comment = "队伍名称")
    private String name;

    public static final int FIELD_heroIdList =4;
    @DataBaseField(type = "blob", fieldname = "heroIdList", comment = "入驻大臣ID列表")
    private byte[] heroIdList;

    public static final int FIELD_curStateStartMs =5;
    @DataBaseField(type = "bigint(20)", fieldname = "curStateStartMs", comment = "当前状态起始时间（毫秒）")
    private long curStateStartMs;

    public static final int FIELD_curStateKeepTimeMS =6;
    @DataBaseField(type = "bigint(20)", fieldname = "curStateKeepTimeMS", comment = "当前状态持续时长（毫秒）")
    private long curStateKeepTimeMS;

    public static final int FIELD_targetPos =7;
    @DataBaseField(type = "bigint(20)", fieldname = "targetPos", comment = "目标位置")
    private long targetPos;

    public static final int FIELD_extData =8;
    @DataBaseField(type = "blob", fieldname = "extData", comment = "当前状态额外数据")
    private byte[] extData;

    public static final int FIELD_lossValue =9;
    @DataBaseField(type = "bigint(20)", fieldname = "lossValue", comment = "队伍损耗数量")
    private long lossValue;

    public PlayerMarsExploreTeamBO() {
        id = 0;
        cid = 0L;
        teamId = 0L;
        curState = 0;
        name = "";
        heroIdList = null;
        curStateStartMs = 0L;
        curStateKeepTimeMS = 0L;
        targetPos = 0L;
        extData = null;
        lossValue = 0L;
    }

    public PlayerMarsExploreTeamBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        teamId = rs.getLong(3);
        curState = rs.getInt(4);
        name = rs.getString(5);
        heroIdList = rs.getBytes(6);
        curStateStartMs = rs.getLong(7);
        curStateKeepTimeMS = rs.getLong(8);
        targetPos = rs.getLong(9);
        extData = rs.getBytes(10);
        lossValue = rs.getLong(11);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsExploreTeamBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `teamId`, `curState`, `name`, `heroIdList`, `curStateStartMs`, `curStateKeepTimeMS`, `targetPos`, `extData`, `lossValue`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_explore_team`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(teamId).append("', ");
        strBuf.append("'").append(curState).append("', ");
        strBuf.append("'").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(curStateStartMs).append("', ");
        strBuf.append("'").append(curStateKeepTimeMS).append("', ");
        strBuf.append("'").append(targetPos).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(lossValue).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(heroIdList); 
        ret.add(extData);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_heroIdList)) ret.add(heroIdList); 
        if(isFieldMarked(FIELD_extData)) ret.add(extData);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 玩家CID
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

    // 队伍ID
    public long getTeamId() { return this.teamId; }
    public void setTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId; 
        markField(_bm, FIELD_teamId); 
    }
    public void saveTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId;
        saveField(_bm, "teamId", teamId);
    }

    // 当前状态
    public int getCurState() { return this.curState; }
    public void setCurState(BM _bm, int curState) {
        if(curState==this.curState) 
            return;
        this.curState = curState; 
        markField(_bm, FIELD_curState); 
    }
    public void saveCurState(BM _bm, int curState) {
        if(curState==this.curState) 
            return;
        this.curState = curState;
        saveField(_bm, "curState", curState);
    }

    // 队伍名称
    public String getName() { return this.name; }
    public void setName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name; 
        markField(_bm, FIELD_name); 
    }
    public void saveName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name;
        saveField(_bm, "name", name);
    }

    // 入驻大臣ID列表
    public byte[] getHeroIdList() { return this.heroIdList; }
    public void setHeroIdList(BM _bm, byte[] heroIdList) {
        if(heroIdList==this.heroIdList) 
            return;
        this.heroIdList = heroIdList; 
        markField(_bm, FIELD_heroIdList); 
    }
    public void saveHeroIdList(BM _bm, byte[] heroIdList) {
        if(heroIdList==this.heroIdList) 
            return;
        this.heroIdList = heroIdList;
        saveFieldBytes(_bm, "heroIdList", heroIdList);
    }

    // 当前状态起始时间（毫秒）
    public long getCurStateStartMs() { return this.curStateStartMs; }
    public void setCurStateStartMs(BM _bm, long curStateStartMs) {
        if(curStateStartMs==this.curStateStartMs) 
            return;
        this.curStateStartMs = curStateStartMs; 
        markField(_bm, FIELD_curStateStartMs); 
    }
    public void saveCurStateStartMs(BM _bm, long curStateStartMs) {
        if(curStateStartMs==this.curStateStartMs) 
            return;
        this.curStateStartMs = curStateStartMs;
        saveField(_bm, "curStateStartMs", curStateStartMs);
    }

    // 当前状态持续时长（毫秒）
    public long getCurStateKeepTimeMS() { return this.curStateKeepTimeMS; }
    public void setCurStateKeepTimeMS(BM _bm, long curStateKeepTimeMS) {
        if(curStateKeepTimeMS==this.curStateKeepTimeMS) 
            return;
        this.curStateKeepTimeMS = curStateKeepTimeMS; 
        markField(_bm, FIELD_curStateKeepTimeMS); 
    }
    public void saveCurStateKeepTimeMS(BM _bm, long curStateKeepTimeMS) {
        if(curStateKeepTimeMS==this.curStateKeepTimeMS) 
            return;
        this.curStateKeepTimeMS = curStateKeepTimeMS;
        saveField(_bm, "curStateKeepTimeMS", curStateKeepTimeMS);
    }

    // 目标位置
    public long getTargetPos() { return this.targetPos; }
    public void setTargetPos(BM _bm, long targetPos) {
        if(targetPos==this.targetPos) 
            return;
        this.targetPos = targetPos; 
        markField(_bm, FIELD_targetPos); 
    }
    public void saveTargetPos(BM _bm, long targetPos) {
        if(targetPos==this.targetPos) 
            return;
        this.targetPos = targetPos;
        saveField(_bm, "targetPos", targetPos);
    }

    // 当前状态额外数据
    public byte[] getExtData() { return this.extData; }
    public void setExtData(BM _bm, byte[] extData) {
        if(extData==this.extData) 
            return;
        this.extData = extData; 
        markField(_bm, FIELD_extData); 
    }
    public void saveExtData(BM _bm, byte[] extData) {
        if(extData==this.extData) 
            return;
        this.extData = extData;
        saveFieldBytes(_bm, "extData", extData);
    }

    // 队伍损耗数量
    public long getLossValue() { return this.lossValue; }
    public void setLossValue(BM _bm, long lossValue) {
        if(lossValue==this.lossValue) 
            return;
        this.lossValue = lossValue; 
        markField(_bm, FIELD_lossValue); 
    }
    public void saveLossValue(BM _bm, long lossValue) {
        if(lossValue==this.lossValue) 
            return;
        this.lossValue = lossValue;
        saveField(_bm, "lossValue", lossValue);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `teamId` = '").append(teamId).append("',");
        sBuilder.append(" `curState` = '").append(curState).append("',");
        sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `heroIdList` = ?,");
        sBuilder.append(" `curStateStartMs` = '").append(curStateStartMs).append("',");
        sBuilder.append(" `curStateKeepTimeMS` = '").append(curStateKeepTimeMS).append("',");
        sBuilder.append(" `targetPos` = '").append(targetPos).append("',");
        sBuilder.append(" `extData` = ?,");
        sBuilder.append(" `lossValue` = '").append(lossValue).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_teamId)) sBuilder.append(" `teamId` = '").append(teamId).append("',");
        if(isFieldMarked(FIELD_curState)) sBuilder.append(" `curState` = '").append(curState).append("',");
        if(isFieldMarked(FIELD_name)) sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_heroIdList)) sBuilder.append(" `heroIdList` = ?,");
        if(isFieldMarked(FIELD_curStateStartMs)) sBuilder.append(" `curStateStartMs` = '").append(curStateStartMs).append("',");
        if(isFieldMarked(FIELD_curStateKeepTimeMS)) sBuilder.append(" `curStateKeepTimeMS` = '").append(curStateKeepTimeMS).append("',");
        if(isFieldMarked(FIELD_targetPos)) sBuilder.append(" `targetPos` = '").append(targetPos).append("',");
        if(isFieldMarked(FIELD_extData)) sBuilder.append(" `extData` = ?,");
        if(isFieldMarked(FIELD_lossValue)) sBuilder.append(" `lossValue` = '").append(lossValue).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_explore_team` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`teamId` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍ID',"
                + "`curState` int(11) NOT NULL DEFAULT '0' COMMENT '当前状态',"
                + "`name` varchar(500) NOT NULL DEFAULT '' COMMENT '队伍名称',"
                + "`heroIdList` blob NULL COMMENT '入驻大臣ID列表',"
                + "`curStateStartMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前状态起始时间（毫秒）',"
                + "`curStateKeepTimeMS` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前状态持续时长（毫秒）',"
                + "`targetPos` bigint(20) NOT NULL DEFAULT '0' COMMENT '目标位置',"
                + "`extData` blob NULL COMMENT '当前状态额外数据',"
                + "`lossValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '队伍损耗数量',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家火星探险队伍数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//teamId
        _size+=4;//curState
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);//name
        _size+=2;_size+=heroIdList.length;//heroIdList
        _size+=8;//curStateStartMs
        _size+=8;//curStateKeepTimeMS
        _size+=8;//targetPos
        _size+=2;_size+=extData.length;//extData
        _size+=8;//lossValue
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(teamId);
        buff.putInt(curState);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, name);
        buff.putShort((short)(heroIdList == null ? 0 : heroIdList.length));if(null != heroIdList){buff.put(heroIdList);}
        buff.putLong(curStateStartMs);
        buff.putLong(curStateKeepTimeMS);
        buff.putLong(targetPos);
        buff.putShort((short)(extData == null ? 0 : extData.length));if(null != extData){buff.put(extData);}
        buff.putLong(lossValue);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        teamId=buff.getLong();
        curState=buff.getInt();
        name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        int heroIdList_count = buff.getShort();if(heroIdList_count>0){heroIdList = new byte[heroIdList_count];buff.get(heroIdList);}
        curStateStartMs=buff.getLong();
        curStateKeepTimeMS=buff.getLong();
        targetPos=buff.getLong();
        int extData_count = buff.getShort();if(extData_count>0){extData = new byte[extData_count];buff.get(extData);}
        lossValue=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
