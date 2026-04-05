package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 请求分享码信息
 **/
public class GC2GS_007_006_ReqShareCodeData implements ALBasicProtocolPack._IALProtocolStructure {
/** 类型 */
private CommonEnum.EShareCodeType type;
/** 分享码 */
private String shareCode;


public GC2GS_007_006_ReqShareCodeData() {
	type = CommonEnum.EShareCodeType.values()[0];
	shareCode = "";
}

public GC2GS_007_006_ReqShareCodeData(
	 CommonEnum.EShareCodeType _type
	, String _shareCode
) {	type = _type;
	shareCode = _shareCode;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)6; }

/** 类型 */
public CommonEnum.EShareCodeType getType() { return type; }
/** 类型 */
public void setType(CommonEnum.EShareCodeType _type) { type = _type; }
/** 分享码 */
public String getShareCode() { return shareCode; }
/** 分享码 */
public void setShareCode(String _shareCode) { shareCode = _shareCode; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(shareCode);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(shareCode);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = CommonEnum.EShareCodeType.EShareCodeType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) shareCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, shareCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
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

