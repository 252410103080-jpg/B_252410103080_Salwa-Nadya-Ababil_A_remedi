using System;
using System.Collections.Generic;

class Sekolah
{
    public string idSekolah;
    public string namaSekolah;
    public int jumlahPaketDiterima;

    public Sekolah(string id, string nama)
    {
        idSekolah = id;
        namaSekolah = nama;
        jumlahPaketDiterima = 0;
    }

    public void TampilInfo()
    {
        Console.WriteLine("ID Sekolah: " + idSekolah);
        Console.WriteLine("Nama Sekolah: " + namaSekolah);
        Console.WriteLine("Jumlah Paket Diterima: " + jumlahPaketDiterima);
    }

    public void TambahPaketDiterima(int jumlah)
    {
        jumlahPaketDiterima += jumlah;
    }
}

abstract class PaketMBG
{
    private string idPaket;
    private string namaPaket;
    private int stokDapur;

    public string IdPaket
    {
        get { return idPaket; }
        set { idPaket = value; }
    }

    public string NamaPaket
    {
        get { return namaPaket; }
        set { namaPaket = value; }
    }

    public int StokDapur
    {
        get { return stokDapur; }
    }

    public PaketMBG(string id, string nama, int stok)
    {
        idPaket = id;
        namaPaket = nama;

        if (stok >= 0)
        {
            stokDapur = stok;
        }
        else
        {
            stokDapur = 0;
        }
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Gagal: Jumlah pengurangan harus lebih dari 0.");
            return false;
        }

        if (jumlah > stokDapur)
        {
            Console.WriteLine("Gagal: Stok dapur tidak mencukupi.");
            return false;
        }

        stokDapur -= jumlah;
        return true;
    }

    public abstract decimal HitungBiaya(int jumlah);
}

class PaketReguler : PaketMBG
{
    public PaketReguler(string id, string nama, int stok)
        : base(id, nama, stok)
    {
    }

    public override decimal HitungBiaya(int jumlah)
    {
        return jumlah * 15000;
    }
}

class PaketKhusus : PaketMBG
{
    public string jenisDiet;

    public PaketKhusus(string id, string nama, int stok, string diet)
        : base(id, nama, stok)
    {
        jenisDiet = diet;
    }

    public override decimal HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }
}

interface DapatDidistribusikan
{
    void KirimPaket();
}

class DistribusiMBG : DapatDidistribusikan
{
    private Sekolah sekolahTujuan;
    private PaketMBG paket;
    private int jumlah;

    public DistribusiMBG(Sekolah sekolah, PaketMBG paketMBG, int jumlahKirim)
    {
        sekolahTujuan = sekolah;
        paket = paketMBG;
        jumlah = jumlahKirim;
    }

    public void KirimPaket()
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Distribusi gagal: Jumlah paket harus lebih dari 0.");
            return;
        }

        if (jumlah > paket.StokDapur)
        {
            Console.WriteLine("Distribusi gagal: Stok dapur tidak mencukupi.");
            return;
        }

        bool berhasil = paket.KurangiStok(jumlah);

        if (berhasil)
        {
            sekolahTujuan.TambahPaketDiterima(jumlah);

            Console.WriteLine(
                "Distribusi MBG berhasil! Sebanyak " + jumlah +
                " paket makanan telah dikirim ke sekolah " +
                sekolahTujuan.namaSekolah
            );
        }
    }
}

class PengelolaMBG
{
    private List<Sekolah> daftarSekolah = new List<Sekolah>();
    private List<PaketMBG> daftarPaket = new List<PaketMBG>();

    public void TambahSekolah(Sekolah sekolah)
    {
        daftarSekolah.Add(sekolah);
    }

    public void TambahPaket(PaketMBG paket)
    {
        daftarPaket.Add(paket);
    }

    public void TampilkanDataMBG()
    {
        Console.WriteLine("\n=== DATA SEKOLAH ===");

        foreach (Sekolah sekolah in daftarSekolah)
        {
            sekolah.TampilInfo();
            Console.WriteLine();
        }

        Console.WriteLine("=== DATA PAKET MBG ===");

        foreach (PaketMBG paket in daftarPaket)
        {
            Console.WriteLine("ID Paket: " + paket.IdPaket);
            Console.WriteLine("Nama Paket: " + paket.NamaPaket);
            Console.WriteLine("Stok Dapur: " + paket.StokDapur);
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Sekolah sekolah1 = new Sekolah("S001", "SDN 1");
        Sekolah sekolah2 = new Sekolah("S002", "SDN 2");

        PaketMBG paket1 = new PaketReguler(
            "P001", "Paket Reguler", 100
        );

        PaketMBG paket2 = new PaketKhusus(
            "P002", "Paket Khusus", 50, "Rendah Gula"
        );

        Console.WriteLine("=== INFORMASI SEKOLAH ===");
        sekolah1.TampilInfo();

        Console.WriteLine();
        sekolah2.TampilInfo();


        Console.WriteLine("\n=== PERHITUNGAN BIAYA ===");

        Console.WriteLine(
            "Biaya Paket Reguler: Rp" +
            paket1.HitungBiaya(10).ToString("N0")
        );

        Console.WriteLine(
            "Biaya Paket Khusus: Rp" +
            paket2.HitungBiaya(10).ToString("N0")
        );


        Console.WriteLine("\n=== PROSES DISTRIBUSI ===");

        DistribusiMBG distribusi1 = new DistribusiMBG(
            sekolah1, paket1, 20
        );

        distribusi1.KirimPaket();

        DistribusiMBG distribusi2 = new DistribusiMBG(
            sekolah2, paket2, 60
        );

        distribusi2.KirimPaket();

        PengelolaMBG pengelola = new PengelolaMBG();

        pengelola.TambahSekolah(sekolah1);
        pengelola.TambahSekolah(sekolah2);

        pengelola.TambahPaket(paket1);
        pengelola.TambahPaket(paket2);

        pengelola.TampilkanDataMBG();

    }
}
