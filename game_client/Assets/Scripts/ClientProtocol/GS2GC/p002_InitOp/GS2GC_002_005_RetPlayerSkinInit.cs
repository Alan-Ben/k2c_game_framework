using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_005_RetPlayerSkinInit : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.NpPlayerInfoObj.PlayerInfo_Skin> skinList;


public GS2GC_002_005_RetPlayerSkinInit() {
	skinList = new List<Common.NpPlayerInfoObj.PlayerInfo_Skin>();
}

public GS2GC_002_005_RetPlayerSkinInit(
	List<Common.NpPlayerInfoObj.PlayerInfo_Skin> _skinList
) {	skinList = _skinList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)5; }

public List<Common.NpPlayerInfoObj.PlayerInfo_Skin> getSkinList() { return skinList; }
public void addSkinList(Common.NpPlayerInfoObj.PlayerInfo_Skin _skinList) { skinList.Add(_skinList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (skinList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (skinList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _skinListCount = _buf.getShort();
	for(int _i = 0; _i < _skinListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_Skin _skinList = new Common.NpPlayerInfoObj.PlayerInfo_Skin();
		int __skinListCustLen = _buf.getInt();
	int __skinListCurPos = _buf.getCurPos();
	_skinList.ReadUnzipBuf(_buf, __skinListCurPos + __skinListCustLen);
	_buf.setPosition(__skinListCurPos + __skinListCustLen);

		skinList.Add(_skinList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)skinList.Count);
	for(int _i = 0; _i < skinList.Count; _i++) { 
		_buf.putInt(skinList[_i].GetBufSize());
	skinList[_i].PutUnzipBuf(_buf);
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
	builder.Append("skinList").Append(":").Append(skinList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

