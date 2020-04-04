using System;
using System.Runtime.InteropServices;

namespace Utilities.Display
{
    public static class GDI
    {
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(
        IntPtr hdcDest,     // handle to destination DC (device context)
        int nXDest,         // x-coord of destination upper-left corner
        int nYDest,         // y-coord of destination upper-left corner
        int nWidth,         // width of destination rectangle
        int nHeight,        // height of destination rectangle
        IntPtr hdcSrc,      // handle to source DC
        int nXSrc,          // x-coordinate of source upper-left corner
        int nYSrc,          // y-coordinate of source upper-left corner
        int dwRop  // raster operation code
        );

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [DllImport("gdi32.dll")]
        public static extern void DeleteObject(IntPtr obj);

        [DllImport("gdi32.dll")]
        public static extern void DeleteDC(IntPtr obj);

        [DllImport("gdi32", EntryPoint = "StretchBlt")]
        public static extern int StretchBlt(
                IntPtr hdc,
                int x,
                int y,
                int nWidth,
                int nHeight,
                IntPtr hSrcDC,
                int xSrc,
                int ySrc,
                int nSrcWidth,
                int nSrcHeight,
                int dwRop
        );
    }
}