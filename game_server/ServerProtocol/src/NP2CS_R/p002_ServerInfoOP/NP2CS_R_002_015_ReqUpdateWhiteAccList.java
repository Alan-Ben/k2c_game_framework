package NP2CS_R.p002_ServerInfoOP;

import java.nio.ByteBuffer;
/*********
 * 请求更新白名单列表
 **/
public class NP2CS_R_002_015_ReqUpdateWhiteAccList implements ALBasicProtocolPack._IALProtocolStructure {
/** 操作类型：ADD添加/REMOVE删除 */
private NPEnum.EWhiteAccOpType opType;
/** 账号 */
private String acc;


public NP2CS_R_002_015_ReqUpdateWhiteAccList() {
	opType = NPEnum.EWhiteAccOpType.values()[0];
	acc = "";
}

public NP2CS_R_002_015_ReqUpdateWhiteAccList(
	 NPEnum.EWhiteAccOpType _opType
	, String _acc
) {	opType = _opType;
	acc = _acc;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)15; }

/** 操作类型：ADD添加/REMOVE删除 */
public NPEnum.EWhiteAccOpType getOpType() { return opType; }
/** 操作类型：ADD添加/REMOVE删除 */
public void setOpType(NPEnum.EWhiteAccOpType _opType) { opType = _opType; }
/** 账号 */
public String getAcc() { return acc; }
/** 账号 */
public void setAcc(String _acc) { acc = _acc; }


public final int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(acc);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(acc);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) opType = NPEnum.EWhiteAccOpType.EWhiteAccOpType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) acc = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(opType.ordinal());

	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, acc);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)15);
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

