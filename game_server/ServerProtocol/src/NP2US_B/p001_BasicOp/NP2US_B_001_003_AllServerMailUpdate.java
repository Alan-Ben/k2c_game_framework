package NP2US_B.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_B_001_003_AllServerMailUpdate implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否全部服务器邮件 */
private boolean isAllServer;
private long allServerMailId;
/** 服务器列表 */
private java.util.ArrayList<Integer> serverList;


public NP2US_B_001_003_AllServerMailUpdate() {
	isAllServer = false;
	allServerMailId = (long)0;
	serverList = new java.util.ArrayList<Integer>();
}

public NP2US_B_001_003_AllServerMailUpdate(
	 boolean _isAllServer
	, long _allServerMailId
	, java.util.ArrayList<Integer> _serverList
) {	isAllServer = _isAllServer;
	allServerMailId = _allServerMailId;
	serverList = _serverList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

/** 是否全部服务器邮件 */
public boolean getIsAllServer() { return isAllServer; }
/** 是否全部服务器邮件 */
public void setIsAllServer(boolean _isAllServer) { isAllServer = _isAllServer; }
public long getAllServerMailId() { return allServerMailId; }
public void setAllServerMailId(long _allServerMailId) { allServerMailId = _allServerMailId; }
/** 服务器列表 */
public java.util.ArrayList<Integer> getServerList() { return serverList; }
/** 服务器列表 */
public void addServerList(int _serverList) { serverList.add(_serverList); }


public final int GetBufSize() {
	int _size = 9;
	_size += 2 + (serverList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += 2 + (serverList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAllServer = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) allServerMailId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _serverListCount = _buf.getShort();
	for(int _i = 0; _i < _serverListCount; _i++) { 
		int _serverList = 0;
		if(_buf.remaining() > 0) _serverList = _buf.getInt();
		serverList.add(_serverList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isAllServer?(byte)1:(byte)0);
	_buf.putLong(allServerMailId);
	_buf.putShort((short)serverList.size());
	for(int _i = 0; _i < serverList.size(); _i++) { 
		_buf.putInt(serverList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

