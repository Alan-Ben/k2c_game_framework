package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * NP 通用 long 数组,用来往数据库存bytebuffer
 **/
public class NpServerObj_CommonLongList implements ALBasicProtocolPack._IALProtocolStructure {
/** long 数组 */
private java.util.ArrayList<Long> longList;


public NpServerObj_CommonLongList() {
	longList = new java.util.ArrayList<Long>();
}

public NpServerObj_CommonLongList(
	 java.util.ArrayList<Long> _longList
) {	longList = _longList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** long 数组 */
public java.util.ArrayList<Long> getLongList() { return longList; }
/** long 数组 */
public void addLongList(long _longList) { longList.add(_longList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (longList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (longList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _longListCount = _buf.getShort();
	for(int _i = 0; _i < _longListCount; _i++) { 
		long _longList = (long)0;
		if(_buf.remaining() > 0) _longList = _buf.getLong();
		longList.add(_longList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)longList.size());
	for(int _i = 0; _i < longList.size(); _i++) { 
		_buf.putLong(longList.get(_i));
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

