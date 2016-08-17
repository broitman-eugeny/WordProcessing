//�������� ������� ������������ C#�
//������� �1
//��������� ��� ��������� ������

//������������ ����
using System;//����� ���������� ����� �������� �������, � �� ����� ������� ���� �����
using System.Windows.Forms;//��� ������ � �������
using System.Drawing;//������ � �������� �������� GDI
using System.IO;//��������� ���� ����� (������ � �������)
using System.Text;//��� Encoding

class MyForm : AppForm 
{
    // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
    [STAThread]//������������ ����������
    public static void Main()
	{
		Application.Run(new MyForm());
	}
}

class AppForm : Form
{
	private myTextBox tbMinNumOfLetters;//������� ����� ������������ ���������� ���� � ������
	private CheckBox cbPunctuationMarks;//������� ������ �������� ������ ����������	

	public AppForm()
	{
		Text="��������� ��������� ������";//��������� ���� ���������

		//���� ��������
		MainMenu mnuFileMenu = new MainMenu();
		this.Menu = mnuFileMenu;
		MenuItem MenuItemFile = new MenuItem("&File");
		MenuItemFile.MenuItems.Add("&Open",new System.EventHandler(this.MenuOpen_Click));
		MenuItemFile.MenuItems.Add("E&xit",new System.EventHandler(this.MenuExit_Click));
		mnuFileMenu.MenuItems.Add(MenuItemFile);
		
		//����� ��� �������� ����� ������������ ���������� ���� � ������
		Label labelMinNumOfLetters = new Label();
		labelMinNumOfLetters.Text = "����������� ���������� ���� � ������:";
		labelMinNumOfLetters.Location = new Point(15,15);
		labelMinNumOfLetters.AutoSize=true;
		labelMinNumOfLetters.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelMinNumOfLetters);

		//������� ����� ������������ ���������� ���� � ������
		tbMinNumOfLetters=new myTextBox();
		tbMinNumOfLetters.Text = "3";
		tbMinNumOfLetters.Location=new Point(17+labelMinNumOfLetters.Width,15);
		tbMinNumOfLetters.Width=30;
		tbMinNumOfLetters.Height=labelMinNumOfLetters.Height;
		tbMinNumOfLetters.TextAlign=HorizontalAlignment.Right;
		this.Controls.Add(tbMinNumOfLetters);

		//����� ��� �������� ������ �������� ������ ����������
		Label labelPunctuationMarks = new Label();
		labelPunctuationMarks.Text = "������� ����� ����������:";
		labelPunctuationMarks.Location = new Point(15,17+tbMinNumOfLetters.Height);
		labelPunctuationMarks.AutoSize=true;
		labelPunctuationMarks.TextAlign=ContentAlignment.BottomLeft;
		this.Controls.Add(labelPunctuationMarks);

