using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncImageControl;

public class PathHelper
{
    public static string GetLocalDownloadDirectory()
    {
        return $"{System.AppDomain.CurrentDomain.BaseDirectory}\\LocalImages\\";
    }
}