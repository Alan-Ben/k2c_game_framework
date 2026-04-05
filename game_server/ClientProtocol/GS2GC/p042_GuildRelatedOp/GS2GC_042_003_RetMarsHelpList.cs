using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

public class GS2GC_042_003_RetMarsHelpList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家自身发起的火星求助数据列表
/// </summary>
private List<Common.GuildObj.Guild_MarsHelpInfo> myHelpList;
/// <summary>
/// 求助数据列表
/// </summary>
private List<Common.GuildObj.Guild_MarsHelpShowInfo> helpList;


public GS2GC_042_003_RetMarsHelpList() {
	myHelpList = new List<Common.GuildObj.Guild_MarsHelpInfo>();
	helpList = new List<Common.GuildObj.Guild_MarsHelpShowInfo>();
}

public GS2GC_042_003_RetMarsHelpList(
	List<Common.GuildObj.Guild_MarsHelpInfo> _myHelpList
	, List<Common.GuildObj.Guild_MarsHelpShowInfo> _helpList
) {	myHelpList = _myHelpList;
	helpList = _helpList;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 玩家自身发起的火星求助数据列表
/// </summary>
public List<Common.GuildObj.Guild_MarsHelpInfo> getMyHelpList() { return myHelpList; }
/// <summary>
/// 玩家自身发起的火星求助数据列表
/// </summary>
public void addMyHelpList(Common.GuildObj.Guild_MarsHelpInfo _myHelpList) { myHelpList.Add(_myHelpList); }
/// <summary>
/// 求助数据列表
/// </summary>
public List<Common.GuildObj.Guild_MarsHelpShowInfo> getHelpList() { return helpList; }
/// <summary>
/// 求助数据列表
/// </summary>
public void addHelpList(Common.GuildObj.Guild_MarsHelpShowInfo _helpList) { helpList.Add(_helpList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < myHelpList.Count; _i++) {
	_size += 4 + myHelpList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < helpList.Count; _i++) {
	_size += 4 + helpList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < myHelpList.Count; _i++) {
	_size += 4 + myHelpList[_i].GetBufSize();
	}

	_size += 2;
for(int _i = 0; _i < helpList.Count; _i++) {
	_size += 4 + helpList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _myHelpListCount = _buf.getShort();
	for(int _i = 0; _i < _myHelpListCount; _i++) { 
		Common.GuildObj.Guild_MarsHelpInfo _myHelpList = new Common.GuildObj.Guild_MarsHelpInfo();
		int __myHelpListCustLen = _buf.getInt();
	int __myHelpListCurPos = _buf.getCurPos();
	_myHelpList.ReadUnzipBuf(_buf, __myHelpListCurPos + __myHelpListCustLen);
	_buf.setPosition(__myHelpListCurPos + __myHelpListCustLen);

		myHelpList.Add(_myHelpList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _helpListCount = _buf.getShort();
	for(int _i = 0; _i < _helpListCount; _i++) { 
		Common.GuildObj.Guild_MarsHelpShowInfo _helpList = new Common.GuildObj.Guild_MarsHelpShowInfo();
		int __helpListCustLen = _buf.getInt();
	int __helpListCurPos = _buf.getCurPos();
	_helpList.ReadUnzipBuf(_buf, __helpListCurPos + __helpListCustLen);
	_buf.setPosition(__helpListCurPos + __helpListCustLen);

		helpList.Add(_helpList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)myHelpList.Count);
	for(int _i = 0; _i < myHelpList.Count; _i++) { 
		_buf.putInt(myHelpList[_i].GetBufSize());
	myHelpList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)helpList.Count);
	for(int _i = 0; _i < helpList.Count; _i++) { 
		_buf.putInt(helpList[_i].GetBufSize());
	helpList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)3);
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
	builder.Append("myHelpList").Append(":").Append(myHelpList.ToString()).Append(", ");
	builder.Append("helpList").Append(":").Append(helpList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

