using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DinnerObj
{

/// <summary>
/// 开宴记录-详细数据
/// </summary>
public class Dinner_StartLogInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宾客信息列表
/// </summary>
private List<Common.DinnerObj.Dinner_ResultGuestInfo> guestLog;


public Dinner_StartLogInfo() {
	guestLog = new List<Common.DinnerObj.Dinner_ResultGuestInfo>();
}

public Dinner_StartLogInfo(
	List<Common.DinnerObj.Dinner_ResultGuestInfo> _guestLog
) {	guestLog = _guestLog;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宾客信息列表
/// </summary>
public List<Common.DinnerObj.Dinner_ResultGuestInfo> getGuestLog() { return guestLog; }
/// <summary>
/// 宾客信息列表
/// </summary>
public void addGuestLog(Common.DinnerObj.Dinner_ResultGuestInfo _guestLog) { guestLog.Add(_guestLog); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (guestLog.Count * 40);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (guestLog.Count * 40);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _guestLogCount = _buf.getShort();
	for(int _i = 0; _i < _guestLogCount; _i++) { 
		Common.DinnerObj.Dinner_ResultGuestInfo _guestLog = new Common.DinnerObj.Dinner_ResultGuestInfo();
		int __guestLogCustLen = _buf.getInt();
	int __guestLogCurPos = _buf.getCurPos();
	_guestLog.ReadUnzipBuf(_buf, __guestLogCurPos + __guestLogCustLen);
	_buf.setPosition(__guestLogCurPos + __guestLogCustLen);

		guestLog.Add(_guestLog);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)guestLog.Count);
	for(int _i = 0; _i < guestLog.Count; _i++) { 
		_buf.putInt(guestLog[_i].GetBufSize());
	guestLog[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("guestLog").Append(":").Append(guestLog.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

