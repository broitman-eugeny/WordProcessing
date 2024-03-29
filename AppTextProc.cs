using System;//Здесь содержится набор основных классов, и мы здесь создаем свой класс
using System.Windows.Forms;

//Класс функции Main() приложения обработки текста
//Программа принимает на вход файл на диске (либо несколько файлов). Обработ-ка файла заключается в следующем:
//a. удаление слов длиной менее какого-либо числа символов;
//b. удаление знаков препинания.
//Результат сохраняется в выходной файл (либо файлы).
//Входной файл, выходной файл, длину слов и необходимость удаления знаков препинания задает пользователь.

class AppTextProc : FormTextProc
{
    // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
    [STAThread]//Однопоточное приложение
    public static void Main()
    {
        Application.Run(new FormTextProc());
    }
}
