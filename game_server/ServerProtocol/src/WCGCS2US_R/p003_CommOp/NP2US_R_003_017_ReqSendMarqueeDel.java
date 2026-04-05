package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 删除跑马灯
 **/
public class NP2US_R_003_017_ReqSendMarqueeDel implements ALBasicProtocolPack._IALProtocolStructure {
/** 跑马灯ID */
private long phpId;


public NP2US_R_003_017_ReqSendMarqueeDel() {
	phpId = (long)0;
}

public NP2US_R_003_017_ReqSendMarqueeDel(
	 long _phpId
) {	phpId = _phpId;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)17; }

/** 跑马灯ID */
public long getPhpId() { return phpId; }
/** 跑马灯ID */
public void setPhpId(long _phpId) { phpId = _phpId; }


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
	if(_buf.remaining() > 0) phpId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(phpId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)17);
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

