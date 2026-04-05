using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_004_RetEquipInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 藏品列表
/// </summary>
private List<Common.HeroObj.Equip_BaseInfo> equipList;


public GS2GC_002_004_RetEquipInit() {
	equipList = new List<Common.HeroObj.Equip_BaseInfo>();
}

public GS2GC_002_004_RetEquipInit(
	List<Common.HeroObj.Equip_BaseInfo> _equipList
) {	equipList = _equipList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 藏品列表
/// </summary>
public List<Common.HeroObj.Equip_BaseInfo> getEquipList() { return equipList; }
/// <summary>
/// 藏品列表
/// </summary>
public void addEquipList(Common.HeroObj.Equip_BaseInfo _equipList) { equipList.Add(_equipList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (equipList.Count * 41);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (equipList.Count * 41);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _equipListCount = _buf.getShort();
	for(int _i = 0; _i < _equipListCount; _i++) { 
		Common.HeroObj.Equip_BaseInfo _equipList = new Common.HeroObj.Equip_BaseInfo();
		int __equipListCustLen = _buf.getInt();
	int __equipListCurPos = _buf.getCurPos();
	_equipList.ReadUnzipBuf(_buf, __equipListCurPos + __equipListCustLen);
	_buf.setPosition(__equipListCurPos + __equipListCustLen);

		equipList.Add(_equipList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)equipList.Count);
	for(int _i = 0; _i < equipList.Count; _i++) { 
		_buf.putInt(equipList[_i].GetBufSize());
	equipList[_i].PutUnzipBuf(_buf);
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
	builder.Append("equipList").Append(":").Append(equipList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

