package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_062_OnTargetRewardUpdate implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标奖励信息 */
private Common.CommonFuncObj.CommonFunc_TargetReward info;


public GS2GC_007_062_OnTargetRewardUpdate() {
	info = new Common.CommonFuncObj.CommonFunc_TargetReward();
}

public GS2GC_007_062_OnTargetRewardUpdate(
	 Common.CommonFuncObj.CommonFunc_TargetReward _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)62; }

/** 目标奖励信息 */
public Common.CommonFuncObj.CommonFunc_TargetReward getInfo() { return info; }
/** 目标奖励信息 */
public void setInfo(Common.CommonFuncObj.CommonFunc_TargetReward _info) { info = _info; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)62);
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

