package NP2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_001_003_USInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 服务器信息 */
private Common.NpServerObj.NpServerObj_SYS_ServerItem serverItem;


public NP2US_001_003_USInfoChg() {
	serverItem = new Common.NpServerObj.NpServerObj_SYS_ServerItem();
}

public NP2US_001_003_USInfoChg(
	 Common.NpServerObj.NpServerObj_SYS_ServerItem _serverItem
) {	serverItem = _serverItem;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

/** 服务器信息 */
public Common.NpServerObj.NpServerObj_SYS_ServerItem getServerItem() { return serverItem; }
/** 服务器信息 */
public void setServerItem(Common.NpServerObj.NpServerObj_SYS_ServerItem _serverItem) { serverItem = _serverItem; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + serverItem.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + serverItem.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _serverItemCustLen = _buf.getInt();
	int _serverItemCurPos = _buf.position();
	serverItem.ReadUnzipBuf(_buf, _serverItemCurPos + _serverItemCustLen);
	_buf.position(_serverItemCurPos + _serverItemCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverItem.GetBufSize());
	serverItem.PutUnzipBuf(_buf);
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

