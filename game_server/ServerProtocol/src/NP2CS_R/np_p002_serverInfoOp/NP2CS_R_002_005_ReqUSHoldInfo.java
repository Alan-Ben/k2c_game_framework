package NP2CS_R.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_R_002_005_ReqUSHoldInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 需要查询的服务器信息列表 */
private java.util.ArrayList<Integer> serverTypeIdList;


public NP2CS_R_002_005_ReqUSHoldInfo() {
	serverTypeIdList = new java.util.ArrayList<Integer>();
}

public NP2CS_R_002_005_ReqUSHoldInfo(
	 java.util.ArrayList<Integer> _serverTypeIdList
) {	serverTypeIdList = _serverTypeIdList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)5; }

/** 需要查询的服务器信息列表 */
public java.util.ArrayList<Integer> getServerTypeIdList() { return serverTypeIdList; }
/** 需要查询的服务器信息列表 */
public void addServerTypeIdList(int _serverTypeIdList) { serverTypeIdList.add(_serverTypeIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (serverTypeIdList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (serverTypeIdList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _serverTypeIdListCount = _buf.getShort();
	for(int _i = 0; _i < _serverTypeIdListCount; _i++) { 
		int _serverTypeIdList = 0;
		if(_buf.remaining() > 0) _serverTypeIdList = _buf.getInt();
		serverTypeIdList.add(_serverTypeIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)serverTypeIdList.size());
	for(int _i = 0; _i < serverTypeIdList.size(); _i++) { 
		_buf.putInt(serverTypeIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)5);
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

