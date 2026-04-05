package WCGCS2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2LCS_RB_001_001_RetUserIndex implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int usIndex;


public WCGCS2LCS_RB_001_001_RetUserIndex() {
	uid = (long)0;
	usIndex = 0;
}

public WCGCS2LCS_RB_001_001_RetUserIndex(
	 long _uid
	, int _usIndex
) {	uid = _uid;
	usIndex = _usIndex;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getUsIndex() { return usIndex; }
public void setUsIndex(int _usIndex) { usIndex = _usIndex; }


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
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usIndex = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(usIndex);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

