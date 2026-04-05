# coding=utf-8
import gen
def genActivityAll():

	#ActivitiesV01
	gen.genAll("ActivitiesV01", r"../ActivitiesV01/src/ActivitiesV01/Bo","ActivitiesV01.Bo","BO.txt")
	gen.genAll("ActivitiesV01Log", r"../ActivitiesV01/src/ActivitiesV01/LogBo","ActivitiesV01.LogBo","LOGBO.txt")

	#ActivitiesV02
	gen.genAll("ActivitiesV02", r"../ActivitiesV02/src/ActivitiesV02/Bo","ActivitiesV02.Bo","BO.txt")
	gen.genAll("ActivitiesV02Log", r"../ActivitiesV02/src/ActivitiesV02/LogBo","ActivitiesV02.LogBo","LOGBO.txt")

if __name__ == "__main__":
	genActivityAll()
	pass
