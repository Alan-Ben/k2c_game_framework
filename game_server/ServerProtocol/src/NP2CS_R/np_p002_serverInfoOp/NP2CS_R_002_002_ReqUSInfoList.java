package NP2CS_R.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_R_002_002_ReqUSInfoList implements ALBasicProtocolPack._IALProtocolStructure {
/** 需要查询的服务逻辑id列表 */
private java.util.ArrayList<Integer> usLogicIdList;


public NP2CS_R_002_002_ReqUSInfoList() {
	usLogicIdList = new java.util.ArrayList<Integer>();
}

public NP2CS_R_002_002_ReqUSInfoList(
	 java.util.ArrayList<Integer> _usLogicIdList
) {	usLogicIdList = _usLogicIdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)2; }

/** 需要查询的服务逻辑id列表 */
public java.util.ArrayList<Integer> getUsLogicIdList() { return usLogicIdList; }
/** 需要查询的服务逻辑id列表 */
public void addUsLogicIdList(int _usLogicIdList) { usLogicIdList.add(_usLogicIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (usLogicIdList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (usLogicIdList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _usLogicIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usLogicIdListCount; _i++) { 
		int _usLogicIdList = 0;
		if(_buf.remaining() > 0) _usLogicIdList = _buf.getInt();
		usLogicIdList.add(_usLogicIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)usLogicIdList.size());
	for(int _i = 0; _i < usLogicIdList.size(); _i++) { 
		_buf.putInt(usLogicIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

