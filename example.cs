//Тестовое задание «Разработчик C#» 
//Задание №1
//Программа для обработки текста

//Пространства имен
using System;//Здесь содержится набор основных классов, и мы здесь создаем свой класс
using System.Windows.Forms;//Для работы с формами
using System.Drawing;//Доступ к основным функциям GDI
using System.IO;//Системный ввод вывод (работа с файлами)
using System.Text;//Для Encoding

class MyForm : AppForm 
{
    // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
    [STAThread]//Однопоточное приложение
    public static void Main()
	{
		Application.Run(new MyForm());
	}
}

class AppForm : Form
{
	private myTextBox tbMinNumOfLetters;//Элемент ввода минимального количества букв в словах
	private CheckBox cbPunctuationMarks;//Элемент выбора удаления знаков препинания	

	public AppForm()
	{
		Text="Программа обработки текста";//Заголовок окна программы

		//Меню пограммы
		MainMenu mnuFileMenu = new MainMenu();
		this.Menu = mnuFileMenu;
		MenuItem MenuItemFile = new MenuItem("&File");
		MenuItemFile.MenuItems.Add("&Open",new System.EventHandler(this.MenuOpen_Click));
		MenuItemFile.MenuItems.Add("E&xit",new System.EventHandler(this.MenuExit_Click));
		mnuFileMenu.MenuItems.Add(MenuItemFile);
		
		//Метка для элемента ввода минимального количества букв в словах
		Label labelMinNumOfLetters = new Label();
		labelMinNumOfLetters.Text = "Минимальное количество букв в словах:";
		labelMinNumOfLetters.Location = new Point(15,15);
		labelMinNumOfLetters.AutoSize=true;
		labelMinNumOfLetters.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelMinNumOfLetters);

