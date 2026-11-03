using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

public class WslKeepAlive : ServiceBase
{
    const string Distro = "Ubuntu";
    Thread t; volatile bool stop; Process p;

    public WslKeepAlive() { ServiceName = "WslKeepAlive"; }

    protected override void OnStart(string[] a)
    {
        t = new Thread(Loop); t.IsBackground = true; t.Start();
    }

    void Loop()
    {
        while (!stop)
        {
            try
            {
                p = Process.Start(new ProcessStartInfo("wsl.exe", "-d " + Distro + " --exec sleep infinity")
                    { CreateNoWindow = true, UseShellExecute = false });
                p.WaitForExit();   // returns if wsl dies/shuts down
            }
            catch { }
            for (int i = 0; i < 5 && !stop; i++) Thread.Sleep(1000);
        }
    }

    protected override void OnStop()
    {
        stop = true;
        try { if (p != null && !p.HasExited) p.Kill(); } catch { }
    }

    public static void Main() { ServiceBase.Run(new WslKeepAlive()); }
}
