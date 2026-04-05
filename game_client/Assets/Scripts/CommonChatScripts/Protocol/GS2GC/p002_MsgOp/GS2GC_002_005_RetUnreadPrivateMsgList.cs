using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_MsgOp
{

public class GS2GC_002_005_RetUnreadPrivateMsgList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 错误码
/// </summary>
private int errCode;
/// <summary>
/// 回调序列号
/// </summary>
private long callBackSerial;
private List<Common.CommObj.PrivateChatMsg> msgList;


public GS2GC_002_005_RetUnreadPrivateMsgList() {
	errCode = 0;
	callBackSerial = (long)0;
	msgList = new List<Common.CommObj.PrivateChatMsg>();
}

public GS2GC_002_005_RetUnreadPrivateMsgList(
	int _errCode
	, long _callBackSerial
	, List<Common.CommObj.PrivateChatMsg> _msgList
) {	errCode = _errCode;
	callBackSerial = _callBackSerial;
	msgList = _msgList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 错误码
/// </summary>
public int getErrCode() { return errCode; }
/// <summary>
/// 错误码
/// </summary>
public void setErrCode(int _errCode) { errCode = _errCode; }
/// <summary>
/// 回调序列号
/// </summary>
public long getCallBackSerial() { return callBackSerial; }
/// <summary>
/// 回调序列号
/// </summary>
public void setCallBackSerial(long _callBackSerial) { callBackSerial = _callBackSerial; }
public List<Common.CommObj.PrivateChatMsg> getMsgList() { return msgList; }
public void addMsgList(Common.CommObj.PrivateChatMsg _msgList) { msgList.Add(_msgList); }


public int GetBufSize() {
	int _size = 12;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2;
for(int _i = 0; _i < msgList.Count; _i++) {
	_size += 4 + msgList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	callBackSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _msgListCount = _buf.getShort();
	for(int _i = 0; _i < _msgListCount; _i++) { 
		Common.CommObj.PrivateChatMsg _msgList = new Common.CommObj.PrivateChatMsg();
		int __msgListCustLen = _buf.getInt();
	int __msgListCurPos = _buf.getCurPos();
	_msgList.ReadUnzipBuf(_buf, __msgListCurPos + __msgListCustLen);
	_buf.setPosition(__msgListCurPos + __msgListCustLen);

		msgList.Add(_msgList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
	_buf.putLong(callBackSerial);
	_buf.putShort((short)msgList.Count);
	for(int _i = 0; _i < msgList.Count; _i++) { 
		_buf.putInt(msgList[_i].GetBufSize());
	msgList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("callBackSerial").Append(":").Append(callBackSerial.ToString()).Append(", ");
	builder.Append("msgList").Append(":").Append(msgList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

