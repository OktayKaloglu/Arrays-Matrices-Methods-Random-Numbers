using System;

namespace Proje1

{
    class Points
    {
        class Point //asıl amaç rastgele oluşturulan verinin bellekte bir nesnede tutulması
        {
            public double x = 0.0;
            public double y = 0.0;
            public Point(double x1 , double y1)
            {
                this.x = x1;
                this.y = y1;
            }
        }
        private int n = 0;
        private int width=0;
        private int height= 0;
        private Point[] points ;
        private double[,] theMatrix;
        public Points(int num , int wid, int hei){
            
            this.n = num;
            this.width = wid;
            this.height = hei;
            points = fillWithPoints();
            outPutPoints();
            theMatrix = distanceMatrix();
            outPutDistanceMatrix();
        }
        
        private Point[] fillWithPoints()
        {
            Point[] tempPoints = new Point[n];
            for (int i = 0; i < n; i++)
            {
                Random random = new Random();
                double x1 = Convert.ToDouble(random.Next(0, height * 10))/10;//format olarak gereken değer aralığında 0.0 formatında noktalar oluşturmakta
                double y1 = Convert.ToDouble(random.Next(0, width * 10))/10;//bu yöntemi tercih etmemin asıl sebebi ise gerekenden fazla basamakta değer oluşturulmasına gerek kalmamakta ve bu sayede işlemlerden önce 0.0 hailne formatlamaya gerek kalmıyor.
                Point pointi = new Point(x1,y1) ;
                tempPoints[i] = pointi;
            }
            return tempPoints;
        }

        private void outPutPoints() {
            for (int i = 0; i < n; i++)
            {
                Console.Write("point " + (i+1) + ": (");
                Console.Write(points[i].x);
                Console.Write(",");
                Console.Write(points[i].y);
                Console.Write(")");
                Console.WriteLine("");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public double[,] distanceMatrix()
        {
            //n*n kere uzaklık hesaplaması yapılması ve hali hazırda veri girişi yapılmış olan indexlere tekrar tekrar veri girişi yapılması engellenmiştir.
            //her nokta kendisi ve kendisinden önce gelen noktalara uzaklığını hesaplayarak matrisin geri kalan alanlarınıda sorunsuz doldurmaktadır.
            //uzaklık hesaplaması n*n  yerine  (1+2+3+4.....n) yani (n*(n+1)/2) kere yapılarak lüzumsuz işlem gücü tüketiminden kaçınılmıştır.
            double[,] disMat = new double[n,n];
            double dist = 0.0;
            for(int i = 0; i<n; i++)
            {
               for(int j =0; j <= i; j++)
                {
                    
                    dist = distanceCalculator(points[i], points[j]);
                    disMat[i,j]=dist;
                    disMat[j, i] = dist;
                }
            }
            double distanceCalculator(Point p1, Point p2) {
                return Math.Sqrt( Math.Pow((p1.x-p2.x),2)+ Math.Pow((p1.y - p2.y), 2)) ;
            }
            return disMat;
        }
        public void outPutDistanceMatrix()
        {
            string spc = new string(' ', 6*(n / 2));
            Console.WriteLine(string.Format(spc+"Distance Matrix"));
            Console.Write("      ");
            for (int i=0;i<n;i++)
            {
                Console.Write(string.Format("{0,6:0.0}", "n" + i, "  "));
            }
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                Console.Write(string.Format("{0,6:0.0}", "n"+i, "  "));

                for (int j = 0; j < n; j++)
                {
                    Console.Write(string.Format("{0,6:0.0}",  theMatrix[i, j] ,"  "));
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }


    class Program
   {
       static void Main(string[] args)
       {
            Points pt = new Points(5, 100, 100);
            
            Points pt1 = new Points(10, 100, 100);

            Console.ReadKey();
        }
   }
}
