using System;

public interface IKemampuan
{
    void GunakanKemampuan(Robot target);
    bool BisaDigunakan();
}

public abstract class Robot
{
    public string NamaRobot;  
    public int EnergiRobot;   
    public int ArmorRobot;    
    public int SeranganRobot; 

    public Robot(string nama, int energi, int armor, int serangan)
    {
        NamaRobot = nama;
        EnergiRobot = energi;
        ArmorRobot = armor;
        SeranganRobot = serangan;
    }

    public void Serang(Robot target)
    {
        int damage = Math.Max(0, SeranganRobot - target.ArmorRobot);
        target.EnergiRobot -= damage;
        Console.WriteLine($"{NamaRobot} menyerang {target.NamaRobot} dan menyebabkan {damage} damage.");
    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama Robot: {NamaRobot}, Energi: {EnergiRobot}, Armor: {ArmorRobot}, Serangan: {SeranganRobot}");
    }
}

public class RobotBiasa : Robot
{
    public RobotBiasa(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    {

    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        if (kemampuan.BisaDigunakan())
        {
            kemampuan.GunakanKemampuan(this);
        }
        else
        {
            Console.WriteLine($"{NamaRobot} tidak dapat menggunakan kemampuan ini.");
        }
    }
}

public class BosRobot : Robot
{
    public int PertahananBos;

    public BosRobot(string nama, int energi, int pertahanan, int serangan)
        : base(nama, energi, 2 * pertahanan, serangan)
    {
        PertahananBos = pertahanan;
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        if (kemampuan.BisaDigunakan())
        {
            kemampuan.GunakanKemampuan(this);
        }
        else
        {
            Console.WriteLine($"{NamaRobot} tidak dapat menggunakan kemampuan ini.");
        }
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.SeranganRobot - ArmorRobot;
        if (damage < 0) damage = 0;
        EnergiRobot -= damage;
        Console.WriteLine($"{NamaRobot} diserang oleh {penyerang.NamaRobot}, energinya berkurang sebesar {damage}. Energi tersisa {EnergiRobot}");

        if (EnergiRobot <= 0)
        {
            Mati();
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{NamaRobot} telah mati.");
    }
}

public class Perbaikan : IKemampuan
{
    private int cooldown;
    private int waktuCooldown;

    public Perbaikan(int cooldown)
    {
        this.cooldown = cooldown;
        this.waktuCooldown = 0;
    }

    public void GunakanKemampuan(Robot target)
    {
        target.EnergiRobot += 20;
        waktuCooldown = cooldown;
        Console.WriteLine($"{target.NamaRobot} menggunakan Perbaikan dan memulihkan 20 energi.");
    }

    public bool BisaDigunakan()
    {
        if (waktuCooldown > 0)
        {
            waktuCooldown--;
            return false;
        }
        return true;
    }
}

public class SeranganListrik : IKemampuan
{
    private int cooldown;
    private int waktuCooldown;

    public SeranganListrik(int cooldown)
    {
        this.cooldown = cooldown;
        this.waktuCooldown = 0;
    }

    public void GunakanKemampuan(Robot target)
    {
        target.EnergiRobot -= 15;
        waktuCooldown = cooldown;
        Console.WriteLine($"{target.NamaRobot} diserang dengan Serangan Listrik dan kehilangan 15 energi.");
    }

    public bool BisaDigunakan()
    {
        if (waktuCooldown > 0)
        {
            waktuCooldown--;
            return false;
        }
        return true;
    }
}

public class SeranganPlasma : IKemampuan
{
    private int cooldown;
    private int waktuCooldown;

    public SeranganPlasma(int cooldown)
    {
        this.cooldown = cooldown;
        this.waktuCooldown = 0;
    }

    public void GunakanKemampuan(Robot target)
    {
        int damage = 25;
        target.EnergiRobot -= damage;
        waktuCooldown = cooldown;
        Console.WriteLine($"{target.NamaRobot} diserang dengan Serangan Plasma dan kehilangan {damage} energi.");
    }

    public bool BisaDigunakan()
    {
        if (waktuCooldown > 0)
        {
            waktuCooldown--;
            return false;
        }
        return true;
    }
}
public class PertahananSuper : IKemampuan
{
    private int cooldown;
    private int waktuCooldown;

    public PertahananSuper(int cooldown)
    {
        this.cooldown = cooldown;
        this.waktuCooldown = 0;
    }

    public void GunakanKemampuan(Robot target)
    {
        target.ArmorRobot += 10; 
        waktuCooldown = cooldown; 
        Console.WriteLine($"{target.NamaRobot} menggunakan Pertahanan Super dan meningkatkan armor sebesar 10.");
    }

    public bool BisaDigunakan()
    {
        if (waktuCooldown > 0)
        {
            waktuCooldown--;
            return false;
        }
        return true;
    }
}

public class Permainan
{
    public void MulaiPertarungan(Robot robot1, Robot robot2)
    {
        while (robot1.EnergiRobot > 0 && robot2.EnergiRobot > 0)
        {
            robot1.CetakInformasi();
            robot2.CetakInformasi();

            robot1.Serang(robot2);
            if (robot2.EnergiRobot <= 0)
            {
                Console.WriteLine($"{robot2.NamaRobot} telah kalah!");
                break;
            }

            robot2.Serang(robot1);
            if (robot1.EnergiRobot <= 0)
            {
                Console.WriteLine($"{robot1.NamaRobot} telah kalah!");
                break;
            }

            robot1.EnergiRobot = Math.Min(robot1.EnergiRobot + 5, 100);
            robot2.EnergiRobot = Math.Min(robot2.EnergiRobot + 5, 150);

            Console.WriteLine($"{robot1.NamaRobot} memulihkan 5 energi, Energi sekarang: {robot1.EnergiRobot}");
            Console.WriteLine($"{robot2.NamaRobot} memulihkan 5 energi, Energi sekarang: {robot2.EnergiRobot}");
        }
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        Robot robot1 = new RobotBiasa("Robot A", 100, 20, 30);
        Robot robot2 = new BosRobot("Bos Robot B", 150, 30, 25);

        IKemampuan perbaikan = new Perbaikan(2);
        IKemampuan seranganListrik = new SeranganListrik(3);
        IKemampuan seranganPlasma = new SeranganPlasma(1);
        IKemampuan pertahananSuper = new PertahananSuper(2);

        robot1.GunakanKemampuan(perbaikan);
        robot2.GunakanKemampuan(seranganListrik);

        Permainan permainan = new Permainan();
        permainan.MulaiPertarungan(robot1, robot2);
    }
}

