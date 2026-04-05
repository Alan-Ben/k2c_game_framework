package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * NP 通用 long Map,用来往数据库存bytebuffer
 **/
public class NpServerObj_CommonLongMap implements ALBasicProtocolPack._IALProtocolStructure {
/** long map */
private java.util.ArrayList<Common.NpServerObj.NpServerObj_CommonLongPair> longMap;


public NpServerObj_CommonLongMap() {
	longMap = new java.util.ArrayList<Common.NpServerObj.NpServerObj_CommonLongPair>();
}

public NpServerObj_CommonLongMap(
	 java.util.ArrayList<Common.NpServerObj.NpServerObj_CommonLongPair> _longMap
) {	longMap = _longMap;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** long map */
public java.util.ArrayList<Common.NpServerObj.NpServerObj_CommonLongPair> getLongMap() { return longMap; }
/** long map */
public void addLongMap(Common.NpServerObj.NpServerObj_CommonLongPair _longMap) { longMap.add(_longMap); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (longMap.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (longMap.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _longMapCount = _buf.getShort();
	for(int _i = 0; _i < _longMapCount; _i++) { 
		Common.NpServerObj.NpServerObj_CommonLongPair _longMap = new Common.NpServerObj.NpServerObj_CommonLongPair();
		if(_buf.remaining() <= 0) return;
	int __longMapCustLen = _buf.getInt();
	int __longMapCurPos = _buf.position();
	_longMap.ReadUnzipBuf(_buf, __longMapCurPos + __longMapCustLen);
	_buf.position(__longMapCurPos + __longMapCustLen);

		longMap.add(_longMap);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)longMap.size());
	for(int _i = 0; _i < longMap.size(); _i++) { 
		_buf.putInt(longMap.get(_i).GetBufSize());
	longMap.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

