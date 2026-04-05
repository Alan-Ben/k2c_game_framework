using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_089_RetForbidChatInit : ALBasicProtocolPack._IALProtocolStructure {
private List<NPCommon.NPCommon_ForbidChatInfo> forbidChatList;


public GS2GC_002_089_RetForbidChatInit() {
	forbidChatList = new List<NPCommon.NPCommon_ForbidChatInfo>();
}

public GS2GC_002_089_RetForbidChatInit(
	List<NPCommon.NPCommon_ForbidChatInfo> _forbidChatList
) {	forbidChatList = _forbidChatList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)89; }

public List<NPCommon.NPCommon_ForbidChatInfo> getForbidChatList() { return forbidChatList; }
public void addForbidChatList(NPCommon.NPCommon_ForbidChatInfo _forbidChatList) { forbidChatList.Add(_forbidChatList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (forbidChatList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (forbidChatList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _forbidChatListCount = _buf.getShort();
	for(int _i = 0; _i < _forbidChatListCount; _i++) { 
		NPCommon.NPCommon_ForbidChatInfo _forbidChatList = new NPCommon.NPCommon_ForbidChatInfo();
		int __forbidChatListCustLen = _buf.getInt();
	int __forbidChatListCurPos = _buf.getCurPos();
	_forbidChatList.ReadUnzipBuf(_buf, __forbidChatListCurPos + __forbidChatListCustLen);
	_buf.setPosition(__forbidChatListCurPos + __forbidChatListCustLen);

		forbidChatList.Add(_forbidChatList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)forbidChatList.Count);
	for(int _i = 0; _i < forbidChatList.Count; _i++) { 
		_buf.putInt(forbidChatList[_i].GetBufSize());
	forbidChatList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)89);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)89);
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
	builder.Append("forbidChatList").Append(":").Append(forbidChatList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

