package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_037_RetWeekCardInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 周卡信息 */
private Common.WeekCardObj.WeekCard_Info info;
/** 设置信息列表 */
private java.util.ArrayList<Common.WeekCardObj.WeekCard_SingleSettingInfo> settingList;


public GS2GC_002_037_RetWeekCardInit() {
	info = new Common.WeekCardObj.WeekCard_Info();
	settingList = new java.util.ArrayList<Common.WeekCardObj.WeekCard_SingleSettingInfo>();
}

public GS2GC_002_037_RetWeekCardInit(
	 Common.WeekCardObj.WeekCard_Info _info
	, java.util.ArrayList<Common.WeekCardObj.WeekCard_SingleSettingInfo> _settingList
) {	info = _info;
	settingList = _settingList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)37; }

/** 周卡信息 */
public Common.WeekCardObj.WeekCard_Info getInfo() { return info; }
/** 周卡信息 */
public void setInfo(Common.WeekCardObj.WeekCard_Info _info) { info = _info; }
/** 设置信息列表 */
public java.util.ArrayList<Common.WeekCardObj.WeekCard_SingleSettingInfo> getSettingList() { return settingList; }
/** 设置信息列表 */
public void addSettingList(Common.WeekCardObj.WeekCard_SingleSettingInfo _settingList) { settingList.add(_settingList); }


public final int GetBufSize() {
	int _size = 21;
	_size += 2;
	for(int _i = 0; _i < settingList.size(); _i++) {
	_size += 4 + settingList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;
	_size += 2;
	for(int _i = 0; _i < settingList.size(); _i++) {
	_size += 4 + settingList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _settingListCount = _buf.getShort();
	for(int _i = 0; _i < _settingListCount; _i++) { 
		Common.WeekCardObj.WeekCard_SingleSettingInfo _settingList = new Common.WeekCardObj.WeekCard_SingleSettingInfo();
		if(_buf.remaining() <= 0) return;
	int __settingListCustLen = _buf.getInt();
	int __settingListCurPos = _buf.position();
	_settingList.ReadUnzipBuf(_buf, __settingListCurPos + __settingListCustLen);
	_buf.position(__settingListCurPos + __settingListCustLen);

		settingList.add(_settingList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putShort((short)settingList.size());
	for(int _i = 0; _i < settingList.size(); _i++) { 
		_buf.putInt(settingList.get(_i).GetBufSize());
	settingList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)37);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)37);
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

