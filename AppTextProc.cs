using System;
using System.Windows.Forms;//����� ���������� ����� �������� �������, � �� ����� ������� ���� �����

//����� ������� Main() ���������� ��������� ������
//��������� ��������� �� ���� ���� �� ����� (���� ��������� ������). �������-�� ����� ����������� � ���������:
//a. �������� ���� ������ ����� ������-���� ����� ��������;
//b. �������� ������ ����������.
//��������� ����������� � �������� ���� (���� �����).
//������� ����, �������� ����, ����� ���� � ������������� �������� ������ ���������� ������ ������������.

class AppTextProc : FormTextProc
{
    // Satisfies rule: MarkWindowsFormsEntryPointsWithStaThread.
    [STAThread]//������������ ����������
    public static void Main()
    {
        Application.Run(new FormTextProc());
    }
}