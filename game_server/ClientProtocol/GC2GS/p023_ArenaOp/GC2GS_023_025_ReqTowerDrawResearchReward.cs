using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p023_ArenaOp
{

/// <summary>
/// 爬塔领取研究奖励
/// </summary>
public class GC2GS_023_025_ReqTowerDrawResearchReward : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 章节ID
/// </summary>
private long chapterId;


public GC2GS_023_025_ReqTowerDrawResearchReward() {
	chapterId = (long)0;
}

public GC2GS_023_025_ReqTowerDrawResearchReward(
	long _chapterId
) {	chapterId = _chapterId;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)25; }

/// <summary>
/// 章节ID
/// </summary>
public long getChapterId() { return chapterId; }
/// <summary>
/// 章节ID
/// </summary>
public void setChapterId(long _chapterId) { chapterId = _chapterId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chapterId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(chapterId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)25);
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
	builder.Append("chapterId").Append(":").Append(chapterId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

