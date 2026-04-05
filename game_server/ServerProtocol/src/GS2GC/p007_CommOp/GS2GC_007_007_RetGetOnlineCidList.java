package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_007_RetGetOnlineCidList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> cidList;


public GS2GC_007_007_RetGetOnlineCidList() {
	cidList = new java.util.ArrayList<Long>();
}

public GS2GC_007_007_RetGetOnlineCidList(
	 java.util.ArrayList<Long> _cidList
) {	cidList = _cidList;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)7; }

public java.util.ArrayList<Long> getCidList() { return cidList; }
public void addCidList(long _cidList) { cidList.add(_cidList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (cidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		if(_buf.remaining() > 0) _cidList = _buf.getLong();
		cidList.add(_cidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
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

