using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.DailyCheckObj
{

/// <summary>
/// 每日签到奖励展示信息
/// </summary>
public class DailyCheck_RewardShowInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// daily_check_loop_reward配置ID
/// </summary>
private long refId;
/// <summary>
/// 当前天数
/// </summary>
private int curDay;


public DailyCheck_RewardShowInfo() {
	refId = (long)0;
	curDay = 0;
}

public DailyCheck_RewardShowInfo(
	long _refId
	, int _curDay
) {	refId = _refId;
	curDay = _curDay;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// daily_check_loop_reward配置ID
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// daily_check_loop_reward配置ID
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 当前天数
/// </summary>
public int getCurDay() { return curDay; }
/// <summary>
/// 当前天数
/// </summary>
public void setCurDay(int _curDay) { curDay = _curDay; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curDay = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refId);
	_buf.putInt(curDay);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("curDay").Append(":").Append(curDay.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

