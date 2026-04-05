package GS2GC.p012_ActivityTeamOp;

import java.nio.ByteBuffer;
/*********
 * 加入活动队伍
 **/
public class GS2GC_012_051_OnJoinActivityTeam implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 活动队伍数据 */
private Common.CrossTeamObj.CrossTeam_Info team;


public GS2GC_012_051_OnJoinActivityTeam() {
	teamId = (long)0;
	team = new Common.CrossTeamObj.CrossTeam_Info();
}

public GS2GC_012_051_OnJoinActivityTeam(
	 long _teamId
	, Common.CrossTeamObj.CrossTeam_Info _team
) {	teamId = _teamId;
	team = _team;
}

public final byte getMainOrder() { return (byte)12; }

public final byte getSubOrder() { return (byte)51; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 活动队伍数据 */
public Common.CrossTeamObj.CrossTeam_Info getTeam() { return team; }
/** 活动队伍数据 */
public void setTeam(Common.CrossTeamObj.CrossTeam_Info _team) { team = _team; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + team.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + team.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _teamCustLen = _buf.getInt();
	int _teamCurPos = _buf.position();
	team.ReadUnzipBuf(_buf, _teamCurPos + _teamCustLen);
	_buf.position(_teamCurPos + _teamCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putInt(team.GetBufSize());
	team.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)51);
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

