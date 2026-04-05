using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p036_TreasureHuntOp
{

public class GS2GC_036_009_RetTreasureHuntTransOre : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 转换结果列表
/// </summary>
private List<Common.TreasureHuntObj.TreasureHunt_TransOreResult> transResultList;


public GS2GC_036_009_RetTreasureHuntTransOre() {
	transResultList = new List<Common.TreasureHuntObj.TreasureHunt_TransOreResult>();
}

public GS2GC_036_009_RetTreasureHuntTransOre(
	List<Common.TreasureHuntObj.TreasureHunt_TransOreResult> _transResultList
) {	transResultList = _transResultList;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)9; }

/// <summary>
/// 转换结果列表
/// </summary>
public List<Common.TreasureHuntObj.TreasureHunt_TransOreResult> getTransResultList() { return transResultList; }
/// <summary>
/// 转换结果列表
/// </summary>
public void addTransResultList(Common.TreasureHuntObj.TreasureHunt_TransOreResult _transResultList) { transResultList.Add(_transResultList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (transResultList.Count * 17);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (transResultList.Count * 17);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _transResultListCount = _buf.getShort();
	for(int _i = 0; _i < _transResultListCount; _i++) { 
		Common.TreasureHuntObj.TreasureHunt_TransOreResult _transResultList = new Common.TreasureHuntObj.TreasureHunt_TransOreResult();
		int __transResultListCustLen = _buf.getInt();
	int __transResultListCurPos = _buf.getCurPos();
	_transResultList.ReadUnzipBuf(_buf, __transResultListCurPos + __transResultListCustLen);
	_buf.setPosition(__transResultListCurPos + __transResultListCustLen);

		transResultList.Add(_transResultList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)transResultList.Count);
	for(int _i = 0; _i < transResultList.Count; _i++) { 
		_buf.putInt(transResultList[_i].GetBufSize());
	transResultList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)9);
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
	builder.Append("transResultList").Append(":").Append(transResultList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

