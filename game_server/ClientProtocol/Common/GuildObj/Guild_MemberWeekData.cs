using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟成员本周信息
/// </summary>
public class Guild_MemberWeekData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 周标记
/// </summary>
private int weekTag;
/// <summary>
/// 已领取活跃宝箱次数
/// </summary>
private int hadDrawActiveBoxNum;
/// <summary>
/// 获得活跃印章数量
/// </summary>
private int gainActiveStampNum;
/// <summary>
/// 已领取联盟大礼次数
/// </summary>
private int hadDrawGreatRewardNum;


public Guild_MemberWeekData() {
	weekTag = 0;
	hadDrawActiveBoxNum = 0;
	gainActiveStampNum = 0;
	hadDrawGreatRewardNum = 0;
}

public Guild_MemberWeekData(
	int _weekTag
	, int _hadDrawActiveBoxNum
	, int _gainActiveStampNum
	, int _hadDrawGreatRewardNum
) {	weekTag = _weekTag;
	hadDrawActiveBoxNum = _hadDrawActiveBoxNum;
	gainActiveStampNum = _gainActiveStampNum;
	hadDrawGreatRewardNum = _hadDrawGreatRewardNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 周标记
/// </summary>
public int getWeekTag() { return weekTag; }
/// <summary>
/// 周标记
/// </summary>
public void setWeekTag(int _weekTag) { weekTag = _weekTag; }
/// <summary>
/// 已领取活跃宝箱次数
/// </summary>
public int getHadDrawActiveBoxNum() { return hadDrawActiveBoxNum; }
/// <summary>
/// 已领取活跃宝箱次数
/// </summary>
public void setHadDrawActiveBoxNum(int _hadDrawActiveBoxNum) { hadDrawActiveBoxNum = _hadDrawActiveBoxNum; }
/// <summary>
/// 获得活跃印章数量
/// </summary>
public int getGainActiveStampNum() { return gainActiveStampNum; }
/// <summary>
/// 获得活跃印章数量
/// </summary>
public void setGainActiveStampNum(int _gainActiveStampNum) { gainActiveStampNum = _gainActiveStampNum; }
/// <summary>
/// 已领取联盟大礼次数
/// </summary>
public int getHadDrawGreatRewardNum() { return hadDrawGreatRewardNum; }
/// <summary>
/// 已领取联盟大礼次数
/// </summary>
public void setHadDrawGreatRewardNum(int _hadDrawGreatRewardNum) { hadDrawGreatRewardNum = _hadDrawGreatRewardNum; }


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
	weekTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawActiveBoxNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainActiveStampNum = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawGreatRewardNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(weekTag);
	_buf.putInt(hadDrawActiveBoxNum);
	_buf.putInt(gainActiveStampNum);
	_buf.putInt(hadDrawGreatRewardNum);
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
	builder.Append("weekTag").Append(":").Append(weekTag.ToString()).Append(", ");
	builder.Append("hadDrawActiveBoxNum").Append(":").Append(hadDrawActiveBoxNum.ToString()).Append(", ");
	builder.Append("gainActiveStampNum").Append(":").Append(gainActiveStampNum.ToString()).Append(", ");
	builder.Append("hadDrawGreatRewardNum").Append(":").Append(hadDrawGreatRewardNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

