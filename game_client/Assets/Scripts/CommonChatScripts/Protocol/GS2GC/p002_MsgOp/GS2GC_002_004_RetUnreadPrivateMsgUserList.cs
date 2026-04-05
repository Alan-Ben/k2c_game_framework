using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_MsgOp
{

public class GS2GC_002_004_RetUnreadPrivateMsgUserList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 错误码
/// </summary>
private int errCode;
/// <summary>
/// 回调序列号
/// </summary>
private long callBackSerial;
private List<Common.CommObj.PrivateChatUser> userList;


public GS2GC_002_004_RetUnreadPrivateMsgUserList() {
	errCode = 0;
	callBackSerial = (long)0;
	userList = new List<Common.CommObj.PrivateChatUser>();
}

public GS2GC_002_004_RetUnreadPrivateMsgUserList(
	int _errCode
	, long _callBackSerial
	, List<Common.CommObj.PrivateChatUser> _userList
) {	errCode = _errCode;
	callBackSerial = _callBackSerial;
	userList = _userList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)4; }

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
public List<Common.CommObj.PrivateChatUser> getUserList() { return userList; }
public void addUserList(Common.CommObj.PrivateChatUser _userList) { userList.Add(_userList); }


public int GetBufSize() {
	int _size = 12;
	_size += 2;
for(int _i = 0; _i < userList.Count; _i++) {
	_size += 4 + userList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += 2;
for(int _i = 0; _i < userList.Count; _i++) {
	_size += 4 + userList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	callBackSerial = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _userListCount = _buf.getShort();
	for(int _i = 0; _i < _userListCount; _i++) { 
		Common.CommObj.PrivateChatUser _userList = new Common.CommObj.PrivateChatUser();
		int __userListCustLen = _buf.getInt();
	int __userListCurPos = _buf.getCurPos();
	_userList.ReadUnzipBuf(_buf, __userListCurPos + __userListCustLen);
	_buf.setPosition(__userListCurPos + __userListCustLen);

		userList.Add(_userList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(errCode);
	_buf.putLong(callBackSerial);
	_buf.putShort((short)userList.Count);
	for(int _i = 0; _i < userList.Count; _i++) { 
		_buf.putInt(userList[_i].GetBufSize());
	userList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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
	builder.Append("errCode").Append(":").Append(errCode.ToString()).Append(", ");
	builder.Append("callBackSerial").Append(":").Append(callBackSerial.ToString()).Append(", ");
	builder.Append("userList").Append(":").Append(userList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

