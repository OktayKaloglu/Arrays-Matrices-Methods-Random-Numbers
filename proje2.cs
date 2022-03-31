using System;
using System.Collections;
using System.Collections.Generic;

namespace Proje2
{
    class Program
    {
        class Data//işlem kolaylığı için verilerin bilgilerini tutması için class yazdım.
        {
            public double varyans = 0.0, carpıklık = 0.0, basıklık = 0.0, entropi = 0.0, distance = 0;
            public int tur = 0, knnTur = 0;

            public Data(double dis = 0, double var = 0, double car = 0, double bas = 0, double entr = 0, int t = 0, int knn = -1)
            {//knn -1 daha hesaplanmadığını ifade etmektedir.
                this.distance = dis;
                this.varyans = var;
                this.carpıklık = car;
                this.basıklık = bas;
                this.entropi = entr;
                this.tur = t;
                this.knnTur = knn;
            }

            public String toString()//nesnenin değerlerine kolay erişim sağlanmıştır.
            {
                return this.distance + " " + this.varyans + " " + this.carpıklık + " " + this.basıklık + " " + this.entropi + " " + this.tur;
            }
        }

        static Data CreateData(String str)//her girilen satır için verilerin data sınıfına dönüştürülmesi
        {
            char[] spearator = { ',' };
            String[] strlist = str.Split(spearator, StringSplitOptions.RemoveEmptyEntries);//girişi yapılan satırların , leri sayesinde ayırılıp gereken verilerin nesneye eklenmesi
            Data dat;

            try
            {
                dat = new Data(0, double.Parse(strlist[0]), double.Parse(strlist[1]), double.Parse(strlist[2]), double.Parse(strlist[3]), int.Parse(strlist[4]), -1);
            }
            catch (IndexOutOfRangeException) { dat = new Data(); }
            return dat;
        }
        static (List<Data>, List<Data>, List<Data>) GetData()
        {
            List<Data> dataSet = new List<Data>();
            List<Data> testSet = new List<Data>();
            List<Data> allSet = new List<Data>();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\t-okt\OneDrive\Masaüstü\c#lows\KNN\Datar.txt");//veri dosyasının yolu girilmelidir


            int endofzereos = 0;//tür değişiminin gerçekletiği index
            for (int i = 0; i < lines.Length; i++)
            {
                Data dat1 = CreateData(lines[i]);

                if (!dat1.toString().Equals("0 0 0 0 0 0"))//0,0,0,0 döndürülürse hatalı bilgi vardır eklenmemelidir.
                {
                    allSet.Add(dat1);

                    if (dat1.tur == 1 && endofzereos == 0)//verilerin türü 1 e döndüğü an
                    {
                        endofzereos = i; //i şuanda türü bir olan ilk datanın indexini temsil ediyor
                    }

                }
            }
            //gerekli veri setlerinin oluşturulması için sıradaki 4 for kullanııldı
            for (int i = 0; i < endofzereos - 100; i++)  //0-1 tür değişimim olduğu noktanın 100 öncesine kadar bütün değerler veri setine eklenmekte          
            {

                dataSet.Add(allSet[i]);

            }

            for (int i = endofzereos; i < allSet.Count - 100; i++)//0-1 değişimi olduktan sonra listenin 100 öncesine kadar değerler verisetine eklenmektedir.
            {
                dataSet.Add(allSet[i]);
            }


            for (int i = endofzereos - 100; i < endofzereos; i++)///0-1 değişimi olduğu andan 100 öncesi test verilerine eklenmektedir.
            {
                testSet.Add(allSet[i]);
            }
            for (int i = allSet.Count - 100; i < allSet.Count; i++)//listenin bitiminden 100 öncesinden başlayarak test verilerine eklenmektedir.
            {
                testSet.Add(allSet[i]);
            }


            return (allSet, dataSet, testSet);
        }

