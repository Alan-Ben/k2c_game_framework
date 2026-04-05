using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_052_RetEveningDungeonInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 副本信息
/// </summary>
private Common.DungeonObj.EveningDungeon_Info info;
/// <summary>
/// 副本时间信息
/// </summary>
private Common.DungeonObj.EveningDungeon_TimeInfo timeInfo;


public GS2GC_002_052_RetEveningDungeonInit() {
	info = new Common.DungeonObj.EveningDungeon_Info();
	timeInfo = new Common.DungeonObj.EveningDungeon_TimeInfo();
}

public GS2GC_002_052_RetEveningDungeonInit(
	Common.DungeonObj.EveningDungeon_Info _info
	, Common.DungeonObj.EveningDungeon_TimeInfo _timeInfo
) {	info = _info;
	timeInfo = _timeInfo;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)52; }

/// <summary>
/// 副本信息
/// </summary>
public Common.DungeonObj.EveningDungeon_Info getInfo() { return info; }
/// <summary>
/// 副本信息
/// </summary>
public void setInfo(Common.DungeonObj.EveningDungeon_Info _info) { info = _info; }
/// <summary>
/// 副本时间信息
/// </summary>
public Common.DungeonObj.EveningDungeon_TimeInfo getTimeInfo() { return timeInfo; }
/// <summary>
/// 副本时间信息
/// </summary>
public void setTimeInfo(Common.DungeonObj.EveningDungeon_TimeInfo _timeInfo) { timeInfo = _timeInfo; }


public int GetBufSize() {
	int _size = 36;
	_size += 4 + info.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + info.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.getCurPos();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.setPosition(_infoCurPos + _infoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _timeInfoCustLen = _buf.getInt();
	int _timeInfoCurPos = _buf.getCurPos();
	timeInfo.ReadUnzipBuf(_buf, _timeInfoCurPos + _timeInfoCustLen);
	_buf.setPosition(_timeInfoCurPos + _timeInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
	_buf.putInt(timeInfo.GetBufSize());
	timeInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)52);
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
	builder.Append("info").Append(":").Append(info == null ? "null" : info.ToString()).Append(", ");
	builder.Append("timeInfo").Append(":").Append(timeInfo == null ? "null" : timeInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

