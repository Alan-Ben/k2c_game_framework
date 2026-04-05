package NP2GS_B.p001_BasicOp;

import java.nio.ByteBuffer;
/*********
 * US服务器信息变更广播协议
 **/
public class NP2GS_B_001_005_USVersionChg implements ALBasicProtocolPack._IALProtocolStructure {
private int usId;
/** 资源版本 */
private String resVersion;
/** 服务器版本 */
private String serverVersion;


public NP2GS_B_001_005_USVersionChg() {
	usId = 0;
	resVersion = "";
	serverVersion = "";
}

public NP2GS_B_001_005_USVersionChg(
	 int _usId
	, String _resVersion
	, String _serverVersion
) {	usId = _usId;
	resVersion = _resVersion;
	serverVersion = _serverVersion;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)5; }

public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
/** 资源版本 */
public String getResVersion() { return resVersion; }
/** 资源版本 */
public void setResVersion(String _resVersion) { resVersion = _resVersion; }
/** 服务器版本 */
public String getServerVersion() { return serverVersion; }
/** 服务器版本 */
public void setServerVersion(String _serverVersion) { serverVersion = _serverVersion; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resVersion);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(serverVersion);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resVersion);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(serverVersion);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resVersion = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverVersion = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(usId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resVersion);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, serverVersion);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

