package GC2GS.p012_ActivityTeamOp;

import java.nio.ByteBuffer;
/*********
 * 创建队伍
 **/
public class GC2GS_012_005_ReqCreateActivityTeam implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long instanceId;
/** 加入方式 */
private Common.CrossTeamEnum.ENPCrossTeamJoinType joinType;
/** 申请条件 */
private Common.CrossTeamObj.CrossTeam_SetInfo_Join joinCond;
/** 队伍名称 */
private String teamName;
/** 队伍宣言 */
private String teamDec;


public GC2GS_012_005_ReqCreateActivityTeam() {
	instanceId = (long)0;
	joinType = Common.CrossTeamEnum.ENPCrossTeamJoinType.values()[0];
	joinCond = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
	teamName = "";
	teamDec = "";
}

public GC2GS_012_005_ReqCreateActivityTeam(
	 long _instanceId
	, Common.CrossTeamEnum.ENPCrossTeamJoinType _joinType
	, Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond
	, String _teamName
	, String _teamDec
) {	instanceId = _instanceId;
	joinType = _joinType;
	joinCond = _joinCond;
	teamName = _teamName;
	teamDec = _teamDec;
}

public final byte getMainOrder() { return (byte)12; }

public final byte getSubOrder() { return (byte)5; }

/** 活动实例ID */
public long getInstanceId() { return instanceId; }
/** 活动实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
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
	int _size = 28;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(teamDec);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
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
	_buf.putLong(instanceId);
	_buf.putInt(joinType.ordinal());

	_buf.putInt(joinCond.GetBufSize());
	joinCond.PutUnzipBuf(_buf);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, teamName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, teamDec);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)5);
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

