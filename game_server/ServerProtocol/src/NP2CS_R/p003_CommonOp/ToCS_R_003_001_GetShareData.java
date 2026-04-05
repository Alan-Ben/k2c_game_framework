package NP2CS_R.p003_CommonOp;

import java.nio.ByteBuffer;
public class ToCS_R_003_001_GetShareData implements ALBasicProtocolPack._IALProtocolStructure {
private int type;
private long serial;


public ToCS_R_003_001_GetShareData() {
	type = 0;
	serial = (long)0;
}

public ToCS_R_003_001_GetShareData(
	 int _type
	, long _serial
) {	type = _type;
	serial = _serial;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)1; }

public int getType() { return type; }
public void setType(int _type) { type = _type; }
public long getSerial() { return serial; }
public void setSerial(long _serial) { serial = _serial; }


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
	if(_buf.remaining() > 0) type = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serial = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type);
	_buf.putLong(serial);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)1);
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

