package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 随机邀约中指定的妃子ID列表变更
 **/
public class GS2GC_015_062_OnRandCallConsortChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private java.util.ArrayList<Long> consortIdList;


public GS2GC_015_062_OnRandCallConsortChg() {
	consortIdList = new java.util.ArrayList<Long>();
}

public GS2GC_015_062_OnRandCallConsortChg(
	 java.util.ArrayList<Long> _consortIdList
) {	consortIdList = _consortIdList;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)62; }

/** 空 */
public java.util.ArrayList<Long> getConsortIdList() { return consortIdList; }
/** 空 */
public void addConsortIdList(long _consortIdList) { consortIdList.add(_consortIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (consortIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (consortIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _consortIdListCount = _buf.getShort();
	for(int _i = 0; _i < _consortIdListCount; _i++) { 
		long _consortIdList = (long)0;
		if(_buf.remaining() > 0) _consortIdList = _buf.getLong();
		consortIdList.add(_consortIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)consortIdList.size());
	for(int _i = 0; _i < consortIdList.size(); _i++) { 
		_buf.putLong(consortIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)62);
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

