using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟成员委托信息
/// </summary>
public class Guild_MemberEntrustInfo : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/// <summary>
/// 当日处理次数
/// </summary>
private int dayDealTimes;
/// <summary>
/// 处理总次数
/// </summary>
private int totalDealTimes;


public Guild_MemberEntrustInfo() {
	cid = (long)0;
	dayDealTimes = 0;
	totalDealTimes = 0;
}

public Guild_MemberEntrustInfo(
	long _cid
	, int _dayDealTimes
	, int _totalDealTimes
) {	cid = _cid;
	dayDealTimes = _dayDealTimes;
	totalDealTimes = _totalDealTimes;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 当日处理次数
/// </summary>
public int getDayDealTimes() { return dayDealTimes; }
/// <summary>
/// 当日处理次数
/// </summary>
public void setDayDealTimes(int _dayDealTimes) { dayDealTimes = _dayDealTimes; }
/// <summary>
/// 处理总次数
/// </summary>
public int getTotalDealTimes() { return totalDealTimes; }
/// <summary>
/// 处理总次数
/// </summary>
public void setTotalDealTimes(int _totalDealTimes) { totalDealTimes = _totalDealTimes; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dayDealTimes = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalDealTimes = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putInt(dayDealTimes);
	_buf.putInt(totalDealTimes);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("dayDealTimes").Append(":").Append(dayDealTimes.ToString()).Append(", ");
	builder.Append("totalDealTimes").Append(":").Append(totalDealTimes.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

