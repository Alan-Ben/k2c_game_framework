package NP2PS_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2PS_RB_001_012_RetOnlineRefServerList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.NpServerObj.NpServerObj_ServerInfo> refServerList;


public NP2PS_RB_001_012_RetOnlineRefServerList() {
	refServerList = new java.util.ArrayList<Common.NpServerObj.NpServerObj_ServerInfo>();
}

public NP2PS_RB_001_012_RetOnlineRefServerList(
	 java.util.ArrayList<Common.NpServerObj.NpServerObj_ServerInfo> _refServerList
) {	refServerList = _refServerList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)12; }

public java.util.ArrayList<Common.NpServerObj.NpServerObj_ServerInfo> getRefServerList() { return refServerList; }
public void addRefServerList(Common.NpServerObj.NpServerObj_ServerInfo _refServerList) { refServerList.add(_refServerList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (refServerList.size() * 12);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (refServerList.size() * 12);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _refServerListCount = _buf.getShort();
	for(int _i = 0; _i < _refServerListCount; _i++) { 
		Common.NpServerObj.NpServerObj_ServerInfo _refServerList = new Common.NpServerObj.NpServerObj_ServerInfo();
		if(_buf.remaining() <= 0) return;
	int __refServerListCustLen = _buf.getInt();
	int __refServerListCurPos = _buf.position();
	_refServerList.ReadUnzipBuf(_buf, __refServerListCurPos + __refServerListCustLen);
	_buf.position(__refServerListCurPos + __refServerListCustLen);

		refServerList.add(_refServerList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)refServerList.size());
	for(int _i = 0; _i < refServerList.size(); _i++) { 
		_buf.putInt(refServerList.get(_i).GetBufSize());
	refServerList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)12);
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

