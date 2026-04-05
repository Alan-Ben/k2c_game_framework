using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.OfflineRewardObj
{

/// <summary>
/// 玩家加入联盟
/// </summary>
public class Offline_PlayerJoinGuild : ALBasicProtocolPack._IALProtocolStructure {
private long newGuildId;
private string guildName;
private string simpleName;
private List<int> buildingAddPerArr;


public Offline_PlayerJoinGuild() {
	newGuildId = (long)0;
	guildName = "";
	simpleName = "";
	buildingAddPerArr = new List<int>();
}

public Offline_PlayerJoinGuild(
	long _newGuildId
	, string _guildName
	, string _simpleName
	, List<int> _buildingAddPerArr
) {	newGuildId = _newGuildId;
	guildName = _guildName;
	simpleName = _simpleName;
	buildingAddPerArr = _buildingAddPerArr;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getNewGuildId() { return newGuildId; }
public void setNewGuildId(long _newGuildId) { newGuildId = _newGuildId; }
public string getGuildName() { return guildName; }
public void setGuildName(string _guildName) { guildName = _guildName; }
public string getSimpleName() { return simpleName; }
public void setSimpleName(string _simpleName) { simpleName = _simpleName; }
public List<int> getBuildingAddPerArr() { return buildingAddPerArr; }
public void addBuildingAddPerArr(int _buildingAddPerArr) { buildingAddPerArr.Add(_buildingAddPerArr); }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += 2 + (buildingAddPerArr.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += 2 + (buildingAddPerArr.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	newGuildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guildName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	simpleName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _buildingAddPerArrCount = _buf.getShort();
	for(int _i = 0; _i < _buildingAddPerArrCount; _i++) { 
		int _buildingAddPerArr = 0;
		_buildingAddPerArr = _buf.getInt();
		buildingAddPerArr.Add(_buildingAddPerArr);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(newGuildId);
	_buf.putString(guildName);
	_buf.putString(simpleName);
	_buf.putShort((short)buildingAddPerArr.Count);
	for(int _i = 0; _i < buildingAddPerArr.Count; _i++) { 
		_buf.putInt(buildingAddPerArr[_i]);
	}
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
	builder.Append("newGuildId").Append(":").Append(newGuildId.ToString()).Append(", ");
	builder.Append("guildName").Append(":").Append(guildName.ToString()).Append(", ");
	builder.Append("simpleName").Append(":").Append(simpleName.ToString()).Append(", ");
	builder.Append("buildingAddPerArr").Append(":").Append(buildingAddPerArr.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

