//Пространства имен
using System;//Здесь содержится набор основных классов, и мы здесь создаем свой класс
using System.IO;
using System.Text;
using System.Windows.Forms;

//Класс обработки текста
//Содержит функции выбора и открытия входных файлов, функции обработки входных файлов и сохранения выходных файлов
public class TextProcessor
{
    public static void OpenProcSaveFiles(int MinNumOfLetters, CheckState DelPunctuationMarks)
	{
        OpenFileDialog ofd = new OpenFileDialog();//Создание объекта стандартного диалога открытия файла 
        ofd.Title = "Входной текст";//Заголовок диалога открытия файла
        //ofd.Filter = "All files (*.*)|*.*|Text (*.txt)|*.txt" ;//Фильтры типов файлов
        ofd.Multiselect = true;//Допускается выбор нескольких файлов
        if (ofd.ShowDialog() == DialogResult.OK)//Показать диалог, и если файлы выбраны
        {
            StringBuilder[] tempOutFileNames;//Массив путей к сохраненным временным выходным файлам
            StringBuilder[] OFDFileNames = new StringBuilder[ofd.FileNames.Length];//Массив строк имен выбранных входных файлов
            for (int i = 0; i < ofd.FileNames.Length; i++)
            {
                OFDFileNames[i]=new StringBuilder(ofd.FileNames[i]);
            }
            tempOutFileNames = processText(OFDFileNames, MinNumOfLetters, DelPunctuationMarks);	//Обработка входных файлов и сохранение результата во
            //временные файлы с именами "..._temp"
            SaveFileDialog sfd = new SaveFileDialog();//Создание объекта стандартного диалога сохранения файла
            // Сохранение результата обработки текстовых файлов
            for (int i = 0; i < ofd.SafeFileNames.Length; i++)//По всем именам входных файлов не включая путей
            {
                //Заголовок диалога сохранения файла
                sfd.Title = "Сохранить результат обработки файла " + ofd.SafeFileNames[i];
                sfd.FileName = "_"+ofd.SafeFileNames[i];
                if (sfd.ShowDialog() == DialogResult.OK)//Показать диалог, и если имена файлов выбраны
                {
                    //Копирует временный файл в заданный пользователем
                    File.Copy(tempOutFileNames[i].ToString(), sfd.FileName, true);
                }
            }
            //Удаляем временные файлы
            for (int i = 0; i < tempOutFileNames.Length; i++)//По всем именам временных файлов
            {
                File.Delete(tempOutFileNames[i].ToString());
            }
        }
	}

    //Функция обработки и сохранения текста
    public static StringBuilder[] processText(StringBuilder[] inFileNames, int minChars, CheckState DelPunctuationMarks)
    {
        Encoding enc = Encoding.GetEncoding(1251);
        //Проверка необходимости удаления знаков препинания
        bool delPunct = (DelPunctuationMarks == CheckState.Checked) ? true : false;
        FileStream fsIn;//Поток очередного входного файла
        StreamReader r;//Объект для чтения входного потока
        StringBuilder[] tempOutFileNames = new StringBuilder[inFileNames.Length];
        FileStream fsOut;//Поток очередного выходного файла
        StreamWriter w;//Объект для записи выходного потока
        StringBuilder CurWord;//Текущее слово, читаемое из входного файла
        int charsInCurWord;//Зафиксированное текущее количество букв и цифр в текущем слове
        char c;//прочитанный из текущего входного файла текущий символ
        CurWord = new StringBuilder("");
        charsInCurWord = 0;
        for (int i = 0; i < inFileNames.Length; i++)//По всем именам входных файлов
        {
            fsIn = new FileStream(inFileNames[i].ToString(), FileMode.Open, FileAccess.Read);//Поток очередного входного файла
            using (r = new StreamReader(fsIn, enc))//Использование конструкции using, чтобы освободить неуправляемые ресурсы в случае возникновения исключения
            {
                tempOutFileNames[i] = inFileNames[i].Append("_temp");
                fsOut = new FileStream(tempOutFileNames[i].ToString(), FileMode.OpenOrCreate, FileAccess.Write);
                using (w = new StreamWriter(fsOut, enc))//Использование конструкции using, чтобы освободить неуправляемые ресурсы в случае возникновения исключения
                {
                    while (r.Peek() >= 0)//пока не конец входного файла (-1)
                    {
                        c = (char)r.Read();
                        if (!(Char.IsPunctuation(c) || Char.IsSeparator(c) || Char.IsControl(c)))//если символ - часть слова
                        {
                            CurWord.Append(c.ToString());
                            charsInCurWord++;
                        }
                        else//если знак препинания, разделитель или управляющий символ
                        {
                            if (charsInCurWord >= minChars)
                            {
                                w.Write(CurWord);//Запись считанного длинного слова в выходной файл
                            }
                            //либо знаки препинания не удаляются, либо считанный символ - не знак препинания
                            if (!(Char.IsPunctuation(c) && delPunct))
                            {
                                w.Write(c);//Запись считанного символа в выходной файл				
                            }
                            //Подготовка к чтению следующего слова
                            CurWord.Clear();
                            charsInCurWord = 0;
                        }
                    }
                    //Запись последнего длинного считанного слова
                    if (charsInCurWord >= minChars)
                    {
                        w.Write(CurWord);
                    }
                    //Подготовка к чтению следующего файла
                    CurWord.Clear();
                    charsInCurWord = 0;
                    w.Flush();//Очищает буферы и записывает их в устройство
                    w.Close();
                    fsOut.Close();
                    r.Close();
                    fsIn.Close();
                }
            }
        }
        return tempOutFileNames;
    }
}
