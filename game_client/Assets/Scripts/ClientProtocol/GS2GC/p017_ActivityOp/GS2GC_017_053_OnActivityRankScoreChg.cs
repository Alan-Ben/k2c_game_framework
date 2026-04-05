using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动排行榜分数变更
/// </summary>
public class GS2GC_017_053_OnActivityRankScoreChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 排行榜ID
/// </summary>
private long rankId;
/// <summary>
/// 原排名
/// </summary>
private int oriRank;
/// <summary>
/// 排名
/// </summary>
private int curRank;
private long score;


public GS2GC_017_053_OnActivityRankScoreChg() {
	instanceId = (long)0;
	rankId = (long)0;
	oriRank = 0;
	curRank = 0;
	score = (long)0;
}

public GS2GC_017_053_OnActivityRankScoreChg(
	long _instanceId
	, long _rankId
	, int _oriRank
	, int _curRank
	, long _score
) {	instanceId = _instanceId;
	rankId = _rankId;
	oriRank = _oriRank;
	curRank = _curRank;
	score = _score;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 活动实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 排行榜ID
/// </summary>
public long getRankId() { return rankId; }
/// <summary>
/// 排行榜ID
/// </summary>
public void setRankId(long _rankId) { rankId = _rankId; }
/// <summary>
/// 原排名
/// </summary>
public int getOriRank() { return oriRank; }
/// <summary>
/// 原排名
/// </summary>
public void setOriRank(int _oriRank) { oriRank = _oriRank; }
/// <summary>
/// 排名
/// </summary>
public int getCurRank() { return curRank; }
/// <summary>
/// 排名
/// </summary>
public void setCurRank(int _curRank) { curRank = _curRank; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oriRank = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	curRank = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(rankId);
	_buf.putInt(oriRank);
	_buf.putInt(curRank);
	_buf.putLong(score);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)53);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("rankId").Append(":").Append(rankId.ToString()).Append(", ");
	builder.Append("oriRank").Append(":").Append(oriRank.ToString()).Append(", ");
	builder.Append("curRank").Append(":").Append(curRank.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

