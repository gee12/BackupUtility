using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SBUConfigurator
{
    public class MessageBoxes
    {
        public const string TitleError = "Ошибка";
        public const string TitleWarning = "Предупреждение!";
        public const string TitleExclamation = "Внимание!";
        public const string TitleNotify = "Информация";
        public const string TitleSuccess = "Операция выполнена успешно";
        public const string TitleNotSuccess = "Операция завершилась неудачей";
        public const string TitleServerConn = "Подключение к серверу ...";
        public const string TitleServerConnClose = "Отключение от сервера ...";
        public const string TitleServerTest = "Тестирование соединения с сервером ...";

        public const string MsgCloseRequest = "Прекратить работу с программой?";
        public const string MsgPassNotSame = "Введенные пароли не совпадают!";
        public const string MsgWrongPass = "Неверный пароль!";
        public const string MsgWrongOldPass = "Неверный предыдущий пароль!";
        public const string MsgConIsActive = "База данных [{0}]  на сервере [{1}] уже подключена. Все равно разорвать текущее подключение?";
        public const string MsgServerNotAviable = "Сервер {0} недоступен!";
        public const string MsgServerConnSuccess = "Соединение с сервером {0} выполнено успешно!";
//        public const string MsgConnConfigLoad = "Загрузка параметров соединения с сервером";
        public const string MsgErrGetUserInfo = "Ошибка получения информации о пользователе!";
        public const string MsgEmptyField = "Значение <{0}> не заполнено!";
        public const string MsgCatDeleteRequest = "Выбран элемент с ID: [{0}]\nУдалить элемент из базы данных?";
        public const string MsgCatDeleteRequestSimple = "Удалить выбранный элемент?";
        public const string MsgCatDeleteAllRequestSimple = "Удалить все выбранные элементы из базы данных?";
        public const string MsgCatCascadeDeleteRequest = "Элемент с ID: [{0}] содержится в записях других таблиц:\n\n{1}\n\nВыполнить каскадное удаление элемента вместе с зависимыми записями?";
        public const string MsgCatCascadeDeleteMessage = "Элемент с ID: [{0}] содержится в записях других таблиц:\n\n{1}\n\nСначала удалите все зависимые записи.";
        public const string MsgNotUserDefined = "Пользователь не указан!";
        public const string MsgUserChangePass = "Изменение пароля для пользователя <{0}>";
//        public const string MsgAddrTableIsEmpty = "Таблица адресов не заполнена";
//        public const string MsgDeleteRoutePoint = "Удалить точку маршрута <{0} <==> {1}>?";
        public const string MsgErrorOperation = "Ошибка при выполнении операции:\n{0}";
        public const string MsgLogOut = "Выйти из учетной записи \"{0}\"?";
        public const string MsgCloseTicket = "Подтвердите закрытие заявки.\nПользователи, не обладающие правами Администратора, не смогут редактировать закрытые заявки";
        public const string MsgCloseTicketWithSetStatus = "Подтвердите закрытие заявки.\nСтатус автоматически установится на [{0}]."
            + "\nПользователи, не обладающие правами Администратора, не смогут редактировать закрытые заявки";
        public const string MsgDeleteCurrentUser = "Учетная запись с ID: [{0}] является текущей. Удаление невозможно.";
        public const string MsgRecordAlreadyExists = "Запись уже существует";
        public const string MsgClearLogs = "Удалить все логи?";
        //public const string MsgUpdateTabels = "Создать табеля для выбранных исполнителей на дату поступления заявки?";
        public const string MsgUpdateTabels = "Была изменена дата поступления заявки и/или список исполнителей.\nОбновить табеля для выбранных исполнителей на дату поступления заявки?";
        public const string MsgUpdateTabels2 = "Обновить табеля для выбранных исполнителей на дату поступления заявки?";
        public const string MsgDateChanged = "Была изменена дата поступления заявки.\n";
        public const string MsgPerformersChanged = "Был изменен список исполнителей.\n";
        public const string MsgDateAndPerformersChanged = "Была изменена дата поступления заявки и список исполнителей.\n";
        public const string MsgDelTabelsFromDelPerformers = "Удалить текущую заявку из табелей сотрудников, которые были убраны из списка исполнителей?";
        public const string MsgLoadUpdates = "Новая версия программы [{0}] готова к загрузке.\nНажмите Да, чтобы загрузить и установить новую версию."
            + " При этом программа будут перезапущена. Обновить?";
        public const string MsgMissingUpdates = "Обновления отсутствуют.\nУстановлена актуальная версия программы [{0}]";

        public const string MsgConfigNotSet = "Не задан концигурационный файл. Создать новый?";

        // OK dialogs
        public static short Success(Window owner, string message)
        {
            MessageBox.Show(owner, message, TitleSuccess,
                MessageBoxButton.OK, MessageBoxImage.Information);
            return ResultCodes.Success;
        }

        public static short Error(Window owner, string message)
        {
            MessageBox.Show(owner, String.Format(MsgErrorOperation, message), TitleError,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return ResultCodes.Error;
        }

        public static short Error(Window owner, Exception ex)
        {
            MessageBox.Show(owner, String.Format(MsgErrorOperation, ex.Message), TitleError,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return ResultCodes.Error;
        }

        public static short Warning(Window owner, string message)
        {
            MessageBox.Show(owner, message, TitleExclamation,
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return ResultCodes.Exclamation;
        }

        public static short ServerNotAvailable(Window owner, string serverName)
        {
            MessageBox.Show(owner, String.Format(MsgServerNotAviable, serverName), TitleError,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return ResultCodes.Error;
        }

        public static short WrongPassword(Window owner)
        {
            MessageBox.Show(owner, MsgWrongPass, TitleError,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return ResultCodes.WrongPassword;
        }

        public static short WrongOldPassword(Window owner)
        {
            MessageBox.Show(owner, MsgWrongOldPass, TitleWarning,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return ResultCodes.WrongPassword;
        }

        public static short PassNotSame(Window owner)
        {
            MessageBox.Show(owner, MsgPassNotSame, TitleNotify,
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
            return ResultCodes.PassNotSame;
        }

        public static short EmptyField(Window owner, object field)
        {
            MessageBox.Show(owner, String.Format(MsgEmptyField, field), TitleNotify,
                MessageBoxButton.OK, MessageBoxImage.Question);
            return ResultCodes.EmptyField;
        }

        public static short ServerTest(Window owner, string serverName, bool isConnected)
        {
            MessageBox.Show(owner, string.Format((isConnected) ? MsgServerConnSuccess : MsgServerNotAviable,  serverName), TitleServerTest,
                MessageBoxButton.OK, MessageBoxImage.Information);
            return (isConnected) ? ResultCodes.Success : ResultCodes.NotSuccess;
        }

        public static short CascadeDelete(Window owner, int itemId, string[] dependents)
        {
            string dependentsString = string.Join("\n", dependents);
            MessageBox.Show(owner, string.Format(MsgCatCascadeDeleteMessage, itemId, dependentsString), TitleExclamation,
                MessageBoxButton.OK, MessageBoxImage.Stop);
            return ResultCodes.Success;
        }

        public static short DeletingCurrentUser(Window owner, int itemId)
        {
            return Warning(owner, String.Format(MsgDeleteCurrentUser, itemId));
        }

        public static short RecordAlreadyExists(Window owner)
        {
            Warning(owner, MsgRecordAlreadyExists);
            return ResultCodes.AlreadyExists;
        }

        // YesNo dialogs
        public static bool DeleteRequest(Window owner, int itemId)
        {
            return (MessageBox.Show(owner, string.Format(MsgCatDeleteRequest, itemId), TitleWarning, 
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes);
        }

        public static bool DeleteRequest(Window owner)
        {
            return (MessageBox.Show(owner, MsgCatDeleteRequestSimple, TitleWarning, 
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes);
        }

        public static bool DeleteAllRequest(Window owner)
        {
            return (MessageBox.Show(owner, MsgCatDeleteAllRequestSimple, TitleWarning,
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes);
        }

        public static bool CascadeDeleteRequest(Window owner, int itemId, string[] dependents)
        {
            string dependentsString = string.Join("\n", dependents);
            return (MessageBox.Show(owner, string.Format(MsgCatCascadeDeleteRequest, itemId, dependentsString), TitleExclamation,
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes);
        }

        public static bool LogOutRequest(Window owner, string login)
        {
            return (MessageBox.Show(owner, String.Format(MsgLogOut, login), TitleWarning,
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
        }

        public static bool CloseTicketRequest(Window owner)
        {
            return (MessageBox.Show(owner, MsgCloseTicket, TitleWarning,
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK);
        }

        public static bool CloseTicketRequestWithSetStatus(Window owner, string statusName)
        {
            return (MessageBox.Show(owner, String.Format(MsgCloseTicketWithSetStatus, statusName), TitleWarning,
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK);
        }

        public static bool ClearLogsRequest(Window owner)
        {
            return (MessageBox.Show(owner, MsgClearLogs, TitleWarning,
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK);
        }

        public static MessageBoxResult UpdateTabelsRequest(Window owner)
        {
            return (MessageBox.Show(owner, MsgUpdateTabels, "",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question));
        }
        public static MessageBoxResult UpdateTabelsRequest(Window owner, bool isChangePerformers, bool isChangeDate)
        {
            string s = (isChangePerformers && isChangeDate) ? MsgDateAndPerformersChanged :
                (isChangePerformers) ? MsgPerformersChanged : (isChangeDate) ? MsgDateChanged : "";
            s = string.Format("{0}{1}", s, MsgUpdateTabels2);

            return (MessageBox.Show(owner, s, "Обновление табелей",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question));
        }

        public static MessageBoxResult DelTabelsFromDelPerformersRequest(Window owner)
        {
            return (MessageBox.Show(owner, MsgDelTabelsFromDelPerformers, "",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question));
        }

        public static void MissingUpdates(Window owner, Version curVersion)
        {
            MessageBox.Show(owner, string.Format(MsgMissingUpdates, curVersion.ToString()), "Обновление",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool CreateListRequest(Window owner)
        {
            return (MessageBox.Show(owner, "Путевой лист на данное число и с водителем из данного списка исполнителей уже существует в базе.\n"
                + "Продолжить и все равно создать новый путевой лист?", TitleNotify,
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes);
        }

        public static bool CreateConfigFile()
        {
            return (MessageBox.Show(MsgConfigNotSet, TitleError,
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes);
        }

        public static bool CreateConfigFile(Window owner)
        {
            return (MessageBox.Show(owner, MsgConfigNotSet, TitleError,
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes);
        }

    }
}
