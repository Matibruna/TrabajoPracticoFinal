using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoPracticoFinal
{
    static class Program
    {
        /// Author: Bruna Matias Mail & GitHub: matibruna6@gmail.com
        /// Trabajo Final Simulacion Universidad Tecnologica Nacional, Facultad Regional Cordoba.
        /// Legajo 77682.
        /// 
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
