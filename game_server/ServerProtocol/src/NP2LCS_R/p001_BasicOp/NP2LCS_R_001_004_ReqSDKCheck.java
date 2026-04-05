package NP2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2LCS_R_001_004_ReqSDKCheck implements ALBasicProtocolPack._IALProtocolStructure {
/** 外部用户id */
private String accName;
/** 校验串 */
private String token;
/** 客户端ip地址 */
private String clientIp;


public NP2LCS_R_001_004_ReqSDKCheck() {
	accName = "";
	token = "";
	clientIp = "";
}

public NP2LCS_R_001_004_ReqSDKCheck(
	 String _accName
	, String _token
	, String _clientIp
) {	accName = _accName;
	token = _token;
	clientIp = _clientIp;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)4; }

/** 外部用户id */
public String getAccName() { return accName; }
/** 外部用户id */
public void setAccName(String _accName) { accName = _accName; }
/** 校验串 */
public String getToken() { return token; }
/** 校验串 */
public void setToken(String _token) { token = _token; }
/** 客户端ip地址 */
public String getClientIp() { return clientIp; }
/** 客户端ip地址 */
public void setClientIp(String _clientIp) { clientIp = _clientIp; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(token);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientIp);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(token);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(clientIp);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) accName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) token = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientIp = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, accName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, token);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, clientIp);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)4);
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

