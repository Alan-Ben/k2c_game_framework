using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_055_RetConsortChatInit : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.ConsortObj.Consort_ChatInfo> chatList;


public GS2GC_002_055_RetConsortChatInit() {
	chatList = new List<Common.ConsortObj.Consort_ChatInfo>();
}

public GS2GC_002_055_RetConsortChatInit(
	List<Common.ConsortObj.Consort_ChatInfo> _chatList
) {	chatList = _chatList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)55; }

public List<Common.ConsortObj.Consort_ChatInfo> getChatList() { return chatList; }
public void addChatList(Common.ConsortObj.Consort_ChatInfo _chatList) { chatList.Add(_chatList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < chatList.Count; _i++) {
	_size += 4 + chatList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < chatList.Count; _i++) {
	_size += 4 + chatList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _chatListCount = _buf.getShort();
	for(int _i = 0; _i < _chatListCount; _i++) { 
		Common.ConsortObj.Consort_ChatInfo _chatList = new Common.ConsortObj.Consort_ChatInfo();
		int __chatListCustLen = _buf.getInt();
	int __chatListCurPos = _buf.getCurPos();
	_chatList.ReadUnzipBuf(_buf, __chatListCurPos + __chatListCustLen);
	_buf.setPosition(__chatListCurPos + __chatListCustLen);

		chatList.Add(_chatList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)chatList.Count);
	for(int _i = 0; _i < chatList.Count; _i++) { 
		_buf.putInt(chatList[_i].GetBufSize());
	chatList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)55);
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
	builder.Append("chatList").Append(":").Append(chatList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

