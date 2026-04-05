package NP2US_R.p008_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 发送排期数据
 **/
public class NP2US_R_008_010_PushSchedule implements ALBasicProtocolPack._IALProtocolStructure {
/** US排期数据 */
private Common.ScheduleObj.Schedule_UsPushData usSchedule;


public NP2US_R_008_010_PushSchedule() {
	usSchedule = new Common.ScheduleObj.Schedule_UsPushData();
}

public NP2US_R_008_010_PushSchedule(
	 Common.ScheduleObj.Schedule_UsPushData _usSchedule
) {	usSchedule = _usSchedule;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)10; }

/** US排期数据 */
public Common.ScheduleObj.Schedule_UsPushData getUsSchedule() { return usSchedule; }
/** US排期数据 */
public void setUsSchedule(Common.ScheduleObj.Schedule_UsPushData _usSchedule) { usSchedule = _usSchedule; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + usSchedule.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + usSchedule.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _usScheduleCustLen = _buf.getInt();
	int _usScheduleCurPos = _buf.position();
	usSchedule.ReadUnzipBuf(_buf, _usScheduleCurPos + _usScheduleCustLen);
	_buf.position(_usScheduleCurPos + _usScheduleCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(usSchedule.GetBufSize());
	usSchedule.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
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