		//Элемент ввода минимального количества букв в словах
		tbMinNumOfLetters=new myTextBox();
		tbMinNumOfLetters.Text = "3";
		tbMinNumOfLetters.Location=new Point(17+labelMinNumOfLetters.Width,15);
		tbMinNumOfLetters.Width=30;
		tbMinNumOfLetters.Height=labelMinNumOfLetters.Height;
		tbMinNumOfLetters.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbMinNumOfLetters);

		//Метка для элемента выбора удаления знаков препинания
		Label labelPunctuationMarks = new Label();
		labelPunctuationMarks.Text = "Удалить знаки препинания:";
		labelPunctuationMarks.Location = new Point(15,17+tbMinNumOfLetters.Height);
		labelPunctuationMarks.AutoSize=true;
		labelPunctuationMarks.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelPunctuationMarks);

		//Элемент выбора удаления знаков препинания
		cbPunctuationMarks=new CheckBox();
		cbPunctuationMarks.Location = new Point(17+labelPunctuationMarks.Width,17+tbMinNumOfLetters.Height);
		this.Controls.Add(cbPunctuationMarks);
	}

	//Получить минимальное количество букв в словах
	public int getMinNumOfLetters()
	{
		return Convert.ToInt32(tbMinNumOfLetters.Text);
	}

	//Получить признак выбора удаления знаков препинания
	public CheckState getDelPunctuationMarks()
	{
		return cbPunctuationMarks.CheckState;
	}
	
	//При выборе меню File\Open
	private void MenuOpen_Click(Object sender, EventArgs e)
	{
		OpenFileDialog ofd = new OpenFileDialog();//Создание объекта стандартного диалога открытия файла 
		ofd.Title = "Входной текст" ;//Заголовок диалога открытия файла
		//ofd.Filter = "All files (*.*)|*.*|Text (*.txt)|*.txt" ;//Фильтры типов файлов
		ofd.Multiselect=true;//Допускается выбор нескольких файлов
		if( ofd.ShowDialog() == DialogResult.OK)//Показать диалог, и если файлы выбраны
		{
			string[] tempOutFileNames;//Массив путей к сохраненным временным выходным файлам
			tempOutFileNames=processText(ofd.FileNames);	//Обработка входных файлов и сохранение результата во
								                            //временные файлы с именами "..._temp"
			SaveFileDialog sfd=new SaveFileDialog();//Создание объекта стандартного диалога сохранения файла
			// Сохранение результата обработки текстовых файлов
        	for(int i=0; i<ofd.SafeFileNames.Length; i++)//По всем именам входных файлов не включая путей
        	{
            	//Заголовок диалога сохранения файла
				sfd.Title = "Сохранить результат обработки файла "+ofd.SafeFileNames[i];
				if( sfd.ShowDialog() == DialogResult.OK)//Показать диалог, и если имена файлов выбраны
				{
					//Копирует временный файл в заданный пользователем
					File.Copy(tempOutFileNames[i], sfd.FileName, true);
				}
			}
			//Удаляем временные файлы
			for(int i=0; i<tempOutFileNames.Length; i++)//По всем именам временных файлов
        	{
				File.Delete(tempOutFileNames[i]);
			}
		}
	}

	//При выборе меню File\Exit
	private void MenuExit_Click(Object sender, EventArgs e)
	{
   		Application.Exit();
	}

	//Функция обработки и сохранения текста
	public string[] processText(string[] inFileNames)
	{
        Encoding enc = Encoding.GetEncoding(1251);
        //Минимальное количество символов в слове
		int minChars=getMinNumOfLetters();
		//Проверка необходимости удаления знаков препинания
		bool delPunct=(getDelPunctuationMarks()==CheckState.Checked)? true: false;
		FileStream fsIn;//Поток очередного входного файла
		StreamReader r;//Объект для чтения входного потока
		string[] tempOutFileNames=new string[inFileNames.Length];			
		FileStream fsOut ;//Поток очередного выходного файла
		StreamWriter w;//Объект для записи выходного потока
		string CurWord;//Текущее слово, читаемое из входного файла
		int charsInCurWord;//Зафиксированное текущее количество букв и цифр в текущем слове
		char c;//прочитанный из текущего входного файла текущий символ
		CurWord="";
		charsInCurWord=0;
		for(int i=0; i<inFileNames.Length; i++)//По всем именам входных файлов
        {
			fsIn = new FileStream(inFileNames[i], FileMode.Open, FileAccess.Read);//Поток очередного входного файла
            r = new StreamReader(fsIn, enc);
			tempOutFileNames[i]=inFileNames[i]+"_temp";
			fsOut = new FileStream(tempOutFileNames[i], FileMode.OpenOrCreate, FileAccess.Write);
            w = new StreamWriter(fsOut, enc);
			while (r.Peek() >= 0)//пока не конец входного файла (-1)
            {
                c=(char)r.Read();
				if(!(Char.IsPunctuation(c) || Char.IsSeparator(c) || Char.IsControl(c)))//если символ - часть слова
				{
					CurWord+=c.ToString();
					charsInCurWord++;
				}
				else//если знак препинания, разделитель или управляющий символ
				{
					if(charsInCurWord>=minChars)
					{
						w.Write(CurWord);//Запись считанного длинного слова в выходной файл
					}
					//либо знаки препинания не удаляются, либо считанный символ - не знак препинания
					if(!(Char.IsPunctuation(c) && delPunct))
					{
						w.Write(c);//Запись считанного символа в выходной файл				
					}
					//Подготовка к чтению следующего слова
					CurWord="";
					charsInCurWord=0;
				}
				
            }
			//Запись последнего длинного считанного слова
			if(charsInCurWord>=minChars)
			{
				w.Write(CurWord);
			}
			//Подготовка к чтению следующего файла
			CurWord="";
			charsInCurWord=0;
			w.Flush();//Очищает буферы и записывает их в устройство
			w.Close();
			fsOut.Close();
			r.Close();
			fsIn.Close();
		}
		return tempOutFileNames;
	}
}

//Класс с возможностью отслеживания ввода чисел 
class myTextBox : TextBox
{
	//Перегрузка функции обработки события изменения текста в TextBox
	protected override void OnTextChanged(EventArgs e)
	{
		string s=getMyTBStr();
		for(int i=0; i<s.Length; i++)
		{
			if(s[i]<'0' || s[i]>'9')
			{
				MessageBox.Show("Введите неотрицательное целое число","Минимальное количество букв в словах");
				this.Text="";
			}
		}		
	}

	//Получить текст в поле ввода минимального количества букв в словах
	public string getMyTBStr()
	{
		return this.Text;
	}	
}
