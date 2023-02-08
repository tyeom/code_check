namespace Transparent_Window
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public static class WindowsServices
    {
        /// <summary>
        /// 투명 윈도우 스타일
        /// </summary>
        const int WS_EX_TRANSPARENT = 0x00000020;
        /// <summary>
        /// 계층화 윈도우 스타일 및 클래스 스타일 적용 <para/>
        /// Window8 버전 이하에서는 최상위 윈도우에서만 지원됨
        /// </summary>
        const int WS_EX_LAYERED = 0x00080000;
        /// <summary>
        /// 새로운 확장 윈도우 스타일 설정
        /// </summary>
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            // 기존 윈도우 스타일 정보를 가져오기
            //var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            if (AppSetting.Instance.IsMouseEventMessagePass)
            {
                SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            }
            else
            {
                SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED);
            }
        }
    }
}
