using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p038_MarsOp
{

/// <summary>
/// 前往火星-阶段留言列表响应
/// </summary>
public class GS2GC_038_005_RetGetStageMsgList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阶段
/// </summary>
private int stage;
/// <summary>
/// 留言列表
/// </summary>
private List<Common.MarsObj.Mars_GoRoute_StageMsg> msgList;


public GS2GC_038_005_RetGetStageMsgList() {
	stage = 0;
	msgList = new List<Common.MarsObj.Mars_GoRoute_StageMsg>();
}

public GS2GC_038_005_RetGetStageMsgList(
	int _stage
	, List<Common.MarsObj.Mars_GoRoute_StageMsg> _msgList
) {	stage = _stage;
	msgList = _msgList;
}

public byte getMainOrder() { return (byte)38; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 阶段
/// </summary>
public int getStage() { return stage; }
/// <summary>
/// 阶段
/// </summary>
public void setStage(int _stage) { stage = _stage; }
/// <summary>
/// 留言列表
/// </summary>
public List<Common.MarsObj.Mars_GoRoute_StageMsg> getMsgList() { return msgList; }
/// <summary>
/// 留言列表
/// </summary>
public void addMsgList(Common.MarsObj.Mars_GoRoute_StageMsg _msgList) { msgList.Add(_msgList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	stage = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _msgListCount = _buf.getShort();
	for(int _i = 0; _i < _msgListCount; _i++) { 
		Common.MarsObj.Mars_GoRoute_StageMsg _msgList = new Common.MarsObj.Mars_GoRoute_StageMsg();
		int __msgListCustLen = _buf.getInt();
	int __msgListCurPos = _buf.getCurPos();
	_msgList.ReadUnzipBuf(_buf, __msgListCurPos + __msgListCustLen);
	_buf.setPosition(__msgListCurPos + __msgListCustLen);

		msgList.Add(_msgList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(stage);
	_buf.putShort((short)msgList.Count);
	for(int _i = 0; _i < msgList.Count; _i++) { 
		_buf.putInt(msgList[_i].GetBufSize());
	msgList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)38);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)38);
	_recBuf.put((byte)5);
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
	builder.Append("msgList").Append(":").Append(msgList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

