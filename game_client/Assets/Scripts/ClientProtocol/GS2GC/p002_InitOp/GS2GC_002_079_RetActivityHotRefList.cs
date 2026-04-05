using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_079_RetActivityHotRefList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动热更配置列表
/// </summary>
private List<Common.ActivityObj.Activity_HotRefInfo> hotRefList;


public GS2GC_002_079_RetActivityHotRefList() {
	hotRefList = new List<Common.ActivityObj.Activity_HotRefInfo>();
}

public GS2GC_002_079_RetActivityHotRefList(
	List<Common.ActivityObj.Activity_HotRefInfo> _hotRefList
) {	hotRefList = _hotRefList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)79; }

/// <summary>
/// 活动热更配置列表
/// </summary>
public List<Common.ActivityObj.Activity_HotRefInfo> getHotRefList() { return hotRefList; }
/// <summary>
/// 活动热更配置列表
/// </summary>
public void addHotRefList(Common.ActivityObj.Activity_HotRefInfo _hotRefList) { hotRefList.Add(_hotRefList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < hotRefList.Count; _i++) {
	_size += 4 + hotRefList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < hotRefList.Count; _i++) {
	_size += 4 + hotRefList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _hotRefListCount = _buf.getShort();
	for(int _i = 0; _i < _hotRefListCount; _i++) { 
		Common.ActivityObj.Activity_HotRefInfo _hotRefList = new Common.ActivityObj.Activity_HotRefInfo();
		int __hotRefListCustLen = _buf.getInt();
	int __hotRefListCurPos = _buf.getCurPos();
	_hotRefList.ReadUnzipBuf(_buf, __hotRefListCurPos + __hotRefListCustLen);
	_buf.setPosition(__hotRefListCurPos + __hotRefListCustLen);

		hotRefList.Add(_hotRefList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)hotRefList.Count);
	for(int _i = 0; _i < hotRefList.Count; _i++) { 
		_buf.putInt(hotRefList[_i].GetBufSize());
	hotRefList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)79);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)79);
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
	builder.Append("hotRefList").Append(":").Append(hotRefList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

