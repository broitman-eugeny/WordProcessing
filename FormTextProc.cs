//Пространства имен
using System;//Здесь содержится набор основных классов, и мы здесь создаем свой класс
using System.Windows.Forms;//Для работы с формами
using System.Drawing;//Доступ к основным функциям GDI
using System.IO;//Системный ввод вывод (работа с файлами)
using System.Text;//Для Encoding

//Главная форма приложения обработки текста
class FormTextProc : Form
{
    private NonNegativeIntegersTextBox tbMinNumOfLetters;//Элемент ввода минимального количества букв в словах
    private CheckBox cbPunctuationMarks;//Элемент выбора удаления знаков препинания	

    public FormTextProc()
    {
        Text = "Программа обработки текста";//Заголовок окна программы

        //Меню пограммы
        MainMenu mnuFileMenu = new MainMenu();
        this.Menu = mnuFileMenu;
        MenuItem MenuItemFile = new MenuItem("&File");
        MenuItemFile.MenuItems.Add("&Open", new System.EventHandler(this.MenuOpen_Click));
        MenuItemFile.MenuItems.Add("E&xit", new System.EventHandler(this.MenuExit_Click));
        mnuFileMenu.MenuItems.Add(MenuItemFile);

        //Метка для элемента ввода минимального количества букв в словах
        Label labelMinNumOfLetters = new Label();
        labelMinNumOfLetters.Text = "Минимальное количество букв в словах:";
        labelMinNumOfLetters.Location = new Point(15, 15);
        labelMinNumOfLetters.AutoSize = true;
        labelMinNumOfLetters.TextAlign = ContentAlignment.BottomLeft;
        this.Controls.Add(labelMinNumOfLetters);

        //Элемент ввода минимального количества букв в словах
        tbMinNumOfLetters = new NonNegativeIntegersTextBox();
        tbMinNumOfLetters.Text = "3";
        tbMinNumOfLetters.Location = new Point(17 + labelMinNumOfLetters.Width, 15);
        tbMinNumOfLetters.Width = 30;
        tbMinNumOfLetters.Height = labelMinNumOfLetters.Height;
        tbMinNumOfLetters.TextAlign = HorizontalAlignment.Right;
        this.Controls.Add(tbMinNumOfLetters);

        //Метка для элемента выбора удаления знаков препинания
        Label labelPunctuationMarks = new Label();
        labelPunctuationMarks.Text = "Удалить знаки препинания:";
        labelPunctuationMarks.Location = new Point(15, 17 + tbMinNumOfLetters.Height);
        labelPunctuationMarks.AutoSize = true;
        labelPunctuationMarks.TextAlign = ContentAlignment.BottomLeft;
        this.Controls.Add(labelPunctuationMarks);

        //Элемент выбора удаления знаков препинания
        cbPunctuationMarks = new CheckBox();
        cbPunctuationMarks.Location = new Point(17 + labelPunctuationMarks.Width, 17 + tbMinNumOfLetters.Height);
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
        TextProcessor.OpenProcSaveFiles(getMinNumOfLetters(), getDelPunctuationMarks());
    }

    //При выборе меню File\Exit
    private void MenuExit_Click(Object sender, EventArgs e)
    {
        Application.Exit();
    }    
}