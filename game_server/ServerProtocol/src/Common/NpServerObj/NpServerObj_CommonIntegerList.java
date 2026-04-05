package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * NP 通用 integer 数组,用来往数据库存bytebuffer
 **/
public class NpServerObj_CommonIntegerList implements ALBasicProtocolPack._IALProtocolStructure {
/** integer 数组 */
private java.util.ArrayList<Integer> integerList;


public NpServerObj_CommonIntegerList() {
	integerList = new java.util.ArrayList<Integer>();
}

public NpServerObj_CommonIntegerList(
	 java.util.ArrayList<Integer> _integerList
) {	integerList = _integerList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** integer 数组 */
public java.util.ArrayList<Integer> getIntegerList() { return integerList; }
/** integer 数组 */
public void addIntegerList(int _integerList) { integerList.add(_integerList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (integerList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (integerList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _integerListCount = _buf.getShort();
	for(int _i = 0; _i < _integerListCount; _i++) { 
		int _integerList = 0;
		if(_buf.remaining() > 0) _integerList = _buf.getInt();
		integerList.add(_integerList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)integerList.size());
	for(int _i = 0; _i < integerList.size(); _i++) { 
		_buf.putInt(integerList.get(_i));
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

