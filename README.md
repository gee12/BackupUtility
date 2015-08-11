# BackupUtility
Console utilities for work with database backup's:

SqlBackUpper - executing SQL queries to databases for creating backup's.
1) работа с архиватором 7-zip
2) работа с конфигурационным xml файлом. Формат:
"<Config [Аттрибуты]>
  <Folder [Аттрибуты]>
  <Folder [Аттрибуты]>
  ...
</Config>"
Аттрибуты корневого тега Config:
  
Путь можно задать в качестве аргумента. По-умолчанию ищет в рабочем каталоге с именем приложения и расширением ".cfg"
3) 
4)

BackupArchiver - archiving files (databases backup's) to some destination folder (using 7-zip).

Programms use .xml configuration file to specify settings.
