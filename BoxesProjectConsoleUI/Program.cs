
using BoxesProjectConsoleUI.Implementations;
using DependencyInjection.Abstractions;
using DependencyInjection.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI
{
    class Program
    {
        static void Main(string[] args) => new BoxesUI(new StartUp(), new InversionOfControlContainer()).Run();
    }
}
