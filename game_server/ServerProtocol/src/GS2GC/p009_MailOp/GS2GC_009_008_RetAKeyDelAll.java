package GS2GC.p009_MailOp;

import java.nio.ByteBuffer;
public class GS2GC_009_008_RetAKeyDelAll implements ALBasicProtocolPack._IALProtocolStructure {
/** 已删除邮件列表 */
private java.util.ArrayList<Long> delIdList;


public GS2GC_009_008_RetAKeyDelAll() {
	delIdList = new java.util.ArrayList<Long>();
}

public GS2GC_009_008_RetAKeyDelAll(
	 java.util.ArrayList<Long> _delIdList
) {	delIdList = _delIdList;
}

public final byte getMainOrder() { return (byte)9; }

public final byte getSubOrder() { return (byte)8; }

/** 已删除邮件列表 */
public java.util.ArrayList<Long> getDelIdList() { return delIdList; }
/** 已删除邮件列表 */
public void addDelIdList(long _delIdList) { delIdList.add(_delIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (delIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (delIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _delIdListCount = _buf.getShort();
	for(int _i = 0; _i < _delIdListCount; _i++) { 
		long _delIdList = (long)0;
		if(_buf.remaining() > 0) _delIdList = _buf.getLong();
		delIdList.add(_delIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)delIdList.size());
	for(int _i = 0; _i < delIdList.size(); _i++) { 
		_buf.putLong(delIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)9);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)9);
	_recBuf.put((byte)8);
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

