package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSCreateTeam_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组实例ID */
private long groupId;
/** 队长玩家CID */
private long cid;
/** 队伍成员数量上限 */
private int memberLimit;
/** 加入方式 */
private Common.CrossTeamEnum.ENPCrossTeamJoinType joinType;
/** 申请条件 */
private Common.CrossTeamObj.CrossTeam_SetInfo_Join joinCond;
/** 队伍名称 */
private String teamName;
/** 队伍宣言 */
private String teamDec;


public CTSCreateTeam_Req() {
	groupId = (long)0;
	cid = (long)0;
	memberLimit = 0;
	joinType = Common.CrossTeamEnum.ENPCrossTeamJoinType.values()[0];
	joinCond = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
	teamName = "";
	teamDec = "";
}

public CTSCreateTeam_Req(
	 long _groupId
	, long _cid
	, int _memberLimit
	, Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType
	, Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond
	, String _teamName
	, String _teamDec
) {	groupId = _groupId;
	cid = _cid;
	memberLimit = _memberLimit;
	joinType = _joinType;
	joinCond = _joinCond;
	teamName = _teamName;
	teamDec = _teamDec;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组实例ID */
public long getGroupId() { return groupId; }
/** 分组实例ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 队长玩家CID */
public long getCid() { return cid; }
/** 队长玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 队伍成员数量上限 */
public int getMemberLimit() { return memberLimit; }
/** 队伍成员数量上限 */
public void setMemberLimit(int _memberLimit) { memberLimit = _memberLimit; }
/** 加入方式 */
public Common.CrossTeamEnum.ENPCrossTeamJoinType getJoinType() { return joinType; }
/** 加入方式 */
public void setJoinType(Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType) { joinType = _joinType; }
/** 申请条件 */
public Common.CrossTeamObj.CrossTeam_SetInfo_Join getJoinCond() { return joinCond; }
/** 申请条件 */
public void setJoinCond(Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond) { joinCond = _joinCond; }
/** 队伍名称 */
public String getTeamName() { return teamName; }
/** 队伍名称 */
public void setTeamName(String _teamName) { teamName = _teamName; }
/** 队伍宣言 */
public String getTeamDec() { return teamDec; }
/** 队伍宣言 */
public void setTeamDec(String _teamDec) { teamDec = _teamDec; }


public final int GetBufSize() {
	int _size = 40;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) memberLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinType = Common.CrossTeamEnum.ENPCrossTeamJoinType.ENPCrossTeamJoinType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _joinCondCustLen = _buf.getInt();
	int _joinCondCurPos = _buf.position();
	joinCond.ReadUnzipBuf(_buf, _joinCondCurPos + _joinCondCustLen);
	_buf.position(_joinCondCurPos + _joinCondCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamDec = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(cid);
	_buf.putInt(memberLimit);
	_buf.putInt(joinType.ordinal());

	_buf.putInt(joinCond.GetBufSize());
	joinCond.PutUnzipBuf(_buf);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, teamName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, teamDec);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

