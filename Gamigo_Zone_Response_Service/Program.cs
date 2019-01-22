using System.ServiceProcess;

namespace Gamigo_Zone_Response_Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Main()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
