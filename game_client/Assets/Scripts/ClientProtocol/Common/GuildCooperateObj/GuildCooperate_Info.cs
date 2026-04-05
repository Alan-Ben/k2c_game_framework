using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildCooperateObj
{

/// <summary>
/// 联盟协作信息
/// </summary>
public class GuildCooperate_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 下次刷新时间
/// </summary>
private long nextRefreshTimeMs;
/// <summary>
/// 已重置次数
/// </summary>
private int resetCount;
/// <summary>
/// 奖励据点信息列表
/// </summary>
private List<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo> pointList;
/// <summary>
/// 推荐奖励据点
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos recommendPos;


public GuildCooperate_Info() {
	nextRefreshTimeMs = (long)0;
	resetCount = 0;
	pointList = new List<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo>();
	recommendPos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
}

public GuildCooperate_Info(
	long _nextRefreshTimeMs
	, int _resetCount
	, List<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo> _pointList
	, Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos
) {	nextRefreshTimeMs = _nextRefreshTimeMs;
	resetCount = _resetCount;
	pointList = _pointList;
	recommendPos = _recommendPos;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 下次刷新时间
/// </summary>
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/// <summary>
/// 下次刷新时间
/// </summary>
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/// <summary>
/// 已重置次数
/// </summary>
public int getResetCount() { return resetCount; }
/// <summary>
/// 已重置次数
/// </summary>
public void setResetCount(int _resetCount) { resetCount = _resetCount; }
/// <summary>
/// 奖励据点信息列表
/// </summary>
public List<Common.GuildCooperateObj.GuildCooperate_RewardPointInfo> getPointList() { return pointList; }
/// <summary>
/// 奖励据点信息列表
/// </summary>
public void addPointList(Common.GuildCooperateObj.GuildCooperate_RewardPointInfo _pointList) { pointList.Add(_pointList); }
/// <summary>
/// 推荐奖励据点
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getRecommendPos() { return recommendPos; }
/// <summary>
/// 推荐奖励据点
/// </summary>
public void setRecommendPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos) { recommendPos = _recommendPos; }


public int GetBufSize() {
	int _size = 28;
	_size += 2;
for(int _i = 0; _i < pointList.Count; _i++) {
	_size += 4 + pointList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += 2;
for(int _i = 0; _i < pointList.Count; _i++) {
	_size += 4 + pointList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	resetCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _pointListCount = _buf.getShort();
	for(int _i = 0; _i < _pointListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_RewardPointInfo _pointList = new Common.GuildCooperateObj.GuildCooperate_RewardPointInfo();
		int __pointListCustLen = _buf.getInt();
	int __pointListCurPos = _buf.getCurPos();
	_pointList.ReadUnzipBuf(_buf, __pointListCurPos + __pointListCustLen);
	_buf.setPosition(__pointListCurPos + __pointListCustLen);

		pointList.Add(_pointList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _recommendPosCustLen = _buf.getInt();
	int _recommendPosCurPos = _buf.getCurPos();
	recommendPos.ReadUnzipBuf(_buf, _recommendPosCurPos + _recommendPosCustLen);
	_buf.setPosition(_recommendPosCurPos + _recommendPosCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(nextRefreshTimeMs);
	_buf.putInt(resetCount);
	_buf.putShort((short)pointList.Count);
	for(int _i = 0; _i < pointList.Count; _i++) { 
		_buf.putInt(pointList[_i].GetBufSize());
	pointList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(recommendPos.GetBufSize());
	recommendPos.PutUnzipBuf(_buf);
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
	builder.Append("nextRefreshTimeMs").Append(":").Append(nextRefreshTimeMs.ToString()).Append(", ");
	builder.Append("resetCount").Append(":").Append(resetCount.ToString()).Append(", ");
	builder.Append("pointList").Append(":").Append(pointList.ToString()).Append(", ");
	builder.Append("recommendPos").Append(":").Append(recommendPos == null ? "null" : recommendPos.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

