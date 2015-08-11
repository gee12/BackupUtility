# BackupUtility
Console utilities for work with database backup's:

<H4>SqlBackUpper.exe</H4>
Executing SQL queries to databases for creating backup's.
<OL>
  <LI> работа с архиватором 7-zip<BR>
  <LI> работа с конфигурационным xml файлом. Формат:<BR>
  
  <PRE>
  &lt;Config [Аттрибуты]>
   &lt;Folder [Аттрибуты]> 
   &lt;Folder [Аттрибуты]>
    ...
  &lt;/Config>
  </PRE>
    			        
</BR>
Крневй тег Config - задание основных настроек. Аттрибуты:<BR>
Тег Folder - задание каталога для обработки (можно задавать несколько каталогов). Аттрибуты:<BR>
  
По-умолчанию конфигурационный файл ищется в рабочем каталоге с именем приложения и расширением ".cfg". Также путь к файлу можно задать в качестве аргумента.<br>
  <LI><br>
  <LI><br>
</OL>
<H4>BackupArchiver</H4> - archiving files (databases backup's) to some destination folder (using 7-zip).

Programms use .xml configuration file to specify settings.
