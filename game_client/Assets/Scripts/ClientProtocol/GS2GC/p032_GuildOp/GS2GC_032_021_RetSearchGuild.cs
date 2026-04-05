using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

public class GS2GC_032_021_RetSearchGuild : ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_ShowInfo showInfo;
private Common.RankObj.Rank_BaseItem rankBaseInfo;


public GS2GC_032_021_RetSearchGuild() {
	showInfo = new Common.GuildObj.Guild_ShowInfo();
	rankBaseInfo = new Common.RankObj.Rank_BaseItem();
}

public GS2GC_032_021_RetSearchGuild(
	Common.GuildObj.Guild_ShowInfo _showInfo
	, Common.RankObj.Rank_BaseItem _rankBaseInfo
) {	showInfo = _showInfo;
	rankBaseInfo = _rankBaseInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)21; }

public Common.GuildObj.Guild_ShowInfo getShowInfo() { return showInfo; }
public void setShowInfo(Common.GuildObj.Guild_ShowInfo _showInfo) { showInfo = _showInfo; }
public Common.RankObj.Rank_BaseItem getRankBaseInfo() { return rankBaseInfo; }
public void setRankBaseInfo(Common.RankObj.Rank_BaseItem _rankBaseInfo) { rankBaseInfo = _rankBaseInfo; }


public int GetBufSize() {
	int _size = 32;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.getCurPos();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.setPosition(_showInfoCurPos + _showInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _rankBaseInfoCustLen = _buf.getInt();
	int _rankBaseInfoCurPos = _buf.getCurPos();
	rankBaseInfo.ReadUnzipBuf(_buf, _rankBaseInfoCurPos + _rankBaseInfoCustLen);
	_buf.setPosition(_rankBaseInfoCurPos + _rankBaseInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
	_buf.putInt(rankBaseInfo.GetBufSize());
	rankBaseInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)21);
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
	builder.Append("showInfo").Append(":").Append(showInfo == null ? "null" : showInfo.ToString()).Append(", ");
	builder.Append("rankBaseInfo").Append(":").Append(rankBaseInfo == null ? "null" : rankBaseInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

