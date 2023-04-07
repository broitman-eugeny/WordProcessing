//Пространства имен
using System;//Здесь содержится набор основных классов, и мы здесь создаем свой класс
using System.Text;
using System.Windows.Forms;

//Класс текстбокса для ввода неотрицательных целых чисел
class NonNegativeIntegersTextBox : TextBox
{
    //Перегрузка функции обработки события изменения текста в TextBox
    protected override void OnTextChanged(EventArgs e)
    {
        StringBuilder s = getMyTBStr();
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] < '0' || s[i] > '9')
            {
                MessageBox.Show("Введите неотрицательное целое число", System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                this.Text = "";
            }
        }
    }

    //Получить текст в поле ввода минимального количества букв в словах
    public StringBuilder getMyTBStr()
    {
        return new StringBuilder(this.Text);
    }

    //Получить численное значение NonNegativeIntegersTextBox
    public int getIntValNNITB()
    {
        return (this.Text == "") ? 0 : Convert.ToInt32(this.Text);
    }    
}