        static List<Data> KNNAlgo(Data dat, int k, List<Data> set)
        {

            MyList linkedList = new MyList();//basit bir linked list yazdım
            for (int i = 0; i < set.Count; i++)
            {
                set[i].distance = Math.Sqrt(Math.Pow(set[i].varyans - dat.varyans, 2) + Math.Pow(set[i].carpıklık - dat.carpıklık, 2) + Math.Pow(set[i].basıklık - dat.basıklık, 2) + Math.Pow(set[i].entropi - dat.entropi, 2));
                linkedList.add(set[i]);//sıralanarak ekleniyor araya item ekleneceği zaman bütün itemlerin indexlerinin arttırılmasına gerek kalmıyor
            }
            return linkedList.returnList(k, dat);// girilen datanın knn ile türünün bulunması ve Linkedlist yerine işlem kolaylığı için List<data> sınıfından bir liste döndürüyor


        }

        class MyList
        {
            public Node head;
            public int count = 0;
            internal class Node
            {
                internal Data dat;
                internal Node prev;
                internal Node next;
                public Node(Data d)
                {
                    this.dat = d;
                    this.prev = null;
                    this.next = null;
                }

            }

            public void add(Data d)//zincirler oluşturulurken girilen  değerler önceden oluşturulmuş zincirler baştan sona büyük küçük kontrolü yapılarak eklenecek yeri aranır ve bulunan yere gerekli referansları değiştirilerek eklenir.
            {
                if (d != null)//hata kontrolü
                {
                    Node tempNode = head;//zincirde ilerlemede yardımcı olması için
                    Node data = new Node(d);
                    if (head != null)
                    {

                        while (true)
                        {
                            if (tempNode.dat.distance < d.distance)//test edilen zincir giriş yapılan değerden küçükse bir sonraki zincirin incelenmesi gerekmektedir.
                            {
                                if (tempNode.next != null)
                                {
                                    tempNode = tempNode.next;//listenin dönülmesi
                                }
                                else//bir sonraki zincir boş ise mecburi olarak sona eklemelidir.
                                {
                                    count++;
                                    tempNode.next = data;
                                    data.prev = tempNode;
                                    break;
                                }
                            }
                            else//doğru yer bulundu ekleme yapılması lazım
                            {
                                if (tempNode != head)//ilk elemandan sonra eklenme
                                {
                                    Node current = tempNode;
                                    current = current.prev;
                                    data.prev = current;
                                    data.next = tempNode;
                                    current.next = data;
                                    tempNode.prev = data;
                                    break;
                                }
                                else//ilk elemanın önüne eklmek için
                                {
                                    tempNode.prev = data;
                                    data.next = tempNode;
                                    head = data;
                                    count++;
                                    break;
                                }
                            }
                        }

                    }
                    else//liste boşsa ise başa eklenmeli
                    {
                        head = data;
                        count++;
                    }
                }
            }

            public List<Data> returnList(int k, Data d)//zincir listenin baştan k ya kadar olanının geri döndürülmesi ve giriş datasının boğruluk değernin ortaya çıkartılması
            {
                List<Data> forReturn = new List<Data>(k);
                Node tempNode = head;
                double sum = 0;
                for (int i = 0; i < k && tempNode != null; i++)
                {
                    sum += tempNode.dat.tur;
                    forReturn.Add(tempNode.dat);
                    tempNode = tempNode.next;
                };
                if (sum / k > 0.5)//eğer kendisine yakın değerlerde doğru sayısı fazla ise
                {
                    d.knnTur = 1;
                }
                else if (sum / k < 0.5)
                {
                    d.knnTur = 0;
                }
                else// eğer sahte ve doğru tür sayısı eşitse baştaki elemanın değerindedir.
                {
                    d.knnTur = head.dat.tur;
                }
                return forReturn;
            }

            public void lListOut()//kontrol amaçlı link listin itemlerinin yazdırılması için 
            {
                Node temp = head;
                while (temp != null)
                {
                    Console.Out.WriteLine(temp.dat.toString());
                    temp = temp.next;
                }
            }



        }

