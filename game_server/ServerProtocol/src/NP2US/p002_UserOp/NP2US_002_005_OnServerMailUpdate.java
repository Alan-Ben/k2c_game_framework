package NP2US.p002_UserOp;

import java.nio.ByteBuffer;
/*********
 * 有新的全服邮件需要查收
 **/
public class NP2US_002_005_OnServerMailUpdate implements ALBasicProtocolPack._IALProtocolStructure {
/** 邮件唯一id */
private long phpMailId;


public NP2US_002_005_OnServerMailUpdate() {
	phpMailId = (long)0;
}

public NP2US_002_005_OnServerMailUpdate(
	 long _phpMailId
) {	phpMailId = _phpMailId;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)5; }

/** 邮件唯一id */
public long getPhpMailId() { return phpMailId; }
/** 邮件唯一id */
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
	_buf.put((byte)2);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

