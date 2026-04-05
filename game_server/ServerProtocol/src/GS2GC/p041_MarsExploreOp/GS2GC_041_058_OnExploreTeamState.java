package GS2GC.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探索队伍-队伍状态变更
 **/
public class GS2GC_041_058_OnExploreTeamState implements ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
private Common.MarsObj.Mars_TeamState state;


public GS2GC_041_058_OnExploreTeamState() {
	teamId = (long)0;
	state = new Common.MarsObj.Mars_TeamState();
}

public GS2GC_041_058_OnExploreTeamState(
	 long _teamId
	, Common.MarsObj.Mars_TeamState _state
) {	teamId = _teamId;
	state = _state;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)58; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
public Common.MarsObj.Mars_TeamState getState() { return state; }
public void setState(Common.MarsObj.Mars_TeamState _state) { state = _state; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + state.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + state.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _stateCustLen = _buf.getInt();
	int _stateCurPos = _buf.position();
	state.ReadUnzipBuf(_buf, _stateCurPos + _stateCustLen);
	_buf.position(_stateCurPos + _stateCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putInt(state.GetBufSize());
	state.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)58);
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

