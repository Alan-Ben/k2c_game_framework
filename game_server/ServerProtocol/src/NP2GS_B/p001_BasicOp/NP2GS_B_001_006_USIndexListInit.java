package NP2GS_B.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2GS_B_001_006_USIndexListInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 服务器信息列表 */
private java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo> serverIndexList;


public NP2GS_B_001_006_USIndexListInit() {
	serverIndexList = new java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo>();
}

public NP2GS_B_001_006_USIndexListInit(
	 java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo> _serverIndexList
) {	serverIndexList = _serverIndexList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)6; }

/** 服务器信息列表 */
public java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo> getServerIndexList() { return serverIndexList; }
/** 服务器信息列表 */
public void addServerIndexList(Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo _serverIndexList) { serverIndexList.add(_serverIndexList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (serverIndexList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (serverIndexList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _serverIndexListCount = _buf.getShort();
	for(int _i = 0; _i < _serverIndexListCount; _i++) { 
		Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo _serverIndexList = new Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo();
		if(_buf.remaining() <= 0) return;
	int __serverIndexListCustLen = _buf.getInt();
	int __serverIndexListCurPos = _buf.position();
	_serverIndexList.ReadUnzipBuf(_buf, __serverIndexListCurPos + __serverIndexListCustLen);
	_buf.position(__serverIndexListCurPos + __serverIndexListCustLen);

		serverIndexList.add(_serverIndexList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)serverIndexList.size());
	for(int _i = 0; _i < serverIndexList.size(); _i++) { 
		_buf.putInt(serverIndexList.get(_i).GetBufSize());
	serverIndexList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)6);
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

