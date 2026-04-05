package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动热更配表信息变更推送
 **/
public class GS2GC_017_062_OnActivityHotRefInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动热更配表信息 */
private Common.ActivityObj.Activity_HotRefInfo hotRefInfo;


public GS2GC_017_062_OnActivityHotRefInfoChg() {
	hotRefInfo = new Common.ActivityObj.Activity_HotRefInfo();
}

public GS2GC_017_062_OnActivityHotRefInfoChg(
	 Common.ActivityObj.Activity_HotRefInfo _hotRefInfo
) {	hotRefInfo = _hotRefInfo;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)62; }

/** 活动热更配表信息 */
public Common.ActivityObj.Activity_HotRefInfo getHotRefInfo() { return hotRefInfo; }
/** 活动热更配表信息 */
public void setHotRefInfo(Common.ActivityObj.Activity_HotRefInfo _hotRefInfo) { hotRefInfo = _hotRefInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + hotRefInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + hotRefInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _hotRefInfoCustLen = _buf.getInt();
	int _hotRefInfoCurPos = _buf.position();
	hotRefInfo.ReadUnzipBuf(_buf, _hotRefInfoCurPos + _hotRefInfoCustLen);
	_buf.position(_hotRefInfoCurPos + _hotRefInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(hotRefInfo.GetBufSize());
	hotRefInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
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

