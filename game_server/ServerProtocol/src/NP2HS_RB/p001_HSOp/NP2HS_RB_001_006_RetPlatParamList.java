package NP2HS_RB.p001_HSOp;

import java.nio.ByteBuffer;
public class NP2HS_RB_001_006_RetPlatParamList implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据序列号 */
private long dataSerial;
/** 平台序列号 */
private String phpSerial;
private Common.ServerObj.ServerObj_PHPParamList pListObj;


public NP2HS_RB_001_006_RetPlatParamList() {
	dataSerial = (long)0;
	phpSerial = "";
	pListObj = new Common.ServerObj.ServerObj_PHPParamList();
}

public NP2HS_RB_001_006_RetPlatParamList(
	 long _dataSerial
	, String _phpSerial
	, Common.ServerObj.ServerObj_PHPParamList _pListObj
) {	dataSerial = _dataSerial;
	phpSerial = _phpSerial;
	pListObj = _pListObj;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)6; }

/** 数据序列号 */
public long getDataSerial() { return dataSerial; }
/** 数据序列号 */
public void setDataSerial(long _dataSerial) { dataSerial = _dataSerial; }
/** 平台序列号 */
public String getPhpSerial() { return phpSerial; }
/** 平台序列号 */
public void setPhpSerial(String _phpSerial) { phpSerial = _phpSerial; }
public Common.ServerObj.ServerObj_PHPParamList getPListObj() { return pListObj; }
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
	_buf.put((byte)1);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

