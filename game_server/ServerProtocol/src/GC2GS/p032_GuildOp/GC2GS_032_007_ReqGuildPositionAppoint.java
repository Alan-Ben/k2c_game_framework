package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 请求联盟职位任命
 **/
public class GC2GS_032_007_ReqGuildPositionAppoint implements ALBasicProtocolPack._IALProtocolStructure {
private long memberId;
/** 职位id */
private long positionId;


public GC2GS_032_007_ReqGuildPositionAppoint() {
	memberId = (long)0;
	positionId = (long)0;
}

public GC2GS_032_007_ReqGuildPositionAppoint(
	 long _memberId
	, long _positionId
) {	memberId = _memberId;
	positionId = _positionId;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)7; }

public long getMemberId() { return memberId; }
public void setMemberId(long _memberId) { memberId = _memberId; }
/** 职位id */
public long getPositionId() { return positionId; }
/** 职位id */
public void setPositionId(long _positionId) { positionId = _positionId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) memberId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) positionId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(memberId);
	_buf.putLong(positionId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)7);
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

