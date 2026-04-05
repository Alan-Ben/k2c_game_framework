using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p042_GuildRelatedOp
{

/// <summary>
/// 联盟宝箱奖励物品列表
/// </summary>
public class GS2GC_042_057_OnGuildBoxAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱类型
/// </summary>
private Common.GuildEnum.EGuildBoxType boxType;
/// <summary>
/// 宝箱数据列表
/// </summary>
private List<Common.GuildObj.Guild_BoxInfo> boxList;


public GS2GC_042_057_OnGuildBoxAdd() {
	boxType = 0;
	boxList = new List<Common.GuildObj.Guild_BoxInfo>();
}

public GS2GC_042_057_OnGuildBoxAdd(
	Common.GuildEnum.EGuildBoxType _boxType
	, List<Common.GuildObj.Guild_BoxInfo> _boxList
) {	boxType = _boxType;
	boxList = _boxList;
}

public byte getMainOrder() { return (byte)42; }

public byte getSubOrder() { return (byte)57; }

/// <summary>
/// 宝箱类型
/// </summary>
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
/// <summary>
/// 宝箱类型
/// </summary>
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
/// <summary>
/// 宝箱数据列表
/// </summary>
public List<Common.GuildObj.Guild_BoxInfo> getBoxList() { return boxList; }
/// <summary>
/// 宝箱数据列表
/// </summary>
public void addBoxList(Common.GuildObj.Guild_BoxInfo _boxList) { boxList.Add(_boxList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (boxList.Count * 36);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (boxList.Count * 36);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	boxType = (Common.GuildEnum.EGuildBoxType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _boxListCount = _buf.getShort();
	for(int _i = 0; _i < _boxListCount; _i++) { 
		Common.GuildObj.Guild_BoxInfo _boxList = new Common.GuildObj.Guild_BoxInfo();
		int __boxListCustLen = _buf.getInt();
	int __boxListCurPos = _buf.getCurPos();
	_boxList.ReadUnzipBuf(_buf, __boxListCurPos + __boxListCustLen);
	_buf.setPosition(__boxListCurPos + __boxListCustLen);

		boxList.Add(_boxList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)boxType);

	_buf.putShort((short)boxList.Count);
	for(int _i = 0; _i < boxList.Count; _i++) { 
		_buf.putInt(boxList[_i].GetBufSize());
	boxList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)57);
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
	builder.Append("boxType").Append(":").Append(boxType.ToString()).Append(", ");
	builder.Append("boxList").Append(":").Append(boxList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

