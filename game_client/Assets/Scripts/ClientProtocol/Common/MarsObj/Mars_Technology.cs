using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MarsObj
{

/// <summary>
/// 火星科技-基础数据
/// </summary>
public class Mars_Technology : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 科技ID
/// </summary>
private long technologyId;
/// <summary>
/// 科技等级
/// </summary>
private int lvl;
/// <summary>
/// 是否升级中
/// </summary>
private bool isUpgrading;
/// <summary>
/// 建筑开始升级时间（毫秒）
/// </summary>
private long startUpgradeLvlMs;
/// <summary>
/// 建筑结束升级时间（毫秒）
/// </summary>
private long endUpgradeLvlMs;
/// <summary>
/// 公会求助数据ID
/// </summary>
private long guildHelpId;
/// <summary>
/// 公会求助时长（秒）
/// </summary>
private int guildHelpSecs;
/// <summary>
/// 道具求助时长（秒）
/// </summary>
private int itemHelpSecs;


public Mars_Technology() {
	technologyId = (long)0;
	lvl = 0;
	isUpgrading = false;
	startUpgradeLvlMs = (long)0;
	endUpgradeLvlMs = (long)0;
	guildHelpId = (long)0;
	guildHelpSecs = 0;
	itemHelpSecs = 0;
}

public Mars_Technology(
	long _technologyId
	, int _lvl
	, bool _isUpgrading
	, long _startUpgradeLvlMs
	, long _endUpgradeLvlMs
	, long _guildHelpId
	, int _guildHelpSecs
	, int _itemHelpSecs
) {	technologyId = _technologyId;
	lvl = _lvl;
	isUpgrading = _isUpgrading;
	startUpgradeLvlMs = _startUpgradeLvlMs;
	endUpgradeLvlMs = _endUpgradeLvlMs;
	guildHelpId = _guildHelpId;
	guildHelpSecs = _guildHelpSecs;
	itemHelpSecs = _itemHelpSecs;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 科技ID
/// </summary>
public long getTechnologyId() { return technologyId; }
/// <summary>
/// 科技ID
/// </summary>
public void setTechnologyId(long _technologyId) { technologyId = _technologyId; }
/// <summary>
/// 科技等级
/// </summary>
public int getLvl() { return lvl; }
/// <summary>
/// 科技等级
/// </summary>
public void setLvl(int _lvl) { lvl = _lvl; }
/// <summary>
/// 是否升级中
/// </summary>
public bool getIsUpgrading() { return isUpgrading; }
/// <summary>
/// 是否升级中
/// </summary>
public void setIsUpgrading(bool _isUpgrading) { isUpgrading = _isUpgrading; }
/// <summary>
/// 建筑开始升级时间（毫秒）
/// </summary>
public long getStartUpgradeLvlMs() { return startUpgradeLvlMs; }
/// <summary>
/// 建筑开始升级时间（毫秒）
/// </summary>
public void setStartUpgradeLvlMs(long _startUpgradeLvlMs) { startUpgradeLvlMs = _startUpgradeLvlMs; }
/// <summary>
/// 建筑结束升级时间（毫秒）
/// </summary>
public long getEndUpgradeLvlMs() { return endUpgradeLvlMs; }
/// <summary>
/// 建筑结束升级时间（毫秒）
/// </summary>
public void setEndUpgradeLvlMs(long _endUpgradeLvlMs) { endUpgradeLvlMs = _endUpgradeLvlMs; }
/// <summary>
/// 公会求助数据ID
/// </summary>
public long getGuildHelpId() { return guildHelpId; }
/// <summary>
/// 公会求助数据ID
/// </summary>
public void setGuildHelpId(long _guildHelpId) { guildHelpId = _guildHelpId; }
/// <summary>
/// 公会求助时长（秒）
/// </summary>
public int getGuildHelpSecs() { return guildHelpSecs; }
/// <summary>
/// 公会求助时长（秒）
/// </summary>
public void setGuildHelpSecs(int _guildHelpSecs) { guildHelpSecs = _guildHelpSecs; }
/// <summary>
/// 道具求助时长（秒）
/// </summary>
public int getItemHelpSecs() { return itemHelpSecs; }
/// <summary>
/// 道具求助时长（秒）
/// </summary>
public void setItemHelpSecs(int _itemHelpSecs) { itemHelpSecs = _itemHelpSecs; }


public int GetBufSize() {
	int _size = 45;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 47;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	technologyId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isUpgrading = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startUpgradeLvlMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	endUpgradeLvlMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildHelpId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildHelpSecs = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	itemHelpSecs = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(technologyId);
	_buf.putInt(lvl);
	_buf.put(isUpgrading?(byte)1:(byte)0);
	_buf.putLong(startUpgradeLvlMs);
	_buf.putLong(endUpgradeLvlMs);
	_buf.putLong(guildHelpId);
	_buf.putInt(guildHelpSecs);
	_buf.putInt(itemHelpSecs);
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
	builder.Append("technologyId").Append(":").Append(technologyId.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("isUpgrading").Append(":").Append(isUpgrading.ToString()).Append(", ");
	builder.Append("startUpgradeLvlMs").Append(":").Append(startUpgradeLvlMs.ToString()).Append(", ");
	builder.Append("endUpgradeLvlMs").Append(":").Append(endUpgradeLvlMs.ToString()).Append(", ");
	builder.Append("guildHelpId").Append(":").Append(guildHelpId.ToString()).Append(", ");
	builder.Append("guildHelpSecs").Append(":").Append(guildHelpSecs.ToString()).Append(", ");
	builder.Append("itemHelpSecs").Append(":").Append(itemHelpSecs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

