using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p004_PlayerOp
{

/// <summary>
/// 举报指定玩家
/// </summary>
public class GC2GS_004_013_ReqReportPlayer : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 举报目标玩家CID
/// </summary>
private long targetCid;
/// <summary>
/// 举报内容
/// </summary>
private string content;


public GC2GS_004_013_ReqReportPlayer() {
	targetCid = (long)0;
	content = "";
}

public GC2GS_004_013_ReqReportPlayer(
	long _targetCid
	, string _content
) {	targetCid = _targetCid;
	content = _content;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)13; }

/// <summary>
/// 举报目标玩家CID
/// </summary>
public long getTargetCid() { return targetCid; }
/// <summary>
/// 举报目标玩家CID
/// </summary>
public void setTargetCid(long _targetCid) { targetCid = _targetCid; }
/// <summary>
/// 举报内容
/// </summary>
public string getContent() { return content; }
/// <summary>
/// 举报内容
/// </summary>
public void setContent(string _content) { content = _content; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(targetCid);
	_buf.putString(content);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)13);
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
	builder.Append("targetCid").Append(":").Append(targetCid.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

