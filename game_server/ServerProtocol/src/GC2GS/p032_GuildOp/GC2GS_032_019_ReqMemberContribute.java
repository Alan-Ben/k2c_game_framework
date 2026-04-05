package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 请求联盟成员贡献列表
 **/
public class GC2GS_032_019_ReqMemberContribute implements ALBasicProtocolPack._IALProtocolStructure {
private long memberId;


public GC2GS_032_019_ReqMemberContribute() {
	memberId = (long)0;
}

public GC2GS_032_019_ReqMemberContribute(
	 long _memberId
) {	memberId = _memberId;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)19; }

public long getMemberId() { return memberId; }
public void setMemberId(long _memberId) { memberId = _memberId; }


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
	if(_buf.remaining() > 0) memberId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(memberId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)19);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)19);
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

