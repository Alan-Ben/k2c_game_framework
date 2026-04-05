using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.NumMergeObj
{

/// <summary>
/// 数字合并-宝箱信息
/// </summary>
public class NumMerge_BoxInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 领取到的宝箱等级
/// </summary>
private int level;
/// <summary>
/// 当前剩余分数
/// </summary>
private long score;


public NumMerge_BoxInfo() {
	level = 0;
	score = (long)0;
}

public NumMerge_BoxInfo(
	int _level
	, long _score
) {	level = _level;
	score = _score;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 领取到的宝箱等级
/// </summary>
public int getLevel() { return level; }
/// <summary>
/// 领取到的宝箱等级
/// </summary>
public void setLevel(int _level) { level = _level; }
/// <summary>
/// 当前剩余分数
/// </summary>
public long getScore() { return score; }
/// <summary>
/// 当前剩余分数
/// </summary>
public void setScore(long _score) { score = _score; }


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
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(level);
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
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

