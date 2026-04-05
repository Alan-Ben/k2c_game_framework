using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟成员当日信息
/// </summary>
public class Guild_MemberDailyData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日期 用于每天重置数据
/// </summary>
private int date;
/// <summary>
/// 当天已领取建设奖励列表
/// </summary>
private List<int> todayDrawConstructRewardList;


public Guild_MemberDailyData() {
	date = 0;
	todayDrawConstructRewardList = new List<int>();
}

public Guild_MemberDailyData(
	int _date
	, List<int> _todayDrawConstructRewardList
) {	date = _date;
	todayDrawConstructRewardList = _todayDrawConstructRewardList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 日期 用于每天重置数据
/// </summary>
public int getDate() { return date; }
/// <summary>
/// 日期 用于每天重置数据
/// </summary>
public void setDate(int _date) { date = _date; }
/// <summary>
/// 当天已领取建设奖励列表
/// </summary>
public List<int> getTodayDrawConstructRewardList() { return todayDrawConstructRewardList; }
/// <summary>
/// 当天已领取建设奖励列表
/// </summary>
public void addTodayDrawConstructRewardList(int _todayDrawConstructRewardList) { todayDrawConstructRewardList.Add(_todayDrawConstructRewardList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (todayDrawConstructRewardList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (todayDrawConstructRewardList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	date = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _todayDrawConstructRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _todayDrawConstructRewardListCount; _i++) { 
		int _todayDrawConstructRewardList = 0;
		_todayDrawConstructRewardList = _buf.getInt();
		todayDrawConstructRewardList.Add(_todayDrawConstructRewardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(date);
	_buf.putShort((short)todayDrawConstructRewardList.Count);
	for(int _i = 0; _i < todayDrawConstructRewardList.Count; _i++) { 
		_buf.putInt(todayDrawConstructRewardList[_i]);
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
	builder.Append("date").Append(":").Append(date.ToString()).Append(", ");
	builder.Append("todayDrawConstructRewardList").Append(":").Append(todayDrawConstructRewardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

