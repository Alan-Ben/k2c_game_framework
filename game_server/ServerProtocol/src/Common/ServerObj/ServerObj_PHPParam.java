package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 平台参数
 **/
public class ServerObj_PHPParam implements ALBasicProtocolPack._IALProtocolStructure {
private CommonEnum.EPlatParamType pKey;
private String pValue;


public ServerObj_PHPParam() {
	pKey = CommonEnum.EPlatParamType.values()[0];
	pValue = "";
}

public ServerObj_PHPParam(
	 CommonEnum.EPlatParamType _pKey
	, String _pValue
) {	pKey = _pKey;
	pValue = _pValue;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public CommonEnum.EPlatParamType getPKey() { return pKey; }
public void setPKey(CommonEnum.EPlatParamType _pKey) { pKey = _pKey; }
public String getPValue() { return pValue; }
public void setPValue(String _pValue) { pValue = _pValue; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pValue);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pValue);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pKey = CommonEnum.EPlatParamType.EPlatParamType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pValue = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pKey.ordinal());

	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, pValue);
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

