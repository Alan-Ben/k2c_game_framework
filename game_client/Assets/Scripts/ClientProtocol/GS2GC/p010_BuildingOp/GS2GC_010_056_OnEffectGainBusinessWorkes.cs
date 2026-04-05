using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p010_BuildingOp
{

/// <summary>
/// 通过效果获得额外员工推送
/// </summary>
public class GS2GC_010_056_OnEffectGainBusinessWorkes : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 效果获得员工数据列表
/// </summary>
private List<Common.BuildingObj.Building_EffectGainWorkers> list;


public GS2GC_010_056_OnEffectGainBusinessWorkes() {
	list = new List<Common.BuildingObj.Building_EffectGainWorkers>();
}

public GS2GC_010_056_OnEffectGainBusinessWorkes(
	List<Common.BuildingObj.Building_EffectGainWorkers> _list
) {	list = _list;
}

public byte getMainOrder() { return (byte)10; }

public byte getSubOrder() { return (byte)56; }

/// <summary>
/// 效果获得员工数据列表
/// </summary>
public List<Common.BuildingObj.Building_EffectGainWorkers> getList() { return list; }
/// <summary>
/// 效果获得员工数据列表
/// </summary>
public void addList(Common.BuildingObj.Building_EffectGainWorkers _list) { list.Add(_list); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (list.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (list.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _listCount = _buf.getShort();
	for(int _i = 0; _i < _listCount; _i++) { 
		Common.BuildingObj.Building_EffectGainWorkers _list = new Common.BuildingObj.Building_EffectGainWorkers();
		int __listCustLen = _buf.getInt();
	int __listCurPos = _buf.getCurPos();
	_list.ReadUnzipBuf(_buf, __listCurPos + __listCustLen);
	_buf.setPosition(__listCurPos + __listCustLen);

		list.Add(_list);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)list.Count);
	for(int _i = 0; _i < list.Count; _i++) { 
		_buf.putInt(list[_i].GetBufSize());
	list[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)56);
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
	builder.Append("list").Append(":").Append(list.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

