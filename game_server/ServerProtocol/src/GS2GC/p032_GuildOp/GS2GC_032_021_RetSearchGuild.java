package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_021_RetSearchGuild implements ALBasicProtocolPack._IALProtocolStructure {
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

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)21; }

public Common.GuildObj.Guild_ShowInfo getShowInfo() { return showInfo; }
public void setShowInfo(Common.GuildObj.Guild_ShowInfo _showInfo) { showInfo = _showInfo; }
public Common.RankObj.Rank_BaseItem getRankBaseInfo() { return rankBaseInfo; }
public void setRankBaseInfo(Common.RankObj.Rank_BaseItem _rankBaseInfo) { rankBaseInfo = _rankBaseInfo; }


public final int GetBufSize() {
	int _size = 32;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + showInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _showInfoCustLen = _buf.getInt();
	int _showInfoCurPos = _buf.position();
	showInfo.ReadUnzipBuf(_buf, _showInfoCurPos + _showInfoCustLen);
	_buf.position(_showInfoCurPos + _showInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _rankBaseInfoCustLen = _buf.getInt();
	int _rankBaseInfoCurPos = _buf.position();
	rankBaseInfo.ReadUnzipBuf(_buf, _rankBaseInfoCurPos + _rankBaseInfoCustLen);
	_buf.position(_rankBaseInfoCurPos + _rankBaseInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showInfo.GetBufSize());
	showInfo.PutUnzipBuf(_buf);
	_buf.putInt(rankBaseInfo.GetBufSize());
	rankBaseInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)21);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

