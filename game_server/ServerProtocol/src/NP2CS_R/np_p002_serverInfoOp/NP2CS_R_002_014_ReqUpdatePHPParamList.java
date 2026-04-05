package NP2CS_R.np_p002_serverInfoOp;

import java.nio.ByteBuffer;
public class NP2CS_R_002_014_ReqUpdatePHPParamList implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据序列号 */
private long dataSerial;
/** 平台序列号 */
private String phpSerial;
/** 平台参数列表 */
private Common.ServerObj.ServerObj_PHPParamList pListObj;


public NP2CS_R_002_014_ReqUpdatePHPParamList() {
	dataSerial = (long)0;
	phpSerial = "";
	pListObj = new Common.ServerObj.ServerObj_PHPParamList();
}

public NP2CS_R_002_014_ReqUpdatePHPParamList(
	 long _dataSerial
	, String _phpSerial
	, Common.ServerObj.ServerObj_PHPParamList _pListObj
) {	dataSerial = _dataSerial;
	phpSerial = _phpSerial;
	pListObj = _pListObj;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)14; }

/** 数据序列号 */
public long getDataSerial() { return dataSerial; }
/** 数据序列号 */
public void setDataSerial(long _dataSerial) { dataSerial = _dataSerial; }
/** 平台序列号 */
public String getPhpSerial() { return phpSerial; }
/** 平台序列号 */
public void setPhpSerial(String _phpSerial) { phpSerial = _phpSerial; }
/** 平台参数列表 */
public Common.ServerObj.ServerObj_PHPParamList getPListObj() { return pListObj; }
/** 平台参数列表 */
public void setPListObj(Common.ServerObj.ServerObj_PHPParamList _pListObj) { pListObj = _pListObj; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(phpSerial);
	_size += 4 + pListObj.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(phpSerial);
	_size += 4 + pListObj.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dataSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) phpSerial = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _pListObjCustLen = _buf.getInt();
	int _pListObjCurPos = _buf.position();
	pListObj.ReadUnzipBuf(_buf, _pListObjCurPos + _pListObjCustLen);
	_buf.position(_pListObjCurPos + _pListObjCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dataSerial);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, phpSerial);
	_buf.putInt(pListObj.GetBufSize());
	pListObj.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)14);
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

