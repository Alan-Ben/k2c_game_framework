using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场反击数据
/// </summary>
public class Arena_FightBackInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 展示信息
/// </summary>
private Common.ArenaObj.Arena_BattleReportShow showInfo;
/// <summary>
/// 是否反击
/// </summary>
private bool hadFightBack;


public Arena_FightBackInfo() {
	dbId = (long)0;
	showInfo = new Common.ArenaObj.Arena_BattleReportShow();
	hadFightBack = false;
}

public Arena_FightBackInfo(
	long _dbId
	, Common.ArenaObj.Arena_BattleReportShow _showInfo
	, bool _hadFightBack
) {	dbId = _dbId;
	showInfo = _showInfo;
	hadFightBack = _hadFightBack;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 展示信息
/// </summary>
public Common.ArenaObj.Arena_BattleReportShow getShowInfo() { return showInfo; }
/// <summary>
/// 展示信息
/// </summary>
public void setShowInfo(Common.ArenaObj.Arena_BattleReportShow _showInfo) { showInfo = _showInfo; }
/// <summary>
/// 是否反击
/// </summary>
public bool getHadFightBack() { return hadFightBack; }
/// <summary>
/// 是否反击
/// </summary>
public void setHadFightBack(bool _hadFightBack) { hadFightBack = _hadFightBack; }


public int GetBufSize() {
	int _size = 37;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.getCurPos();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.setPosition(_showInfoCurPos + _showInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadFightBack = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
	_buf.put(hadFightBack?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("showInfo").Append(":").Append(showInfo == null ? "null" : showInfo.ToString()).Append(", ");
	builder.Append("hadFightBack").Append(":").Append(hadFightBack.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

