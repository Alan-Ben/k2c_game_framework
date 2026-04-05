using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 玩家权限信息变更推送
/// </summary>
public class GS2GC_021_062_OnPlayerPermissionsChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已生效权限ID列表
/// </summary>
private List<long> effectIdList;


public GS2GC_021_062_OnPlayerPermissionsChg() {
	effectIdList = new List<long>();
}

public GS2GC_021_062_OnPlayerPermissionsChg(
	List<long> _effectIdList
) {	effectIdList = _effectIdList;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)62; }

/// <summary>
/// 已生效权限ID列表
/// </summary>
public List<long> getEffectIdList() { return effectIdList; }
/// <summary>
/// 已生效权限ID列表
/// </summary>
public void addEffectIdList(long _effectIdList) { effectIdList.Add(_effectIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (effectIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (effectIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _effectIdListCount = _buf.getShort();
	for(int _i = 0; _i < _effectIdListCount; _i++) { 
		long _effectIdList = (long)0;
		_effectIdList = _buf.getLong();
		effectIdList.Add(_effectIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)effectIdList.Count);
	for(int _i = 0; _i < effectIdList.Count; _i++) { 
		_buf.putLong(effectIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)62);
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
	builder.Append("effectIdList").Append(":").Append(effectIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

