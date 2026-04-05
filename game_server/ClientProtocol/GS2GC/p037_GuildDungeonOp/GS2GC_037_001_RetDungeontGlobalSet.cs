using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

public class GS2GC_037_001_RetDungeontGlobalSet : ALBasicProtocolPack._IALProtocolStructure {
private List<long> autoStartDungeonIdList;
/// <summary>
/// 自动开启时间：小时
/// </summary>
private int autoHour;
/// <summary>
/// 自动开启时间：分钟
/// </summary>
private int autoMin;


public GS2GC_037_001_RetDungeontGlobalSet() {
	autoStartDungeonIdList = new List<long>();
	autoHour = 0;
	autoMin = 0;
}

public GS2GC_037_001_RetDungeontGlobalSet(
	List<long> _autoStartDungeonIdList
	, int _autoHour
	, int _autoMin
) {	autoStartDungeonIdList = _autoStartDungeonIdList;
	autoHour = _autoHour;
	autoMin = _autoMin;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)1; }

public List<long> getAutoStartDungeonIdList() { return autoStartDungeonIdList; }
public void addAutoStartDungeonIdList(long _autoStartDungeonIdList) { autoStartDungeonIdList.Add(_autoStartDungeonIdList); }
/// <summary>
/// 自动开启时间：小时
/// </summary>
public int getAutoHour() { return autoHour; }
/// <summary>
/// 自动开启时间：小时
/// </summary>
public void setAutoHour(int _autoHour) { autoHour = _autoHour; }
/// <summary>
/// 自动开启时间：分钟
/// </summary>
public int getAutoMin() { return autoMin; }
/// <summary>
/// 自动开启时间：分钟
/// </summary>
public void setAutoMin(int _autoMin) { autoMin = _autoMin; }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (autoStartDungeonIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (autoStartDungeonIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _autoStartDungeonIdListCount = _buf.getShort();
	for(int _i = 0; _i < _autoStartDungeonIdListCount; _i++) { 
		long _autoStartDungeonIdList = (long)0;
		_autoStartDungeonIdList = _buf.getLong();
		autoStartDungeonIdList.Add(_autoStartDungeonIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	autoHour = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	autoMin = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)autoStartDungeonIdList.Count);
	for(int _i = 0; _i < autoStartDungeonIdList.Count; _i++) { 
		_buf.putLong(autoStartDungeonIdList[_i]);
	}
	_buf.putInt(autoHour);
	_buf.putInt(autoMin);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)1);
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
	builder.Append("autoStartDungeonIdList").Append(":").Append(autoStartDungeonIdList.ToString()).Append(", ");
	builder.Append("autoHour").Append(":").Append(autoHour.ToString()).Append(", ");
	builder.Append("autoMin").Append(":").Append(autoMin.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

