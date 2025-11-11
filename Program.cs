namespace inclassLottery;

class Ticket
{
    public int[] RegTickets { get; set; }
    public int PowerBall { get; set; }
    public Ticket(int[] numbers, int powerBall)
    {
        RegTickets = new int[5];
        RegTickets[0] = numbers[0];
        RegTickets[1] = numbers[1];
        RegTickets[2] = numbers[2];
        RegTickets[3] = numbers[3];
        RegTickets[4] = numbers[4];
        PowerBall = powerBall;
    }
    public Ticket()
    {
        RegTickets = new int[5];
        for (int i = 0; i < 5; i++)
        {
            RegTickets[i] = Random.Shared.Next(1, 70);
        }
        PowerBall = Random.Shared.Next(1, 27);
    }

    public int GetTicketWinnings(LotteryPeriod period)
    {
        List<int> sharedNumbers = RegTickets.Union(period.WinningTicket.RegTickets).ToList();
        bool powerBallSame = PowerBall == period.WinningTicket.PowerBall;

        if (powerBallSame)
        {
            switch (sharedNumbers.Count())
            {
                case 0: return 4;
                case 1: return 4;
                case 2: return 7;
                case 3: return 100;
                case 4: return 50_000;
                case 5: return 2_000_000;
                default: return 0;
            }

        }
        else
        {
            switch (sharedNumbers.Count())
            {
                case 3: return 7;
                case 4: return 100;
                case 5: return 1_000_000;
                default: return 0;
            }
        }
    }
}
class LotteryPeriod
{
    public Ticket WinningTicket { get; set; }
    public List<Ticket> SoldTickets { get; set; } = new List<Ticket>();
    public int TotalNumbOfZeroDollarTickets {get {return CountTicketsOfValue(0);}}
    public int TotalNumbOfFourDollarTickets {get {return CountTicketsOfValue(4);}}
    public int TotalNumbOfSevenDollarTickets {get {return CountTicketsOfValue(7);}}
    public int TotalNumbOfHundredDollarTickets {get {return CountTicketsOfValue(100);}}
    public int TotalNumbOfFiftyKTickets {get {return CountTicketsOfValue(50_000);}}
    public int TotalNumbOfOneMilTickets {get {return CountTicketsOfValue(1_000_000);}}
    public int TotalNumbOfJackpotTickets {get {return CountTicketsOfValue(2_000_000);}}

    public LotteryPeriod()
    {
        int[] numbers = new int[5] { 1, 2, 3, 4, 5 };
        SetWinningTicket(numbers, 6);

    }
    public void SetWinningTicket(int[] numbers, int powerBall)
    {
        WinningTicket = new Ticket(numbers, powerBall);
    }

    private int CountTicketsOfValue(int value)
    {
        return SoldTickets.Where(t => t.GetTicketWinnings(this) == value).Count();
    }
}
class LotteryVendor
{
    public LotteryVendor()
    {
    }
    public void SellTickets(LotteryPeriod period, int numberOfTickets)
    {
        for (int i = 0; i < numberOfTickets; i++)
        {
            Ticket ticket = new Ticket();
            period.SoldTickets.Add(ticket);
        }
    }
}
class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Lets sell 1Million Tickets!");
        LotteryPeriod period = new LotteryPeriod();
        LotteryVendor vendor1 = new LotteryVendor();
        LotteryVendor vendor2 = new LotteryVendor();
        LotteryVendor vendor3 = new LotteryVendor();
        Console.WriteLine("SOLD 1Million Tickets!");

        var thread1 = new Thread(() => { LotteryVendor vendor = new(); vendor.SellTickets(period, 10_000_000);});
        var thread2 = new Thread(() => { LotteryVendor vendor = new(); vendor.SellTickets(period, 10_000_000);});
        var thread3 = new Thread(() => { LotteryVendor vendor = new(); vendor.SellTickets(period, 10_000_000); });

        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();

        Console.WriteLine($"# of $0 Winners: {period.TotalNumbOfZeroDollarTickets}");
        Console.WriteLine($"# of $4 Winners: {period.TotalNumbOfFourDollarTickets}");
        Console.WriteLine($"# of $7 Winners: {period.TotalNumbOfSevenDollarTickets}");
        Console.WriteLine($"# of $100 Winners: {period.TotalNumbOfHundredDollarTickets}");
        Console.WriteLine($"# of $50,000 Winners: {period.TotalNumbOfFiftyKTickets}");
        Console.WriteLine($"# of $1,000,000 Winners: {period.TotalNumbOfOneMilTickets}");
        Console.WriteLine($"# of Jackpot Winners: {period.TotalNumbOfJackpotTickets}");
        //TODO: 1a) make 3 vendors sell 10M tickets each
        // 1b) 3 vendors sell tickets in parallel
        // 2) Modify Ticket class to be able to judge a winner level
        // 3) Gather statistics on how many winners of each level there are
        // 4) Print out the statistics
        // AFTER 1-4 is working, try to do (GatherStatistics) with Parallel Programming
    }

}
