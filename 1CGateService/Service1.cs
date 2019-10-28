using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1CGateService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        Timer t;
        Timer t1;

        protected override void OnStart(string[] args)
        {
            t = new Timer(GateService.QueueProcess, null, 0, 20000);
            t1 = new Timer(GateService.StockBalanceProcess, null, 0, 180000);
        }

        protected override void OnStop()
        {
            t = null;
            t1 = null;
        }
    }
}