        static void bankotSiniflandirma(List<Data> set)//kullanıcı giriş verilerinin veri seti ile karşılaştırılması
        {
            String[] degerler = { "varyans", "çarpıklık", "basıklık", "entropi" };
            while (true)
            {
                double[] data = new double[4];
                List<Data> rList;
                Console.WriteLine("Lütfen bir k değeri giriniz: ");
                int k = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine("Lütfen bir" + degerler[i] + "değeri giriniz: ");
                    data[i] = Convert.ToDouble(Console.ReadLine());
                }

                Data dat = new Data(0, data[0], data[1], data[2], data[3], -1, -1);//turu bilinmediği için tur yerine -1 girdim

                rList = KNNAlgo(dat, k, set);
                Console.Out.WriteLine("knn ile hesaplanan banknot türü: " + dat.knnTur);
                dataListOut(rList);

                Console.WriteLine("başka bir banknot için de işlem yapmak ister misiniz ?(e/E - h/H): ");
                string durum = Console.ReadLine();
                if (durum == "h" || durum == "H")
                {
                    break;
                }

            }
        }

        static void dataListOut(List<Data> set, int k = 0)//List<Data> türünden listenin yazdırılması

        {
            bool durum = k != 2;// en son verilerin tamamı yazdırılırken önceden hesaplanmış uzaklıkların yazdırılmasına gerek yoktur bunu engellemek için if döngülerinde kullandım.
            Console.Write(string.Format("{0,20:0.0}", "Varyans"));
            Console.Write(string.Format("{0,20:0.0}", "Çarpıklık"));
            Console.Write(string.Format("{0,20:0.0}", "Basıklık"));
            Console.Write(string.Format("{0,20:0.0}", "Entropi"));
            Console.Write(string.Format("{0,20:0.0}", "Tür"));
            if (durum)
            {
                Console.Write(string.Format("{0,20:0.0}", "Uzaklık"));
            }
            Console.Out.WriteLine("");
            for (int i = 0; i < set.Count; i++)
            {
                Console.Write(string.Format("{0,20}", set[i].varyans));
                Console.Write(string.Format("{0,20}", set[i].carpıklık));
                Console.Write(string.Format("{0,20}", set[i].basıklık));
                Console.Write(string.Format("{0,20}", set[i].entropi));
                Console.Write(string.Format("{0,20}", set[i].tur));
                if (durum)
                {
                    Console.Write(string.Format("{0,20:0.00000000}", set[i].distance));
                }
                Console.Out.WriteLine("");
            }
        }

        static void basariOlcumu(List<Data> testData, List<Data> dataSet)
        {

            while (true)
            {
                double dogruSay = 0;//knn ile hesaplanan türünün kendi türü ile karşılaştırılıp eşit çıkan değerler için bu sayaç kullanılmıştır.


                Console.WriteLine("Lütfen bir k değeri giriniz: ");
                int k = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("K " + k + " için hesaplanıyor");
                for (int i = 0; i < testData.Count; i++)
                {
                    List<Data> rList = KNNAlgo(testData[i], k, dataSet);//datanın iki tür verisi var biri knnTur diğer tur. KNNAlgo giriş yapılmış olan data nesnesine kendi hesapladığı knn verisini işlemektedir.
                    Console.WriteLine("Veri " + i + " asıl türü: " + testData[i].tur + " knn ile hesaplanan türü: " + testData[i].knnTur);
                    dataListOut(rList);//döndürülen en yakın k kadar verinin yazdırılması.
                    Console.WriteLine();
                    if (testData[i].tur == testData[i].knnTur)
                    {
                        dogruSay++;
                    }

                }

                //knn ile tahmin    gerçek tür



                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("Başarı oranı yüzde : %" + dogruSay / 2);//yüzde doğrusayısı/200 *100
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.WriteLine("başka bir k değeri için de işlem yapmak ister misiniz ?(e/E - h/H): ");
                string durum = Console.ReadLine();
                if (durum == "h" || durum == "H")
                {
                    break;
                }
            }

        }


        static void Main(string[] args)
        {
            List<Data> allSet;//bütün datalar
            List<Data> dataSet;//her türden son 100 verinin çıkarılmış hali
            List<Data> testSet;//200lü olan test verileri
            (allSet, dataSet, testSet) = GetData();//işlemlerde kullanılacak data setlerinin oluşturulması

            bankotSiniflandirma(allSet);
            basariOlcumu(testSet, dataSet);

            Console.WriteLine("Tüm verilerin çıktısı :");
            dataListOut(allSet, 2);
            Console.ReadKey();
        }


    }
}