		//������� ������ �������� ������ ����������
		cbPunctuationMarks=new CheckBox();
		cbPunctuationMarks.Location = new Point(17+labelPunctuationMarks.Width,17+tbMinNumOfLetters.Height);
		this.Controls.Add(cbPunctuationMarks);
	}

	//�������� ����������� ���������� ���� � ������
	public int getMinNumOfLetters()
	{
		return Convert.ToInt32(tbMinNumOfLetters.Text);
	}

	//�������� ������� ������ �������� ������ ����������
	public CheckState getDelPunctuationMarks()
	{
		return cbPunctuationMarks.CheckState;
	}
	
	//��� ������ ���� File\Open
	private void MenuOpen_Click(Object sender, EventArgs e)
	{
		OpenFileDialog ofd = new OpenFileDialog();//�������� ������� ������������ ������� �������� ����� 
		ofd.Title = "������� �����" ;//��������� ������� �������� �����
		//ofd.Filter = "All files (*.*)|*.*|Text (*.txt)|*.txt" ;//������� ����� ������
		ofd.Multiselect=true;//����������� ����� ���������� ������
		if( ofd.ShowDialog() == DialogResult.OK)//�������� ������, � ���� ����� �������
		{
			string[] tempOutFileNames;//������ ����� � ����������� ��������� �������� ������
			tempOutFileNames=processText(ofd.FileNames);	//��������� ������� ������ � ���������� ���������� ��
								                            //��������� ����� � ������� "..._temp"
			SaveFileDialog sfd=new SaveFileDialog();//�������� ������� ������������ ������� ���������� �����
			// ���������� ���������� ��������� ��������� ������
        	for(int i=0; i<ofd.SafeFileNames.Length; i++)//�� ���� ������ ������� ������ �� ������� �����
        	{
            	//��������� ������� ���������� �����
				sfd.Title = "��������� ��������� ��������� ����� "+ofd.SafeFileNames[i];
				if( sfd.ShowDialog() == DialogResult.OK)//�������� ������, � ���� ����� ������ �������
				{
					//�������� ��������� ���� � �������� �������������
					File.Copy(tempOutFileNames[i], sfd.FileName, true);
				}
			}
			//������� ��������� �����
			for(int i=0; i<tempOutFileNames.Length; i++)//�� ���� ������ ��������� ������
        	{
				File.Delete(tempOutFileNames[i]);
			}
		}
	}

	//��� ������ ���� File\Exit
	private void MenuExit_Click(Object sender, EventArgs e)
	{
   		Application.Exit();
	}

	//������� ��������� � ���������� ������
	public string[] processText(string[] inFileNames)
	{
        Encoding enc = Encoding.GetEncoding(1251);
        //����������� ���������� �������� � �����
		int minChars=getMinNumOfLetters();
		//�������� ������������� �������� ������ ����������
		bool delPunct=(getDelPunctuationMarks()==CheckState.Checked)? true: false;
		FileStream fsIn;//����� ���������� �������� �����
		StreamReader r;//������ ��� ������ �������� ������
		string[] tempOutFileNames=new string[inFileNames.Length];			
		FileStream fsOut ;//����� ���������� ��������� �����
		StreamWriter w;//������ ��� ������ ��������� ������
		string CurWord;//������� �����, �������� �� �������� �����
		int charsInCurWord;//��������������� ������� ���������� ���� � ���� � ������� �����
		char c;//����������� �� �������� �������� ����� ������� ������
		CurWord="";
		charsInCurWord=0;
		for(int i=0; i<inFileNames.Length; i++)//�� ���� ������ ������� ������
        {
			fsIn = new FileStream(inFileNames[i], FileMode.Open, FileAccess.Read);//����� ���������� �������� �����
            r = new StreamReader(fsIn, enc);
			tempOutFileNames[i]=inFileNames[i]+"_temp";
			fsOut = new FileStream(tempOutFileNames[i], FileMode.OpenOrCreate, FileAccess.Write);
            w = new StreamWriter(fsOut, enc);
			while (r.Peek() >= 0)//���� �� ����� �������� ����� (-1)
            {
                c=(char)r.Read();
				if(!(Char.IsPunctuation(c) || Char.IsSeparator(c) || Char.IsControl(c)))//���� ������ - ����� �����
				{
					CurWord+=c.ToString();
					charsInCurWord++;
				}
				else//���� ���� ����������, ����������� ��� ����������� ������
				{
					if(charsInCurWord>=minChars)
					{
						w.Write(CurWord);//������ ���������� �������� ����� � �������� ����
					}
					//���� ����� ���������� �� ���������, ���� ��������� ������ - �� ���� ����������
					if(!(Char.IsPunctuation(c) && delPunct))
					{
						w.Write(c);//������ ���������� ������� � �������� ����				
					}
					//���������� � ������ ���������� �����
					CurWord="";
					charsInCurWord=0;
				}
				
            }
			//������ ���������� �������� ���������� �����
			if(charsInCurWord>=minChars)
			{
				w.Write(CurWord);
			}
			//���������� � ������ ���������� �����
			CurWord="";
			charsInCurWord=0;
			w.Flush();//������� ������ � ���������� �� � ����������
			w.Close();
			fsOut.Close();
			r.Close();
			fsIn.Close();
		}
		return tempOutFileNames;
	}
}

//����� � ������������ ������������ ����� ����� 
class myTextBox : TextBox
{
	//���������� ������� ��������� ������� ��������� ������ � TextBox
	protected override void OnTextChanged(EventArgs e)
	{
		string s=getMyTBStr();
		for(int i=0; i<s.Length; i++)
		{
			if(s[i]<'0' || s[i]>'9')
			{
				MessageBox.Show("������� ��������������� ����� �����","����������� ���������� ���� � ������");
				this.Text="";
			}
		}		
	}

	//�������� ����� � ���� ����� ������������ ���������� ���� � ������
	public string getMyTBStr()
	{
		return this.Text;
	}	
}