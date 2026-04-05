using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_028_RetChatEmoteGroupInit : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup> emoteGroupList;


public GS2GC_002_028_RetChatEmoteGroupInit() {
	emoteGroupList = new List<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup>();
}

public GS2GC_002_028_RetChatEmoteGroupInit(
	List<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup> _emoteGroupList
) {	emoteGroupList = _emoteGroupList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)28; }

public List<Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup> getEmoteGroupList() { return emoteGroupList; }
public void addEmoteGroupList(Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup _emoteGroupList) { emoteGroupList.Add(_emoteGroupList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (emoteGroupList.Count * 17);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (emoteGroupList.Count * 17);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _emoteGroupListCount = _buf.getShort();
	for(int _i = 0; _i < _emoteGroupListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup _emoteGroupList = new Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup();
		int __emoteGroupListCustLen = _buf.getInt();
	int __emoteGroupListCurPos = _buf.getCurPos();
	_emoteGroupList.ReadUnzipBuf(_buf, __emoteGroupListCurPos + __emoteGroupListCustLen);
	_buf.setPosition(__emoteGroupListCurPos + __emoteGroupListCustLen);

		emoteGroupList.Add(_emoteGroupList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)emoteGroupList.Count);
	for(int _i = 0; _i < emoteGroupList.Count; _i++) { 
		_buf.putInt(emoteGroupList[_i].GetBufSize());
	emoteGroupList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)28);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)28);
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
	builder.Append("emoteGroupList").Append(":").Append(emoteGroupList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

