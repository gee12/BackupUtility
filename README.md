# BackupUtility
Консольные утилиты для работы с бэкапами баз данных MS SQL Server.<BR>
Работают под .Net 2.0 и выше
<!--Console utilities for work with database backup's:-->

<!--#####################################################################################################################-->
<H4>SqlBackUpper.exe</H4>
<!--Executing SQL queries to databases for creating backup's.-->
Выполнение запросов к базам данных MS SQL Server для создания резервных копий
<OL>
  <LI>Работа с СУБД <B>MS SQL Server</B>
  <LI>Работа с конфигурационным <B>.xml</B> файлом. По-умолчанию ищется в рабочем каталоге с именем приложения и расширением ".cfg". Также полное имя конфигурационного файла можно задать в качестве аргумента. При отсутствии конфиг.файла - работа программы прекращается. Формат описания:<BR>
  
  <PRE class="font-weight: bold;">
    &lt;Config [Атрибуты]>
     &lt;Folder [Атрибуты]> 
     &lt;Folder [Атрибуты]>
      ...
    &lt;/Config>
  </PRE>
    	
Корневой тег <B>Config</B> - задание основных настроек. Атрибуты:
<UL>
  <LI><B>BackupNameMask</B> (необязательный) - маска для формирования имени создаваемого бэкапа. Обязательно должна содержать местозаполнитель с индексом 0 ("{0}") для вставки текущей даты и времени. Значение по-умолчанию -  "_{0:dd-MM-yyyy_HH-mm-ss}.bak"
  <LI><B>ConnectionMask</B> (необязательный) - маска для формирования строки подключения к определенному серверу и базе данных. Обязательно должна содержать местозаполнители с индексами 0-6 для вставки параметров подключения. Значение по-умолчанию - "Data Source={0};Initial Catalog={1};User ID={2};Password={3};Integrated Security={4};Persist Security Info={5};Trusted_Connection={6};"
  <LI><B>ConnectionGroupMask</B> (необязательный) - маска для формирования строки подключения к определенному серверу (используется, если UniteSameInst=True). Обязательно должна содержать местозаполнители с индексами 0-5 для вставки параметров подключения. Значение по-умолчанию - "Data Source={0};User ID={1};Password={2};Integrated Security={3};Persist Security Info={4};Trusted_Connection={5};"
  <LI><B>SqlQueryMask</B> (необязательный) - маска для формирования строки запроса к БД на создание бэкапа. Обязательно должна содержать местозаполнители с индексами 0-3 для вставки имени БД (BaseName), пути к хранищу (BackupPath) и имени файла бэкапа соответственно (BackupNameMask). Значение по-умолчанию - "BACKUP DATABASE [{0}] TO  DISK = N'{1}\\{2}' WITH NOFORMAT, INIT,  NAME = N'{3} - Full backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
  <LI><B>Timeout</B> (необязательный) - максимальное время в секундах, выделенное на выполнение запроса. Значение по-умолчанию - "30"
  <LI><B>UniteSameInst</B> (необязательный) - переключатель, необходимо ли группировать запросы к одинаковым серверам. Значение по-умолчанию - False
  <LI><B>LogPath</B> (необязательный) - путь для создания log-файла. По-умолчанию - ".\" (путь к рабочему каталогу)<BR>
  <LI><B>MaxLogs</B> (необязательный) - максимальное количество хранимых на диске (по указанному пути в LogPath) log-файлов. Удаляются лишние - наиболее старые. Поиск и удаление происходит по маске "*.log". Значение по-умолчанию - '5'.<BR>
  <LI><B>WindowStyle</B> (необязательный) - стиль отображения консольных окон приложения и 7-zip. Возможные значения: Hidden (по-умолчанию), Normal, Maximized, Minimized<BR>
  <LI><B>ReadKeyInFinish</B> (необязательный) - переключатель того, необходимо ли ожидать от пользователя нажатия любой клавиши клавиатуры перед завершением программы. Возможные значения: True (по-умолчанию), False. Действует только при WindowStyle = Normal | Maximized<BR>
</UL>
<BR>
Тег <B>Connection</B> - задание экземпляра соединения с базой данных и выполнения запроса на создание бэкапа. Могут задаваться несколько тегов Connection. Атрибуты:
<UL>
  <LI><B>InstName</B> (обязательный) - имя целевого сервера<BR>
  <LI><B>BaseName</B> (обязательный) - имя целевой БД<BR>
  <LI><B>UserName</B> (обязательный) - имя учетной записи для подключения к серверу<BR>
  <LI><B>Password</B> (обязательный) - пароль<BR>
  <LI><B>BackupPath</B> (необязательный) - путь к хранилищу - каталогу назначения. Если заданный путь отсутствует, программа создает его автоматически. Значение по-умолчанию - ".\" (путь к рабочему каталогу)<BR>
  <LI><B>MaxBackups</B> (необязательный) - максимальное количество хранимых на диске (по указанному пути в BackupPath) бэкапов. Удаляются лишние - наиболее старые. Поиск и удаление происходит по маске "&lt;BaseName>*". Значение по-умолчанию - '5'.<BR>
</UL>
<P><B>Примечание:</B> Имя создаваемого файла бэкапа для каждого экземпляра Connection формируется как: BaseName + BackupNameMask</P>

