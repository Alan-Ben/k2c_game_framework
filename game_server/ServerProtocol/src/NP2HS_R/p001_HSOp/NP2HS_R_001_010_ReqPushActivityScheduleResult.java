package NP2HS_R.p001_HSOp;

import java.nio.ByteBuffer;
/*********
 * 通知HS活动排期推送结果
 **/
public class NP2HS_R_001_010_ReqPushActivityScheduleResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 后台操作序列号 */
private long phpOpSerial;
/** 错误码 0-无操作 */
private int errCode;
/** 错误信息 */
private String errMsg;


public NP2HS_R_001_010_ReqPushActivityScheduleResult() {
	phpOpSerial = (long)0;
	errCode = 0;
	errMsg = "";
}

public NP2HS_R_001_010_ReqPushActivityScheduleResult(
	 long _phpOpSerial
	, int _errCode
	, String _errMsg
) {	phpOpSerial = _phpOpSerial;
	errCode = _errCode;
	errMsg = _errMsg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)10; }

/** 后台操作序列号 */
public long getPhpOpSerial() { return phpOpSerial; }
/** 后台操作序列号 */
public void setPhpOpSerial(long _phpOpSerial) { phpOpSerial = _phpOpSerial; }
/** 错误码 0-无操作 */
public int getErrCode() { return errCode; }
/** 错误码 0-无操作 */
public void setErrCode(int _errCode) { errCode = _errCode; }
/** 错误信息 */
public String getErrMsg() { return errMsg; }
/** 错误信息 */
public void setErrMsg(String _errMsg) { errMsg = _errMsg; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(errMsg);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(errMsg);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) phpOpSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errMsg = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(phpOpSerial);
	_buf.putInt(errCode);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, errMsg);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)10);
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

