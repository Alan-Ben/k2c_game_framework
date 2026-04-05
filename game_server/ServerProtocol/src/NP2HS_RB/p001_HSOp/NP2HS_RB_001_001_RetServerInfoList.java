package NP2HS_RB.p001_HSOp;

import java.nio.ByteBuffer;
public class NP2HS_RB_001_001_RetServerInfoList implements ALBasicProtocolPack._IALProtocolStructure {
/** 序列号 */
private long serial;
/** 后台服务器信息列表 */
private java.util.ArrayList<NPCommon.NP_SYS_ServerItem> serverInfoList;


public NP2HS_RB_001_001_RetServerInfoList() {
	serial = (long)0;
	serverInfoList = new java.util.ArrayList<NPCommon.NP_SYS_ServerItem>();
}

public NP2HS_RB_001_001_RetServerInfoList(
	 long _serial
	, java.util.ArrayList<NPCommon.NP_SYS_ServerItem> _serverInfoList
) {	serial = _serial;
	serverInfoList = _serverInfoList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

/** 序列号 */
public long getSerial() { return serial; }
/** 序列号 */
public void setSerial(long _serial) { serial = _serial; }
/** 后台服务器信息列表 */
public java.util.ArrayList<NPCommon.NP_SYS_ServerItem> getServerInfoList() { return serverInfoList; }
/** 后台服务器信息列表 */
public void addServerInfoList(NPCommon.NP_SYS_ServerItem _serverInfoList) { serverInfoList.add(_serverInfoList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < serverInfoList.size(); _i++) {
	_size += 4 + serverInfoList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < serverInfoList.size(); _i++) {
	_size += 4 + serverInfoList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _serverInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _serverInfoListCount; _i++) { 
		NPCommon.NP_SYS_ServerItem _serverInfoList = new NPCommon.NP_SYS_ServerItem();
		if(_buf.remaining() <= 0) return;
	int __serverInfoListCustLen = _buf.getInt();
	int __serverInfoListCurPos = _buf.position();
	_serverInfoList.ReadUnzipBuf(_buf, __serverInfoListCurPos + __serverInfoListCustLen);
	_buf.position(__serverInfoListCurPos + __serverInfoListCustLen);

		serverInfoList.add(_serverInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(serial);
	_buf.putShort((short)serverInfoList.size());
	for(int _i = 0; _i < serverInfoList.size(); _i++) { 
		_buf.putInt(serverInfoList.get(_i).GetBufSize());
	serverInfoList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

