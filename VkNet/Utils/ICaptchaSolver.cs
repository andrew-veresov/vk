namespace VkNet.Utils
{
    /// <summary>
    /// ��������� �����������, ������������� �����.
    /// </summary>
    public interface ICaptchaSolver
    {
        /// <summary>
        /// ���������� ����� �����.
        /// </summary>
        /// <param name="url">������ �� ����������� �����.</param>
        /// <returns>������, ���������� �����, ������� ��� ����������� � �����.</returns>
        string Solve(string url);

        /// <summary>
        /// ��������, ��� ��������� ����� ���� ���������� �������.
        /// </summary>
        void CaptchaIsFalse();
    }
}