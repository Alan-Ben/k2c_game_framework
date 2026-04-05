package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSSetTeamApplyCond_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍实例ID */
private long teamId;
/** 玩家CID */
private long cid;
private Common.CrossTeamObj.CrossTeam_SetInfo_Join condValue;


public CTSSetTeamApplyCond_Req() {
	teamId = (long)0;
	cid = (long)0;
	condValue = new Common.CrossTeamObj.CrossTeam_SetInfo_Join();
}

public CTSSetTeamApplyCond_Req(
	 long _teamId
	, long _cid
	, Common.CrossTeamObj.CrossTeam_SetInfo_Join _condValue
) {	teamId = _teamId;
	cid = _cid;
	condValue = _condValue;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍实例ID */
public long getTeamId() { return teamId; }
/** 队伍实例ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
public Common.CrossTeamObj.CrossTeam_SetInfo_Join getCondValue() { return condValue; }
public void setCondValue(Common.CrossTeamObj.CrossTeam_SetInfo_Join _condValue) { condValue = _condValue; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _condValueCustLen = _buf.getInt();
	int _condValueCurPos = _buf.position();
	condValue.ReadUnzipBuf(_buf, _condValueCurPos + _condValueCustLen);
	_buf.position(_condValueCurPos + _condValueCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(cid);
	_buf.putInt(condValue.GetBufSize());
	condValue.PutUnzipBuf(_buf);
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

