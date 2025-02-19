using FlashNotify;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;

namespace FlashNotify;

public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Microsoft.UI.Xaml.Application.Start((p) =>
        {
            var app = new App();
        });
    }
}
