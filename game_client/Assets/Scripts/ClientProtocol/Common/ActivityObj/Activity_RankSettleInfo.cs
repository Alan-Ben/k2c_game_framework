using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ActivityObj
{

/// <summary>
/// 活动排行榜结算信息
/// </summary>
public class Activity_RankSettleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 排行榜ID
/// </summary>
private long rankId;
/// <summary>
/// 排名
/// </summary>
private long rank;
/// <summary>
/// 是否已领奖
/// </summary>
private bool hadDraw;
/// <summary>
/// 分数
/// </summary>
private long score;


public Activity_RankSettleInfo() {
	rankId = (long)0;
	rank = (long)0;
	hadDraw = false;
	score = (long)0;
}

public Activity_RankSettleInfo(
	long _rankId
	, long _rank
	, bool _hadDraw
	, long _score
) {	rankId = _rankId;
	rank = _rank;
	hadDraw = _hadDraw;
	score = _score;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 排行榜ID
/// </summary>
public long getRankId() { return rankId; }
/// <summary>
/// 排行榜ID
/// </summary>
public void setRankId(long _rankId) { rankId = _rankId; }
/// <summary>
/// 排名
/// </summary>
public long getRank() { return rank; }
/// <summary>
/// 排名
/// </summary>
public void setRank(long _rank) { rank = _rank; }
/// <summary>
/// 是否已领奖
/// </summary>
public bool getHadDraw() { return hadDraw; }
/// <summary>
/// 是否已领奖
/// </summary>
public void setHadDraw(bool _hadDraw) { hadDraw = _hadDraw; }
/// <summary>
/// 分数
/// </summary>
public long getScore() { return score; }
/// <summary>
/// 分数
/// </summary>
public void setScore(long _score) { score = _score; }


public int GetBufSize() {
	int _size = 25;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rank = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDraw = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(rankId);
	_buf.putLong(rank);
	_buf.put(hadDraw?(byte)1:(byte)0);
	_buf.putLong(score);
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
	builder.Append("rankId").Append(":").Append(rankId.ToString()).Append(", ");
	builder.Append("rank").Append(":").Append(rank.ToString()).Append(", ");
	builder.Append("hadDraw").Append(":").Append(hadDraw.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

