using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_SeasonRankInfo : ALBasicProtocolPack._IALProtocolStructure {
private int seaonId;
private int rank;
private int grade;
private int star;
private int legendScore;


public WCGGS2GC_SeasonRankInfo() {
	seaonId = 0;
	rank = 0;
	grade = 0;
	star = 0;
	legendScore = 0;
}

public WCGGS2GC_SeasonRankInfo(
	int _seaonId
	, int _rank
	, int _grade
	, int _star
	, int _legendScore
) {	seaonId = _seaonId;
	rank = _rank;
	grade = _grade;
	star = _star;
	legendScore = _legendScore;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public int getSeaonId() { return seaonId; }
public void setSeaonId(int _seaonId) { seaonId = _seaonId; }
public int getRank() { return rank; }
public void setRank(int _rank) { rank = _rank; }
public int getGrade() { return grade; }
public void setGrade(int _grade) { grade = _grade; }
public int getStar() { return star; }
public void setStar(int _star) { star = _star; }
public int getLegendScore() { return legendScore; }
public void setLegendScore(int _legendScore) { legendScore = _legendScore; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	seaonId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rank = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	grade = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	star = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	legendScore = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(seaonId);
	_buf.putInt(rank);
	_buf.putInt(grade);
	_buf.putInt(star);
	_buf.putInt(legendScore);
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
	builder.Append("seaonId").Append(":").Append(seaonId.ToString()).Append(", ");
	builder.Append("rank").Append(":").Append(rank.ToString()).Append(", ");
	builder.Append("grade").Append(":").Append(grade.ToString()).Append(", ");
	builder.Append("star").Append(":").Append(star.ToString()).Append(", ");
	builder.Append("legendScore").Append(":").Append(legendScore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

