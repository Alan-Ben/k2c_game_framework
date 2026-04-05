using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p038_MarsOp
{

/// <summary>
/// 前往火星-发送阶段留言
/// </summary>
public class GC2GS_038_004_ReqSendStageMsg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当前阶段
/// </summary>
private int stage;
/// <summary>
/// 留言内容
/// </summary>
private string content;


public GC2GS_038_004_ReqSendStageMsg() {
	stage = 0;
	content = "";
}

public GC2GS_038_004_ReqSendStageMsg(
	int _stage
	, string _content
) {	stage = _stage;
	content = _content;
}

public byte getMainOrder() { return (byte)38; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 当前阶段
/// </summary>
public int getStage() { return stage; }
/// <summary>
/// 当前阶段
/// </summary>
public void setStage(int _stage) { stage = _stage; }
/// <summary>
/// 留言内容
/// </summary>
public string getContent() { return content; }
/// <summary>
/// 留言内容
/// </summary>
public void setContent(string _content) { content = _content; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stage = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stage);
	_buf.putString(content);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)38);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)38);
	_recBuf.put((byte)4);
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
	builder.Append("stage").Append(":").Append(stage.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

