# coding=utf-8
import gen
import genHotActivitys

if __name__ == "__main__":

	#UserServer
	gen.genAll("UserServer", r"../UserServer/src/USDB/Bo","USDB.Bo","BO.txt")
	gen.genAll("UserServerLog", r"../UserServer/src/USLOGDB/Bo","USLOGDB.Bo","LOGBO.txt")
	gen.genAll("UserServerOptLog", r"../UserServer/src/USLOGDB/OptBo","USLOGDB.OptBo","OptLogBO.txt")
	#MJLog
	gen.genAll("MJLog", r"../UserServer/src/MJLog/Bo","MJLog.Bo","BO.txt")
	gen.genAll("MJEventLog", r"../UserServer/src/MJLog/EventBo","MJLog.EventBo","MJLogBO.txt")
	#活动数据库
	genHotActivitys.genActivityAll() 

	#ScheduleServer
	gen.genAll("ScheduleServer", r"../ScheduleServer/src/SSDB/Bo","SSDB.Bo","BO.txt")
	gen.genAll("ScheduleServerLog", r"../ScheduleServer/src/SSLOGDB/Bo","SSLOGDB.Bo","LOGBO.txt")

	#RecordServer
	gen.genAll("RecordServer", r"../RecordServer/src/RCSDB/Bo","RCSDB.Bo","BO.txt")

	#LogincheckServer
	gen.genAll("LoginCheckServer", r"../LoginCheckServer/src/LCSDB/Bo","LCSDB.Bo","BO.txt")
	
	#InterfaceServer
	gen.genAll("InterfaceServer", r"../InterfaceServer/src/ISDB/Bo","ISDB.Bo","BO.txt")

	#HttpServer
	gen.genAll("HttpServer", r"../HttpServer/src/HSDB/Bo","HSDB.Bo","BO.txt")
	gen.genAll("HttpServerLog", r"../HttpServer/src/HSLOGDB/Bo","HSLOGDB.Bo","LOGBO.txt")

	#CommonServer
	gen.genAll("CommonServer", r"../CommonServer/src/CSDB/Bo","CSDB.Bo","BO.txt")

	#CrossRankServer
	gen.genAll("CrossRankServer", r"../CrossRankServer/src/CRSDB/Bo","CRSDB.Bo","BO.txt")
	gen.genAll("CrossRankServerLog", r"../CrossRankServer/src/CRSLOGDB/Bo","CRSLOGDB.Bo","LOGBO.txt")

	#CrossGameServer
	gen.genAll("CrossGameServer", r"../CrossGameServer/src/CGSDB/Bo","CGSDB.Bo","BO.txt")
	gen.genAll("CrossGameServerLog", r"../CrossGameServer/src/CGSLOGDB/Bo","CGSLOGDB.Bo","LOGBO.txt")

	#ShareCodeCenter
	gen.genAll("ShareCodeCenter", r"../ShareCodeCenter/src/ShareCodeDB/Bo","ShareCodeDB.Bo","BO.txt")

	#PayCenter
	gen.genAll("PayCenter", r"../PayCenter/src/PayDB/Bo","PayDB.Bo","BO.txt")
    
	#CrossTeamServer
	gen.genAll("CrossTeamServer", r"../CrossTeamServer/src/CTSDB/Bo","CTSDB.Bo","BO.txt")
	gen.genAll("CrossTeamServerLog", r"../CrossTeamServer/src/CTSLOGDB/Bo","CTSLOGDB.Bo","LOGBO.txt")
    
	#GameLogicServer
	gen.genAll("GameLogicServer", r"../GameLogicServer/src/GLSDB/Bo","GLSDB.Bo","BO.txt")
	gen.genAll("GameLogicServerLog", r"../GameLogicServer/src/GLSLOGDB/Bo","GLSLOGDB.Bo","LOGBO.txt")