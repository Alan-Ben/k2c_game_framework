package NP2HS_R.p002_HsClientOp;

import java.nio.ByteBuffer;
public class NP2HS_R_002_001_ReqExecGmSuper implements ALBasicProtocolPack._IALProtocolStructure {
private boolean isAll;
private int serverType;
private java.util.ArrayList<Integer> serverIdList;
private String gmComamnd;
/** 全部服务端资源的服务器列表 */
private boolean isAllRef;


public NP2HS_R_002_001_ReqExecGmSuper() {
	isAll = false;
	serverType = 0;
	serverIdList = new java.util.ArrayList<Integer>();
	gmComamnd = "";
	isAllRef = false;
}

public NP2HS_R_002_001_ReqExecGmSuper(
	 boolean _isAll
	, int _serverType
	, java.util.ArrayList<Integer> _serverIdList
	, String _gmComamnd
	, boolean _isAllRef
) {	isAll = _isAll;
	serverType = _serverType;
	serverIdList = _serverIdList;
	gmComamnd = _gmComamnd;
	isAllRef = _isAllRef;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public boolean getIsAll() { return isAll; }
public void setIsAll(boolean _isAll) { isAll = _isAll; }
public int getServerType() { return serverType; }
public void setServerType(int _serverType) { serverType = _serverType; }
public java.util.ArrayList<Integer> getServerIdList() { return serverIdList; }
public void addServerIdList(int _serverIdList) { serverIdList.add(_serverIdList); }
public String getGmComamnd() { return gmComamnd; }
public void setGmComamnd(String _gmComamnd) { gmComamnd = _gmComamnd; }
/** 全部服务端资源的服务器列表 */
public boolean getIsAllRef() { return isAllRef; }
/** 全部服务端资源的服务器列表 */
public void setIsAllRef(boolean _isAllRef) { isAllRef = _isAllRef; }


public final int GetBufSize() {
	int _size = 6;
	_size += 2 + (serverIdList.size() * 4);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gmComamnd);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 8;
	_size += 2 + (serverIdList.size() * 4);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gmComamnd);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAll = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _serverIdListCount = _buf.getShort();
	for(int _i = 0; _i < _serverIdListCount; _i++) { 
		int _serverIdList = 0;
		if(_buf.remaining() > 0) _serverIdList = _buf.getInt();
		serverIdList.add(_serverIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gmComamnd = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAllRef = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isAll?(byte)1:(byte)0);
	_buf.putInt(serverType);
	_buf.putShort((short)serverIdList.size());
	for(int _i = 0; _i < serverIdList.size(); _i++) { 
		_buf.putInt(serverIdList.get(_i));
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, gmComamnd);
	_buf.put(isAllRef?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)1);
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

