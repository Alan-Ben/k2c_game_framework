package NP2SS_R.p001_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 命令推送资源更新信息
 **/
public class ToSS_R_001_017_CmdPushResUpdateInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 后台资源更新数据 */
private Common.ScheduleObj.Schedule_ResUpdateInfo resUpdateInfo;


public ToSS_R_001_017_CmdPushResUpdateInfo() {
	resUpdateInfo = new Common.ScheduleObj.Schedule_ResUpdateInfo();
}

public ToSS_R_001_017_CmdPushResUpdateInfo(
	 Common.ScheduleObj.Schedule_ResUpdateInfo _resUpdateInfo
) {	resUpdateInfo = _resUpdateInfo;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)17; }

/** 后台资源更新数据 */
public Common.ScheduleObj.Schedule_ResUpdateInfo getResUpdateInfo() { return resUpdateInfo; }
/** 后台资源更新数据 */
public void setResUpdateInfo(Common.ScheduleObj.Schedule_ResUpdateInfo _resUpdateInfo) { resUpdateInfo = _resUpdateInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + resUpdateInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + resUpdateInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _resUpdateInfoCustLen = _buf.getInt();
	int _resUpdateInfoCurPos = _buf.position();
	resUpdateInfo.ReadUnzipBuf(_buf, _resUpdateInfoCurPos + _resUpdateInfoCustLen);
	_buf.position(_resUpdateInfoCurPos + _resUpdateInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(resUpdateInfo.GetBufSize());
	resUpdateInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)17);
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

