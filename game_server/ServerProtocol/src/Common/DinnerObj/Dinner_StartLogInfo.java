package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 开宴记录-详细数据
 **/
public class Dinner_StartLogInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宾客信息列表 */
private java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo> guestLog;


public Dinner_StartLogInfo() {
	guestLog = new java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo>();
}

public Dinner_StartLogInfo(
	 java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo> _guestLog
) {	guestLog = _guestLog;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宾客信息列表 */
public java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo> getGuestLog() { return guestLog; }
/** 宾客信息列表 */
public void addGuestLog(Common.DinnerObj.Dinner_ResultGuestInfo _guestLog) { guestLog.add(_guestLog); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (guestLog.size() * 40);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (guestLog.size() * 40);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _guestLogCount = _buf.getShort();
	for(int _i = 0; _i < _guestLogCount; _i++) { 
		Common.DinnerObj.Dinner_ResultGuestInfo _guestLog = new Common.DinnerObj.Dinner_ResultGuestInfo();
		if(_buf.remaining() <= 0) return;
	int __guestLogCustLen = _buf.getInt();
	int __guestLogCurPos = _buf.position();
	_guestLog.ReadUnzipBuf(_buf, __guestLogCurPos + __guestLogCustLen);
	_buf.position(__guestLogCurPos + __guestLogCustLen);

		guestLog.add(_guestLog);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)guestLog.size());
	for(int _i = 0; _i < guestLog.size(); _i++) { 
		_buf.putInt(guestLog.get(_i).GetBufSize());
	guestLog.get(_i).PutUnzipBuf(_buf);
	}
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

