package Common.ArenaObj;

import java.nio.ByteBuffer;
/*********
 * 竞技场buff数据
 **/
public class Arena_BuffList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo> buffList;


public Arena_BuffList() {
	buffList = new java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo>();
}

public Arena_BuffList(
	 java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo> _buffList
) {	buffList = _buffList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.ArenaObj.Arena_SingleBuffInfo> getBuffList() { return buffList; }
public void addBuffList(Common.ArenaObj.Arena_SingleBuffInfo _buffList) { buffList.add(_buffList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (buffList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (buffList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _buffListCount = _buf.getShort();
	for(int _i = 0; _i < _buffListCount; _i++) { 
		Common.ArenaObj.Arena_SingleBuffInfo _buffList = new Common.ArenaObj.Arena_SingleBuffInfo();
		if(_buf.remaining() <= 0) return;
	int __buffListCustLen = _buf.getInt();
	int __buffListCurPos = _buf.position();
	_buffList.ReadUnzipBuf(_buf, __buffListCurPos + __buffListCustLen);
	_buf.position(__buffListCurPos + __buffListCustLen);

		buffList.add(_buffList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)buffList.size());
	for(int _i = 0; _i < buffList.size(); _i++) { 
		_buf.putInt(buffList.get(_i).GetBufSize());
	buffList.get(_i).PutUnzipBuf(_buf);
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

