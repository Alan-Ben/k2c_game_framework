using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_MsgOp
{

/// <summary>
/// 获得房间历史记录
/// </summary>
public class GS2GC_002_001_RetRoomHistory : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 错误码
/// </summary>
private int errCode;
/// <summary>
/// 回调序列号
/// </summary>
private long callBackSerial;
/// <summary>
/// 房间聊天信息
/// </summary>
private List<Common.CommObj.RoomMsg> roomMsgList;
/// <summary>
/// 历史记录结束
/// </summary>
private bool isEnd;


public GS2GC_002_001_RetRoomHistory() {
	errCode = 0;
	callBackSerial = (long)0;
	roomMsgList = new List<Common.CommObj.RoomMsg>();
	isEnd = false;
}

public GS2GC_002_001_RetRoomHistory(
	int _errCode
	, long _callBackSerial
	, List<Common.CommObj.RoomMsg> _roomMsgList
	, bool _isEnd
) {	errCode = _errCode;
	callBackSerial = _callBackSerial;
	roomMsgList = _roomMsgList;
	isEnd = _isEnd;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)1; }

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
/// <summary>
/// 房间聊天信息
/// </summary>
public List<Common.CommObj.RoomMsg> getRoomMsgList() { return roomMsgList; }
/// <summary>
/// 房间聊天信息
/// </summary>
public void addRoomMsgList(Common.CommObj.RoomMsg _roomMsgList) { roomMsgList.Add(_roomMsgList); }
/// <summary>
/// 历史记录结束
/// </summary>
public bool getIsEnd() { return isEnd; }
/// <summary>
/// 历史记录结束
/// </summary>
public void setIsEnd(bool _isEnd) { isEnd = _isEnd; }


public int GetBufSize() {
	int _size = 13;
	_size += 2;
for(int _i = 0; _i < roomMsgList.Count; _i++) {
	_size += 4 + roomMsgList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;
	_size += 2;
for(int _i = 0; _i < roomMsgList.Count; _i++) {
	_size += 4 + roomMsgList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	callBackSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _roomMsgListCount = _buf.getShort();
	for(int _i = 0; _i < _roomMsgListCount; _i++) { 
		Common.CommObj.RoomMsg _roomMsgList = new Common.CommObj.RoomMsg();
		int __roomMsgListCustLen = _buf.getInt();
	int __roomMsgListCurPos = _buf.getCurPos();
	_roomMsgList.ReadUnzipBuf(_buf, __roomMsgListCurPos + __roomMsgListCustLen);
	_buf.setPosition(__roomMsgListCurPos + __roomMsgListCustLen);

		roomMsgList.Add(_roomMsgList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isEnd = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
	_buf.putLong(callBackSerial);
	_buf.putShort((short)roomMsgList.Count);
	for(int _i = 0; _i < roomMsgList.Count; _i++) { 
		_buf.putInt(roomMsgList[_i].GetBufSize());
	roomMsgList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(isEnd?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)1);
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
	builder.Append("roomMsgList").Append(":").Append(roomMsgList.ToString()).Append(", ");
	builder.Append("isEnd").Append(":").Append(isEnd.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

