
rmdir /S/Q ..\ActivitiesV01\src\Hotfix\V01
mkdir ..\ActivitiesV01\src\Hotfix\V01
xcopy ..\ServerProtocol\src\Hotfix\V01\*  ..\ActivitiesV01\src\Hotfix\V01 /S/R/Q/Y
rmdir /S/Q ..\ServerProtocol\src\Hotfix\V01

rmdir /S/Q ..\ActivitiesV02\src\Hotfix\V02
mkdir ..\ActivitiesV02\src\Hotfix\V02
xcopy ..\ServerProtocol\src\Hotfix\V02\*  ..\ActivitiesV02\src\Hotfix\V02 /S/R/Q/Y
rmdir /S/Q ..\ServerProtocol\src\Hotfix\V02
