using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_022_RetPlayerIconBgk : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家拥有的头像框信息队列
/// </summary>
private List<Common.NpPlayerInfoObj.PlayerInfo_IconBgk> iconBgkInfoList;


public GS2GC_002_022_RetPlayerIconBgk() {
	iconBgkInfoList = new List<Common.NpPlayerInfoObj.PlayerInfo_IconBgk>();
}

public GS2GC_002_022_RetPlayerIconBgk(
	List<Common.NpPlayerInfoObj.PlayerInfo_IconBgk> _iconBgkInfoList
) {	iconBgkInfoList = _iconBgkInfoList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)22; }

/// <summary>
/// 玩家拥有的头像框信息队列
/// </summary>
public List<Common.NpPlayerInfoObj.PlayerInfo_IconBgk> getIconBgkInfoList() { return iconBgkInfoList; }
/// <summary>
/// 玩家拥有的头像框信息队列
/// </summary>
public void addIconBgkInfoList(Common.NpPlayerInfoObj.PlayerInfo_IconBgk _iconBgkInfoList) { iconBgkInfoList.Add(_iconBgkInfoList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (iconBgkInfoList.Count * 17);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (iconBgkInfoList.Count * 17);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _iconBgkInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _iconBgkInfoListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_IconBgk _iconBgkInfoList = new Common.NpPlayerInfoObj.PlayerInfo_IconBgk();
		int __iconBgkInfoListCustLen = _buf.getInt();
	int __iconBgkInfoListCurPos = _buf.getCurPos();
	_iconBgkInfoList.ReadUnzipBuf(_buf, __iconBgkInfoListCurPos + __iconBgkInfoListCustLen);
	_buf.setPosition(__iconBgkInfoListCurPos + __iconBgkInfoListCustLen);

		iconBgkInfoList.Add(_iconBgkInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)iconBgkInfoList.Count);
	for(int _i = 0; _i < iconBgkInfoList.Count; _i++) { 
		_buf.putInt(iconBgkInfoList[_i].GetBufSize());
	iconBgkInfoList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)22);
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
	builder.Append("iconBgkInfoList").Append(":").Append(iconBgkInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