<B>Пример:</B>
  <PRE>
    &lt;Config
      BackupNameMask="_{0:dd-MM-yyyy_HH-mm-ss}.bak" 
      SqlQueryMask="BACKUP DATABASE [{0}] TO  DISK = N'{1}\\{2}' WITH NOFORMAT, INIT,  NAME = N'{3} - Full backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
      Timeout="30"
      UniteSameInst="true"
      LogPath=".\" 
      MaxLogs="3" 
      WindowStyle="Normal" 
      ReadKeyInFinish="true" >
         &lt;Connection InstName="(localdb)\V11.0" BaseName="master" UserName="" Password="" BackupPath="D:\Backup1\" MaxBackups="3"/>
         &lt;Connection InstName=".\SQLExpress" BaseName="model" UserName="gee12-PC\gee12" Password="" BackupPath="D:\Backup2\" MaxBackups="5"/>
    &lt;/Config>
  </PRE>
  
  <LI>Для работы необходима библиотека <B>BackupLibrary.dll</B><BR>
</OL>

<!--#####################################################################################################################-->
<H4>BackupArchiver.exe</H4>
Запуск 7-zip с необходимыми аргументами для архивирования резервных копий БД (или других файлов в целевом каталоге) в хранилище
<!--Archiving files (databases backup's) to some destination folder (using 7-zip).-->
<OL>
  <LI>Работа с архиватором <B>7-zip</B> (необходимо наличие установленного 7-zip)<BR>
  <LI>Работа с конфигурационным <B>.xml</B> файлом (см.пункт 1 в SqlBackUpper.exe). Формат описания:<BR>
  
  <PRE class="font-weight: bold;">
    &lt;Config [Атрибуты]>
     &lt;Folder [Атрибуты]> 
     &lt;Folder [Атрибуты]>
      ...
    &lt;/Config>
  </PRE>
    	
Корневой тег <B>Config</B> - задание основных настроек. Атрибуты:
<UL>
  <LI><B>ZipPath</B> (необязательный) - путь к каталогу с 7-zip. Небходимо наличие 7z.exe, 7z.dll и 7-zip.dll. По-умолчанию ищет в рабочем каталоге, 'C:\Program Files\' или в 'C:\Program Files (x86)\'<BR>
  <LI><B>ZipArgs</B> (необязательный) - аргументы для 7-zip. Значение по-умолчанию - "a -tzip -y"<BR>
  <LI><B>TailMask</B> (необязательный) - маска для формирования "хвостовой" части имени создаваемого архива. Обязательно должна содержать местозаполнитель с индексом 0 ("{0}") для вставки текущей даты и времени. Значение по-умолчанию - "{0:dd-MM-yyyy_HH-mm-ss}.zip"<BR>
  <LI><B>LogPath</B> (необязательный) - см.пункт 1 в SqlBackUpper.exe<BR>
  <LI><B>MaxLogs</B> (необязательный) - см.пункт 1 в SqlBackUpper.exe<BR>
  <LI><B>WindowStyle</B> (необязательный) - см.пункт 1 в SqlBackUpper.exe<BR>
  <LI><B>ReadKeyInFinish</B> (необязательный) - см.пункт 1 в SqlBackUpper.exe<BR>
</UL>
<BR>
Тег <B>Folder</B> - задание целевого каталога и хранилища. Могут задаваться несколько тегов Folder. Атрибуты:
<UL>
  <LI><B>SourcePath</B> (обязательный) - путь к целевому каталогу. Если заданный путь отсутствует, текущий Folder не обрабатывается<BR>
  <LI><B>DestPath</B> (необязательный) - путь к хранилищу - каталогу назначения. Если заданный путь отсутствует, 7-zip создаст его автоматически. Значение по-умолчанию - ".\" (путь к рабочему каталогу)<BR>
  <LI><B>HeadMask</B> (необязательный) - маска для формирования "головной" части имени создаваемого архива. Значение по-умолчанию - "Backup"<BR>
  <LI><B>MaxArchives</B> (необязательный) - максимальное количество хранимых на диске (по указанному пути в DestPath) архивов. Удаляются лишние - наиболее старые. Поиск и удаление происходит по маске "&lt;HeadMask>*". Значение по-умолчанию - '5'.<BR>
</UL>
<P><B>Примечание:</B> Имя создаваемого файла архива для каждого экземпляра Folder формируется как: HeadMask + TailMask</P>

<B>Пример:</B>
  <PRE>
    &lt;Config 
      ZipPath="C:\Program Files\7-Zip\" 
      TailMask="{0:dd-MM-yyyy_HH-mm-ss}.zip" 
      ZipArgs="a -tzip -y" 
      LogPath="D:\" 
      MaxLogs="3" 
      WindowStyle="Normal" 
      ReadKeyInFinish="true">
            &lt;Folder HeadMask="backup1_" 	 SourcePath="C:\Backup1" 	DestPath="D:\Archive1" MaxArchives="3" />
            &lt;Folder HeadMask="backup2_" 	 SourcePath="D:\Backup2" 	DestPath="D:\Archive2" MaxArchives="10" />
    &lt;/Config>
  </PRE>
  
  <LI>Для работы необходима библиотека <B>BackupLibrary.dll</B><BR>
</OL>

