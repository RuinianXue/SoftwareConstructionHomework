// See https://aka.ms/new-console-template for more information
public class Clock
{
    public event EventHandler Tick;
    public event EventHandler Alarm;
    public DateTime AlarmTime { get; set; }

    public Clock(DateTime alarmTime)
    {
        this.AlarmTime = alarmTime;
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        timer.Elapsed += (sender, e) =>
        {
            OnTick(); 

            if (DateTime.Now >= AlarmTime) 
            {
                OnAlarm();
                timer.Stop();
            }
        };
        timer.Start();
    }
    public virtual void OnTick()
    {
        if (Tick != null)
        {
            Tick(this, EventArgs.Empty);
            Console.WriteLine("Tick tick..."); 
        }

    }
    public virtual void OnAlarm()
    {
        if (Alarm != null)
        {
            Alarm(this, EventArgs.Empty); 
            Console.WriteLine("It rings！"); 
        }

    }
}

class Program
{
    static void Main(string[] args)
    {
        while(true)
        {
            System.Console.WriteLine("Please set your alarm time. (s)");
            int aT=5;//default time is 5seconds
            try
            {
                aT = int.Parse(System.Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            DateTime alarmTime = DateTime.Now.AddSeconds(aT);
            Clock clock = new Clock(alarmTime);
            clock.Tick += Clock_Tick;
            clock.Alarm += Clock_Alarm;
            Console.ReadLine();
        }
    }
    private static void Clock_Tick(object sender, EventArgs e)
    {
        Console.WriteLine("Now Time：{0}", DateTime.Now); 
    }

    private static void Clock_Alarm(object sender, EventArgs e)
    {
        Console.WriteLine("DiDiDi！");
    }
}