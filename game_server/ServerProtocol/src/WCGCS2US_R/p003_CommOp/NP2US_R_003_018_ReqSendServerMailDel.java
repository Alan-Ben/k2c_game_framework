package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 删除全服邮件
 **/
public class NP2US_R_003_018_ReqSendServerMailDel implements ALBasicProtocolPack._IALProtocolStructure {
/** 全服邮件平台ID */
private long phpMailId;


public NP2US_R_003_018_ReqSendServerMailDel() {
	phpMailId = (long)0;
}

public NP2US_R_003_018_ReqSendServerMailDel(
	 long _phpMailId
) {	phpMailId = _phpMailId;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)18; }

/** 全服邮件平台ID */
public long getPhpMailId() { return phpMailId; }
/** 全服邮件平台ID */
public void setPhpMailId(long _phpMailId) { phpMailId = _phpMailId; }


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
	if(_buf.remaining() > 0) phpMailId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(phpMailId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)18);
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

