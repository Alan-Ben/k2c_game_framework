package NP2HS_R.p001_HSOp;

import java.nio.ByteBuffer;
public class NP2HS_R_001_002_ReqAllServerMail implements ALBasicProtocolPack._IALProtocolStructure {
/** 全服邮件最大id，取大于此id的邮件 */
private long maxMailDbId;
/** us服务器id */
private int usTypeId;


public NP2HS_R_001_002_ReqAllServerMail() {
	maxMailDbId = (long)0;
	usTypeId = 0;
}

public NP2HS_R_001_002_ReqAllServerMail(
	 long _maxMailDbId
	, int _usTypeId
) {	maxMailDbId = _maxMailDbId;
	usTypeId = _usTypeId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

/** 全服邮件最大id，取大于此id的邮件 */
public long getMaxMailDbId() { return maxMailDbId; }
/** 全服邮件最大id，取大于此id的邮件 */
public void setMaxMailDbId(long _maxMailDbId) { maxMailDbId = _maxMailDbId; }
/** us服务器id */
public int getUsTypeId() { return usTypeId; }
/** us服务器id */
public void setUsTypeId(int _usTypeId) { usTypeId = _usTypeId; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxMailDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usTypeId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(maxMailDbId);
	_buf.putInt(usTypeId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

