package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险-队伍召回
 **/
public class GC2GS_041_012_ReqMarsTeamBack implements ALBasicProtocolPack._IALProtocolStructure {
private long teamId;


public GC2GS_041_012_ReqMarsTeamBack() {
	teamId = (long)0;
}

public GC2GS_041_012_ReqMarsTeamBack(
	 long _teamId
) {	teamId = _teamId;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)12; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)12);
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

