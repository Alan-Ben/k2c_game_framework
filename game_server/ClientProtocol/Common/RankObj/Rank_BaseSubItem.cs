using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.RankObj
{

/// <summary>
/// 排行榜基础子数据
/// </summary>
public class Rank_BaseSubItem : ALBasicProtocolPack._IALProtocolStructure {
private long key;
/// <summary>
/// 分数来源id 针对大臣....
/// </summary>
private long sourceId;
private long score;


public Rank_BaseSubItem() {
	key = (long)0;
	sourceId = (long)0;
	score = (long)0;
}

public Rank_BaseSubItem(
	long _key
	, long _sourceId
	, long _score
) {	key = _key;
	sourceId = _sourceId;
	score = _score;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getKey() { return key; }
public void setKey(long _key) { key = _key; }
/// <summary>
/// 分数来源id 针对大臣....
/// </summary>
public long getSourceId() { return sourceId; }
/// <summary>
/// 分数来源id 针对大臣....
/// </summary>
public void setSourceId(long _sourceId) { sourceId = _sourceId; }
public long getScore() { return score; }
public void setScore(long _score) { score = _score; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	key = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sourceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	score = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(key);
	_buf.putLong(sourceId);
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
	builder.Append("key").Append(":").Append(key.ToString()).Append(", ");
	builder.Append("sourceId").Append(":").Append(sourceId.ToString()).Append(", ");
	builder.Append("score").Append(":").Append(score.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

