package NP2CS_RB.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_RB_002_005_RetUSHoldInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** US负载信息 */
private java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo> holdInfoList;


public NP2CS_RB_002_005_RetUSHoldInfo() {
	holdInfoList = new java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo>();
}

public NP2CS_RB_002_005_RetUSHoldInfo(
	 java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo> _holdInfoList
) {	holdInfoList = _holdInfoList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)5; }

/** US负载信息 */
public java.util.ArrayList<Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo> getHoldInfoList() { return holdInfoList; }
/** US负载信息 */
public void addHoldInfoList(Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo _holdInfoList) { holdInfoList.add(_holdInfoList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (holdInfoList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (holdInfoList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _holdInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _holdInfoListCount; _i++) { 
		Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo _holdInfoList = new Common.NpServerObj.NpServerObj_SYS_ServerHoldInfo();
		if(_buf.remaining() <= 0) return;
	int __holdInfoListCustLen = _buf.getInt();
	int __holdInfoListCurPos = _buf.position();
	_holdInfoList.ReadUnzipBuf(_buf, __holdInfoListCurPos + __holdInfoListCustLen);
	_buf.position(__holdInfoListCurPos + __holdInfoListCustLen);

		holdInfoList.add(_holdInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)holdInfoList.size());
	for(int _i = 0; _i < holdInfoList.size(); _i++) { 
		_buf.putInt(holdInfoList.get(_i).GetBufSize());
	holdInfoList.get(_i).PutUnzipBuf(_buf);
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

