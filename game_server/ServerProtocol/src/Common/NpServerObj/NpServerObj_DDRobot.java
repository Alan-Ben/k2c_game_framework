package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 钉钉预警机器人信息
 **/
public class NpServerObj_DDRobot implements ALBasicProtocolPack._IALProtocolStructure {
/** 报警类型 */
private NPEnum.ENPDDAlertType alertType;
/** 钉钉机器人access_token */
private String token;
/** 钉钉机器人签名 */
private String secret;


public NpServerObj_DDRobot() {
	alertType = NPEnum.ENPDDAlertType.values()[0];
	token = "";
	secret = "";
}

public NpServerObj_DDRobot(
	 NPEnum.ENPDDAlertType _alertType
	, String _token
	, String _secret
) {	alertType = _alertType;
	token = _token;
	secret = _secret;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 报警类型 */
public NPEnum.ENPDDAlertType getAlertType() { return alertType; }
/** 报警类型 */
public void setAlertType(NPEnum.ENPDDAlertType _alertType) { alertType = _alertType; }
/** 钉钉机器人access_token */
public String getToken() { return token; }
/** 钉钉机器人access_token */
public void setToken(String _token) { token = _token; }
/** 钉钉机器人签名 */
public String getSecret() { return secret; }
/** 钉钉机器人签名 */
public void setSecret(String _secret) { secret = _secret; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(token);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(secret);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(token);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(secret);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) alertType = NPEnum.ENPDDAlertType.ENPDDAlertType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) token = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) secret = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(alertType.ordinal());

	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, token);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, secret);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

