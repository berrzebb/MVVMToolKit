namespace MVVMToolKit.Helper.Native
{
    /// <summary>
    /// �ܺ� DLL ������ �̸��� ����� �����ϴ� Ŭ�����Դϴ�.<br/>
    /// </summary>
    internal class ExternDll
    {
        /// <summary>
        /// User32.dll ������ �̸��� ��Ÿ���ϴ�.<br/>
        /// �� DLL�� ����� �������̽��� �����ϴ� Windows API �Լ��� �����ϰ� �ֽ��ϴ�.
        /// </summary>
        public const string User32 = "user32.dll";
        /// <summary>
        /// Shcore.dll ������ �̸��� ��Ÿ���ϴ�.<br/>
        /// �� DLL�� ���ػ� ���÷��̸� �����ϴ� Windows API �Լ��� �����ϰ� �ֽ��ϴ�.
        /// </summary>
        public const string Shcore = "shcore.dll";
        /// <summary>
        /// D2D1.dll ������ �̸��� ��Ÿ���ϴ�.<br/>
        /// �� DLL�� 2D �׷��� �������� ���� Direct2D API �Լ��� �����ϰ� �ֽ��ϴ�.
        /// </summary>
        public const string D2D1 = "d2d1.dll";
    }
}
