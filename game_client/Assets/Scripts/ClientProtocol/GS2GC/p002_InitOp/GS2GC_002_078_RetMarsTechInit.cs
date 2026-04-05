using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 火星科技数据初始化
/// </summary>
public class GS2GC_002_078_RetMarsTechInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 科技列表
/// </summary>
private List<Common.MarsObj.Mars_Technology> technologyList;


public GS2GC_002_078_RetMarsTechInit() {
	technologyList = new List<Common.MarsObj.Mars_Technology>();
}

public GS2GC_002_078_RetMarsTechInit(
	List<Common.MarsObj.Mars_Technology> _technologyList
) {	technologyList = _technologyList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)78; }

/// <summary>
/// 科技列表
/// </summary>
public List<Common.MarsObj.Mars_Technology> getTechnologyList() { return technologyList; }
/// <summary>
/// 科技列表
/// </summary>
public void addTechnologyList(Common.MarsObj.Mars_Technology _technologyList) { technologyList.Add(_technologyList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (technologyList.Count * 49);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (technologyList.Count * 49);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _technologyListCount = _buf.getShort();
	for(int _i = 0; _i < _technologyListCount; _i++) { 
		Common.MarsObj.Mars_Technology _technologyList = new Common.MarsObj.Mars_Technology();
		int __technologyListCustLen = _buf.getInt();
	int __technologyListCurPos = _buf.getCurPos();
	_technologyList.ReadUnzipBuf(_buf, __technologyListCurPos + __technologyListCustLen);
	_buf.setPosition(__technologyListCurPos + __technologyListCustLen);

		technologyList.Add(_technologyList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)technologyList.Count);
	for(int _i = 0; _i < technologyList.Count; _i++) { 
		_buf.putInt(technologyList[_i].GetBufSize());
	technologyList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)78);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)78);
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
	builder.Append("technologyList").Append(":").Append(technologyList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

