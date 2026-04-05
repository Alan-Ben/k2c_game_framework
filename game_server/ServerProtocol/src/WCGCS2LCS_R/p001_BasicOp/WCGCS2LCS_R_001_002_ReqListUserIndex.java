package WCGCS2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2LCS_R_001_002_ReqListUserIndex implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> uids;


public WCGCS2LCS_R_001_002_ReqListUserIndex() {
	uids = new java.util.ArrayList<Long>();
}

public WCGCS2LCS_R_001_002_ReqListUserIndex(
	 java.util.ArrayList<Long> _uids
) {	uids = _uids;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public java.util.ArrayList<Long> getUids() { return uids; }
public void addUids(long _uids) { uids.add(_uids); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (uids.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (uids.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _uidsCount = _buf.getShort();
	for(int _i = 0; _i < _uidsCount; _i++) { 
		long _uids = (long)0;
		if(_buf.remaining() > 0) _uids = _buf.getLong();
		uids.add(_uids);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)uids.size());
	for(int _i = 0; _i < uids.size(); _i++) { 
		_buf.putLong(uids.get(_i));
	}
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

