package NP2CS_R.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_R_002_011_ReqUpdateUSInfoList implements ALBasicProtocolPack._IALProtocolStructure {
/** 服务器信息列表 */
private java.util.ArrayList<NPCommon.NP_SYS_ServerItem> serverItemList;


public NP2CS_R_002_011_ReqUpdateUSInfoList() {
	serverItemList = new java.util.ArrayList<NPCommon.NP_SYS_ServerItem>();
}

public NP2CS_R_002_011_ReqUpdateUSInfoList(
	 java.util.ArrayList<NPCommon.NP_SYS_ServerItem> _serverItemList
) {	serverItemList = _serverItemList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)11; }

/** 服务器信息列表 */
public java.util.ArrayList<NPCommon.NP_SYS_ServerItem> getServerItemList() { return serverItemList; }
/** 服务器信息列表 */
public void addServerItemList(NPCommon.NP_SYS_ServerItem _serverItemList) { serverItemList.add(_serverItemList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < serverItemList.size(); _i++) {
	_size += 4 + serverItemList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < serverItemList.size(); _i++) {
	_size += 4 + serverItemList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _serverItemListCount = _buf.getShort();
	for(int _i = 0; _i < _serverItemListCount; _i++) { 
		NPCommon.NP_SYS_ServerItem _serverItemList = new NPCommon.NP_SYS_ServerItem();
		if(_buf.remaining() <= 0) return;
	int __serverItemListCustLen = _buf.getInt();
	int __serverItemListCurPos = _buf.position();
	_serverItemList.ReadUnzipBuf(_buf, __serverItemListCurPos + __serverItemListCustLen);
	_buf.position(__serverItemListCurPos + __serverItemListCustLen);

		serverItemList.add(_serverItemList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)serverItemList.size());
	for(int _i = 0; _i < serverItemList.size(); _i++) { 
		_buf.putInt(serverItemList.get(_i).GetBufSize());
	serverItemList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)11);
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

