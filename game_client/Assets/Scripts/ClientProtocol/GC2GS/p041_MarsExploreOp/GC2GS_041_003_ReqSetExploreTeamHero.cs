using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p041_MarsExploreOp
{

/// <summary>
/// 火星探险-设置队伍大臣
/// </summary>
public class GC2GS_041_003_ReqSetExploreTeamHero : ALBasicProtocolPack._IALProtocolStructure {
private long teamId;
private List<long> heroIdList;


public GC2GS_041_003_ReqSetExploreTeamHero() {
	teamId = (long)0;
	heroIdList = new List<long>();
}

public GC2GS_041_003_ReqSetExploreTeamHero(
	long _teamId
	, List<long> _heroIdList
) {	teamId = _teamId;
	heroIdList = _heroIdList;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)3; }

public long getTeamId() { return teamId; }
public void setTeamId(long _teamId) { teamId = _teamId; }
public List<long> getHeroIdList() { return heroIdList; }
public void addHeroIdList(long _heroIdList) { heroIdList.Add(_heroIdList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (heroIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (heroIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroIdListCount = _buf.getShort();
	for(int _i = 0; _i < _heroIdListCount; _i++) { 
		long _heroIdList = (long)0;
		_heroIdList = _buf.getLong();
		heroIdList.Add(_heroIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(teamId);
	_buf.putShort((short)heroIdList.Count);
	for(int _i = 0; _i < heroIdList.Count; _i++) { 
		_buf.putLong(heroIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)3);
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
	builder.Append("teamId").Append(":").Append(teamId.ToString()).Append(", ");
	builder.Append("heroIdList").Append(":").Append(heroIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

