package GC2GS.p012_ActivityTeamOp;

import java.nio.ByteBuffer;
/*********
 * 设置队伍申请条件
 **/
public class GC2GS_012_004_ReqSetActivityTeamApplyCond implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 申请条件 */
private Common.CrossTeamObj.CrossTeam_SetInfo_Join joinCond;


public GC2GS_012_004_ReqSetActivityTeamApplyCond() {
	teamId = (long)0;
	joinCond = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
}

public GC2GS_012_004_ReqSetActivityTeamApplyCond(
	 long _teamId
	, Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond
) {	teamId = _teamId;
	joinCond = _joinCond;
}

public final byte getMainOrder() { return (byte)12; }

public final byte getSubOrder() { return (byte)4; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 申请条件 */
public Common.CrossTeamObj.CrossTeam_SetInfo_Join getJoinCond() { return joinCond; }
/** 申请条件 */
public void setJoinCond(Common.CrossTeamObj.CrossTeam_SetInfo_Join _joinCond) { joinCond = _joinCond; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _joinCondCustLen = _buf.getInt();
	int _joinCondCurPos = _buf.position();
	joinCond.ReadUnzipBuf(_buf, _joinCondCurPos + _joinCondCustLen);
	_buf.position(_joinCondCurPos + _joinCondCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putInt(joinCond.GetBufSize());
	joinCond.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)4);
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

