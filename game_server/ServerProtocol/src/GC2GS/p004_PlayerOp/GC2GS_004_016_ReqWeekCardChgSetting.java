package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 请求修改周卡的设置
 **/
public class GC2GS_004_016_ReqWeekCardChgSetting implements ALBasicProtocolPack._IALProtocolStructure {
/** 周卡设置信息 */
private Common.WeekCardObj.WeekCard_SingleSettingInfo settingInfo;


public GC2GS_004_016_ReqWeekCardChgSetting() {
	settingInfo = new Common.WeekCardObj.WeekCard_SingleSettingInfo();
}

public GC2GS_004_016_ReqWeekCardChgSetting(
	 Common.WeekCardObj.WeekCard_SingleSettingInfo _settingInfo
) {	settingInfo = _settingInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)16; }

/** 周卡设置信息 */
public Common.WeekCardObj.WeekCard_SingleSettingInfo getSettingInfo() { return settingInfo; }
/** 周卡设置信息 */
public void setSettingInfo(Common.WeekCardObj.WeekCard_SingleSettingInfo _settingInfo) { settingInfo = _settingInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + settingInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + settingInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _settingInfoCustLen = _buf.getInt();
	int _settingInfoCurPos = _buf.position();
	settingInfo.ReadUnzipBuf(_buf, _settingInfoCurPos + _settingInfoCustLen);
	_buf.position(_settingInfoCurPos + _settingInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(settingInfo.GetBufSize());
	settingInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)16);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)16);
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

