using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_021_RetPlayerIcon : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家拥有的头像信息队列
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_Icon> iconInfoList;


public GS2GC_002_021_RetPlayerIcon() {
	iconInfoList = new List<Common.NpPlayerInfoObj.PlayerInfo_Icon>();
}

public GS2GC_002_021_RetPlayerIcon(
	List<Common.NpPlayerInfoObj.PlayerInfo_Icon> _iconInfoList
) {	iconInfoList = _iconInfoList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 玩家拥有的头像信息队列
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_Icon> getIconInfoList() { return iconInfoList; }
/// <summary>
/// 玩家拥有的头像信息队列
/// </summary>
public void addIconInfoList(Common.NpPlayerInfoObj.PlayerInfo_Icon _iconInfoList) { iconInfoList.Add(_iconInfoList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (iconInfoList.Count * 17);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (iconInfoList.Count * 17);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _iconInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _iconInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Icon _iconInfoList = new Common.NpPlayerInfoObj.PlayerInfo_Icon();
		int __iconInfoListCustLen = _buf.getInt();
	int __iconInfoListCurPos = _buf.getCurPos();
	_iconInfoList.ReadUnzipBuf(_buf, __iconInfoListCurPos + __iconInfoListCustLen);
	_buf.setPosition(__iconInfoListCurPos + __iconInfoListCustLen);

		iconInfoList.Add(_iconInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)iconInfoList.Count);
	for(int _i = 0; _i < iconInfoList.Count; _i++) { 
		_buf.putInt(iconInfoList[_i].GetBufSize());
	iconInfoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)21);
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
	builder.Append("iconInfoList").Append(":").Append(iconInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

