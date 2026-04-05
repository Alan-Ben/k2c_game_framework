package NP2CS_R.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_R_002_009_ReqTryCrossGameServer implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请时先加上的权重 */
private int tmpHandleWeight;
/** 自增类型 */
private NPEnum.ENPCommonGeneralEnum generalV;


public NP2CS_R_002_009_ReqTryCrossGameServer() {
	tmpHandleWeight = 0;
	generalV = NPEnum.ENPCommonGeneralEnum.values()[0];
}

public NP2CS_R_002_009_ReqTryCrossGameServer(
	 int _tmpHandleWeight
	, NPEnum.ENPCommonGeneralEnum _generalV
) {	tmpHandleWeight = _tmpHandleWeight;
	generalV = _generalV;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)9; }

/** 申请时先加上的权重 */
public int getTmpHandleWeight() { return tmpHandleWeight; }
/** 申请时先加上的权重 */
public void setTmpHandleWeight(int _tmpHandleWeight) { tmpHandleWeight = _tmpHandleWeight; }
/** 自增类型 */
public NPEnum.ENPCommonGeneralEnum getGeneralV() { return generalV; }
/** 自增类型 */
public void setGeneralV(NPEnum.ENPCommonGeneralEnum _generalV) { generalV = _generalV; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) tmpHandleWeight = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) generalV = NPEnum.ENPCommonGeneralEnum.ENPCommonGeneralEnum_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(tmpHandleWeight);
	_buf.putInt(generalV.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)9);
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

