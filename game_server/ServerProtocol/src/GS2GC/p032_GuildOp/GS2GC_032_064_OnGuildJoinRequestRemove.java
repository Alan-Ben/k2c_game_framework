package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_064_OnGuildJoinRequestRemove implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> requestDbIdList;


public GS2GC_032_064_OnGuildJoinRequestRemove() {
	requestDbIdList = new java.util.ArrayList<Long>();
}

public GS2GC_032_064_OnGuildJoinRequestRemove(
	 java.util.ArrayList<Long> _requestDbIdList
) {	requestDbIdList = _requestDbIdList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)64; }

public java.util.ArrayList<Long> getRequestDbIdList() { return requestDbIdList; }
public void addRequestDbIdList(long _requestDbIdList) { requestDbIdList.add(_requestDbIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (requestDbIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (requestDbIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _requestDbIdListCount = _buf.getShort();
	for(int _i = 0; _i < _requestDbIdListCount; _i++) { 
		long _requestDbIdList = (long)0;
		if(_buf.remaining() > 0) _requestDbIdList = _buf.getLong();
		requestDbIdList.add(_requestDbIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)requestDbIdList.size());
	for(int _i = 0; _i < requestDbIdList.size(); _i++) { 
		_buf.putLong(requestDbIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)64);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)64);
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

